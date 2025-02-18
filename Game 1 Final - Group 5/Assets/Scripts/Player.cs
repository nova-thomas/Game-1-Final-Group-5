using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Player : Actor
{
    public bool canJump;
    public bool canShoot = true;
    private bool isSprinting = false;
    private Vector2 moveInput;
    private Vector2 lookInput;
    public float lookSpeed = 1f;
    public float jumpForce = 5f;

    public double iceUpgrade = 1;
    public double fireUpgrade = 1;
    public double poisonUpgrade = 1; 

    public GameObject bulletPrefab;
    public GameObject iceBulletPrefab;
    public GameObject fireBulletPrefab;
    public GameObject poisonBulletPrefab;
    public Transform shooter;
    public float bulletSpeed = 20f;
    public Inventory inventory;
    public GameObject deathScreenUI;
    public GameObject pauseScreenUI;
    public GameObject keyButtonUI;
    public Transform playerCamera;
    public Shop shop;

    public AudioClip coinPickupSound;
    public AudioClip doorOpenSound;
    public AudioClip bulletFireSound;
    public AudioClip reloadSound;
    public AudioClip keyCollectSound;
    public AudioClip bulletTypeSwitchSound;
    public AudioClip powerUpCollectSound;

    private float xRotation = 0f;
    
    private Rigidbody rb;

    private GameObject nearbyDoor;
    private string nearbyDoorTag;
    private Shop nearbyShop;

    private bool isUsingIceBullet = false;
    private bool isUsingFireBullet = false;
    private bool isUsingPoisonBullet = false;

    private AudioSource audioSource;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI ammoText;

    public float damageInterval;
    private float damageTimer = 0f;

    private Animator playerAnimator;
    public GameObject playerModel;

    public Material targetMaterial;
    public Texture emissionNormal; 
    public Texture emissionFire;
    public Texture emissionPoison;
    public Texture emissionIce;

    public GameObject damageCanvas;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        inventory = GetComponent<Inventory>();
        audioSource = GetComponent<AudioSource>();
        playerAnimator = playerModel.GetComponent<Animator>();
        speed = 8;
        canJump = true;
        canShoot = true;
        LockCursor();
        deathScreenUI = GameObject.Find("DeathCanvas");
        deathScreenUI.SetActive(false);

        pauseScreenUI = GameObject.Find("PauseCanvas");
        pauseScreenUI.SetActive(false);

        isSprinting = GameManager.Instance.AutoSprintEnabled;
        keyButtonUI.SetActive(false);

        ChangeEmissionTexture(emissionNormal);
    }

    void Update()
    {
        Vector3 moveDirection = transform.forward * moveInput.y + transform.right * moveInput.x;
        float currentSpeed = isSprinting || GameManager.Instance.AutoSprintEnabled ? speed * 1.75f : speed;
        transform.position += moveDirection * currentSpeed * Time.deltaTime;

        LookAround();
        UpdateHealthUI();
        UpdateAmmoUI();


        if (GameManager.Instance.health <= 0)
        {
            GameManager.Instance.health = 0; 
            Die();
        }
    }

    public void Move(InputAction.CallbackContext mv)
    {
        if (mv.phase == InputActionPhase.Performed)
        {
            moveInput = mv.ReadValue<Vector2>();
            playerAnimator.SetBool("isWalking", true);
        }
        else if (mv.phase == InputActionPhase.Canceled)
        {
            moveInput = Vector2.zero;
            playerAnimator.SetBool("isWalking", false);
        }
    }

    public void Look(InputAction.CallbackContext lk)
    {
        if (lk.phase == InputActionPhase.Performed)
        {
            lookInput = lk.ReadValue<Vector2>() * lookSpeed;
        }
    }

    private void LookAround()
    {
        // Only rotate camera when the cursor is locked
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            transform.Rotate(Vector3.up * lookInput.x * Time.deltaTime);
            xRotation -= lookInput.y * Time.deltaTime;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            lookInput = Vector2.zero;
        }
    }

    public void Jump(InputAction.CallbackContext jm)
    {
        if (jm.phase == InputActionPhase.Started && canJump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canJump = false;
        }
    }

    new private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy Toxic") || collision.gameObject.CompareTag("Enemy Fire"))
        {
            TakeDamage(1);
        }
    }

    public void Fire(InputAction.CallbackContext fr)
    {
        if (fr.phase == InputActionPhase.Started && canShoot && GameManager.Instance.ammo > 0)
        {
            if (shop.shopUI.activeSelf) 
            {
                return; 
            }

            Debug.Log("Fire button pressed.");
            Shoot();
            if (bulletFireSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(bulletFireSound);
            }
            GameManager.Instance.ammo--;
            UpdateAmmoUI();
            Debug.Log("Ammo left: " + GameManager.Instance.ammo);

            if (GameManager.Instance.ammo <= 0)
            {
                Debug.Log("Out of ammo. Reloading...");
                StartCoroutine(Reload());
            }
        }
    }

    public void ToggleShooting(bool state)
    {
        canShoot = state;
    }

    private void Shoot()
    {
        GameObject currentBulletPrefab = isUsingIceBullet ? iceBulletPrefab :
                                         isUsingFireBullet ? fireBulletPrefab :
                                         isUsingPoisonBullet ? poisonBulletPrefab : bulletPrefab;

        if (currentBulletPrefab != null && shooter != null)
        {
            GameObject bullet = Instantiate(currentBulletPrefab, shooter.position, shooter.rotation);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

            if (bulletRb != null)
            {
                bulletRb.velocity = shooter.forward * bulletSpeed;
            }
        }
    }

    private IEnumerator Reload()
    {
        canShoot = false;
        if (reloadSound != null && audioSource != null)
        {
            audioSource.pitch = reloadSound.length / 2f; 
            audioSource.PlayOneShot(reloadSound);
        }
        yield return new WaitForSeconds(2);
        // Adjust reload time as needed
        GameManager.Instance.ammo = GameManager.Instance.ammoMax;
        UpdateAmmoUI();
        canShoot = true;
        audioSource.pitch = 1f;
    }
    public void ReloadManual(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started && GameManager.Instance.ammo < GameManager.Instance.ammoMax)
        {
            StartCoroutine(Reload());
        }
    }

    public void Sprint(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
        {
            isSprinting = true;  
        }
        else if (ctx.phase == InputActionPhase.Canceled)
        {
            isSprinting = false;  
        }
    }
    private void PlayBulletTypeSwitchSound()
    {
        if (bulletTypeSwitchSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(bulletTypeSwitchSound);
        }
    }

    public void ActivateIceBullet(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started && GameManager.Instance.icePower > 0)
        {
            StartCoroutine(SwitchToIceBullet());
        }
    }

    public void ActivateFireBullet(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started && GameManager.Instance.firePower > 0)
        {
            StartCoroutine(SwitchToFireBullet());
        }
    }

    public void ActivatePoisonBullet(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started && GameManager.Instance.poisonPower > 0)
        {
            StartCoroutine(SwitchToPoisonBullet());
        }
    }

    private IEnumerator SwitchToIceBullet()
    {
        isUsingIceBullet = true;
        ChangeEmissionTexture(emissionIce);
        GameManager.Instance.icePower--;
        UpdatePowerUpUI();
        PlayBulletTypeSwitchSound();
        yield return new WaitForSeconds(5);
        isUsingIceBullet = false;
        ChangeEmissionTexture(emissionNormal);
        PlayBulletTypeSwitchSound();
    }

    private IEnumerator SwitchToFireBullet()
    {
        isUsingFireBullet = true;
        ChangeEmissionTexture(emissionFire);
        GameManager.Instance.firePower--;
        UpdatePowerUpUI();
        PlayBulletTypeSwitchSound();
        yield return new WaitForSeconds(5);
        isUsingFireBullet = false;
        ChangeEmissionTexture(emissionNormal);
        PlayBulletTypeSwitchSound();
    }

    private IEnumerator SwitchToPoisonBullet()
    {
        isUsingPoisonBullet = true;
        ChangeEmissionTexture(emissionPoison);
        GameManager.Instance.poisonPower--;
        UpdatePowerUpUI();
        PlayBulletTypeSwitchSound();
        yield return new WaitForSeconds(5);
        isUsingPoisonBullet = false;
        ChangeEmissionTexture(emissionNormal);
        PlayBulletTypeSwitchSound();
    }

    private void UpdatePowerUpUI()
    {
        // Delegate the UI update to the Inventory script since it controls the visuals
        var inventory = FindObjectOfType<Inventory>();
        if (inventory != null)
        {
            inventory.UpdatePowerUpUI();
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Blue Door") || other.CompareTag("Red Door") || other.CompareTag("Green Door"))
        {
            nearbyDoor = other.gameObject;
            nearbyDoorTag = other.tag;
            Debug.Log("Player is near a " + nearbyDoorTag + ".");
        }
        else if (other.CompareTag("Blue Key"))
        {
            inventory.AddKey("Blue");
            Destroy(other.gameObject);
            PlayKeyCollectSound();
            ShowKeyButton();
        }
        else if (other.CompareTag("Red Key"))
        {
            inventory.AddKey("Red");
            Destroy(other.gameObject);
            PlayKeyCollectSound();
            ShowKeyButton();
        }
        else if (other.CompareTag("Green Key"))
        {
            inventory.AddKey("Green");
            Destroy(other.gameObject);
            PlayKeyCollectSound();
            ShowKeyButton();
        }
        else if (other.CompareTag("Coin"))
        {
            int coinAmount = Random.Range(10, 21); // Generates a random amount between 10 and 20
            inventory.AddCoin(coinAmount);
            if (coinPickupSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(coinPickupSound);

            }
            Destroy(other.gameObject);


        }
        else if (other.CompareTag("Fire Power"))
        {
            inventory.AddFirePower();
            Destroy(other.gameObject);
            PlayPowerUpCollectSound();

        }
        else if (other.CompareTag("Ice Power"))
        {
            inventory.AddIcePower();
            Destroy(other.gameObject);
            PlayPowerUpCollectSound();
        }
        else if (other.CompareTag("Poison Power"))
        {
            inventory.AddPoisonPower();
            Destroy(other.gameObject);
            PlayPowerUpCollectSound();
        }
        else if (other.CompareTag("Shop"))
        {
            nearbyShop = other.GetComponent<Shop>();
        }
        else if (other.CompareTag("Enemy Ice"))
        {
            // Attempt to get the IceProjectile component from the triggering object
            IceProjectile iceProjectile = other.gameObject.GetComponent<IceProjectile>();

            // Check if the component exists to avoid null reference errors
            if (iceProjectile != null)
            {
                TakeDamage(iceProjectile.damage);
            }
        }
        else if (other.CompareTag("Hitbox"))
        {
            TakeDamage(7);
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        damageTimer += Time.fixedDeltaTime;

        if (other.CompareTag("Enemy Fire"))
        {
            FireProjectile fireProjectile = other.gameObject.GetComponent<FireProjectile>();

            if (fireProjectile != null && damageTimer >= damageInterval)
            {
                // Damage over time from fireProjectile.damage
                TakeDamage(fireProjectile.damage);
                damageTimer = 0f;
            }
        }
        else if (other.CompareTag("Enemy Toxic"))
        {
            ToxicProjectile toxicProjectile = other.gameObject.GetComponent<ToxicProjectile>();

            if (toxicProjectile != null && damageTimer >= damageInterval)
            {
                // Damage over time from toxicProjectile.damage
                TakeDamage(toxicProjectile.damage);
                damageTimer = 0f;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Blue Door") || other.CompareTag("Red Door") || other.CompareTag("Green Door"))
        {
            nearbyDoor = null;
            nearbyDoorTag = null;
            Debug.Log("Player has left the vicinity of the door.");
        }
        else if (other.CompareTag("Shop"))
        {
            nearbyShop = null;
        }
    }

    public void Interact(InputAction.CallbackContext it)
    {
        if (it.phase == InputActionPhase.Started)
        {
            if (nearbyDoor != null)
            {
                bool doorOpened = false;

                if (nearbyDoorTag == "Blue Door" && inventory.HasKey("Blue"))
                {
                    Destroy(nearbyDoor);
                    inventory.RemoveKey("Blue");
                    doorOpened = true;
                    Debug.Log("Blue door opened and destroyed!");
                }
                else if (nearbyDoorTag == "Red Door" && inventory.HasKey("Red"))
                {
                    Destroy(nearbyDoor);
                    inventory.RemoveKey("Red");
                    doorOpened = true;
                    Debug.Log("Red door opened and destroyed!");
                }
                else if (nearbyDoorTag == "Green Door" && inventory.HasKey("Green"))
                {
                    Destroy(nearbyDoor);
                    inventory.RemoveKey("Green");
                    doorOpened = true;
                    Debug.Log("Green door opened and destroyed!");
                }
                else
                {
                    Debug.Log("Cannot open door: Missing key or wrong door type.");
                }

                if (doorOpened && doorOpenSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(doorOpenSound);
                }

                HideKeyButton();
            }
            else if (nearbyShop != null)
            {
                nearbyShop.Interact();
                UnlockCursor();
            }
        }
    }

    private void PlayKeyCollectSound()
    {
        if (keyCollectSound != null && audioSource != null)
        {
            audioSource.pitch = keyCollectSound.length / 2f; 
            audioSource.PlayOneShot(keyCollectSound);
            audioSource.pitch = 1f; 
        }
    }

    public void BackFromShop()
    {
        LockCursor();
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Die()
    {
        deathScreenUI.SetActive(true);
        UnlockCursor();
        DisablePlayerControls();
        Time.timeScale = 0f; 
    }

    public void Respawn(Vector3 respawnPosition)
    {
        deathScreenUI.SetActive(false);
        LockCursor();
        EnablePlayerControls();
        transform.position = respawnPosition; 
        GameManager.Instance.health = GameManager.Instance.healthMax;
        GameManager.Instance.ammo = GameManager.Instance.ammoMax;    
        UpdateHealthUI();
        UpdateAmmoUI();
        Time.timeScale = 1f; 
    }

    public  void DisablePlayerControls()
    {
        canShoot = false;
        canJump = false;
        isSprinting = false;
        moveInput = Vector2.zero; 
        lookInput = Vector2.zero;
    }

    public void EnablePlayerControls()
    {
        canShoot = true;
        canJump = true;
    }
    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = GameManager.Instance.health.ToString() + "/" + GameManager.Instance.healthMax.ToString();
        }
    }
    private void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = GameManager.Instance.ammo.ToString() + "/" + GameManager.Instance.ammoMax.ToString();
        }
    }

    private void PlayPowerUpCollectSound()
    {
        if (powerUpCollectSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(powerUpCollectSound);
        }
    }

    public void Pause(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            if (pauseScreenUI.activeSelf)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        pauseScreenUI.SetActive(true);
        UnlockCursor();
        canShoot = false;
        canJump = false;
        moveInput = Vector2.zero; 
        Time.timeScale = 0; //freeze
    }

    private void ResumeGame()
    {
        pauseScreenUI.SetActive(false);
        LockCursor();
        canShoot = true;
        canJump = true;
        Time.timeScale = 1; //unfreeze
    }

    private void TakeDamage(double amount)
    {
        GameManager.Instance.health -= (int)amount;
        if (damageCanvas != null)
        {
            StartCoroutine(FlashDamageCanvas());
        }
        if (GameManager.Instance.health <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashDamageCanvas()
    {
        damageCanvas.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        damageCanvas.SetActive(false);
    }

    public void ToggleSprintingState(bool isAutoSprintEnabled)
    {
        isSprinting = isAutoSprintEnabled;
    }
    private void ShowKeyButton()
    {
        if (keyButtonUI != null)
        {
            keyButtonUI.SetActive(true); 
        }
    }

    private void HideKeyButton()
    {
        if (keyButtonUI != null)
        {
            keyButtonUI.SetActive(false); 
        }
    }

    void ChangeEmissionTexture(Texture newTexture)
    {
        if (newTexture != null)
        {
            targetMaterial.SetTexture("_EmissionMap", newTexture);
        }
        else
        {
            Debug.LogWarning("New texture is null!");
        }
    }
}
