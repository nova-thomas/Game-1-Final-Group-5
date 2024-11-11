using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Actor
{
    public int ammo = 6;
    public int ammoMax = 6;
    public int healthMax = 20;
    public bool canJump;
    public bool canShoot = true;
    private Vector2 moveInput;
    private Vector2 lookInput;
    public float lookSpeed = 1f;
    public float jumpForce = 5f;
    public GameObject bulletPrefab;
    public GameObject iceBulletPrefab;
    public GameObject fireBulletPrefab;
    public GameObject poisonBulletPrefab;
    public Transform shooter;
    public float bulletSpeed = 20f;
    public Inventory inventory;

    private float xRotation = 0f;
    public Transform playerCamera;
    private Rigidbody rb;

    private GameObject nearbyDoor;
    private string nearbyDoorTag;
    private Shop nearbyShop;

    private bool isUsingIceBullet = false;
    private bool isUsingFireBullet = false;
    private bool isUsingPoisonBullet = false;

    public Shop shop;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        inventory = GetComponent<Inventory>();
        speed = 5;
        ammo = 6;
        canJump = true;
        canShoot = true;
        LockCursor();
    }

    void Update()
    {
        Vector3 moveDirection = transform.forward * moveInput.y + transform.right * moveInput.x;
        transform.position += moveDirection * speed * Time.deltaTime;
        LookAround();
    }

    public void Move(InputAction.CallbackContext mv)
    {
        if (mv.phase == InputActionPhase.Performed)
        {
            moveInput = mv.ReadValue<Vector2>();
        }
        else if (mv.phase == InputActionPhase.Canceled)
        {
            moveInput = Vector2.zero;
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

    public void Fire(InputAction.CallbackContext fr)
    {
        if (fr.phase == InputActionPhase.Started && canShoot && ammo > 0)
        {
            if (shop.shopUI.activeSelf) 
            {
                return; 
            }

            Debug.Log("Fire button pressed.");
            Shoot();
            ammo--;
            Debug.Log("Ammo left: " + ammo);

            if (ammo <= 0)
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
        yield return new WaitForSeconds(2); // Adjust reload time as needed
        ammo = ammoMax;
        canShoot = true;
    }

    public void ActivateIceBullet(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started && inventory.icePower > 0)
        {
            StartCoroutine(SwitchToIceBullet());
        }
    }

    public void ActivateFireBullet(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started && inventory.firePower > 0)
        {
            StartCoroutine(SwitchToFireBullet());
        }
    }

    public void ActivatePoisonBullet(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started && inventory.poisonPower > 0)
        {
            StartCoroutine(SwitchToPoisonBullet());
        }
    }

    private IEnumerator SwitchToIceBullet()
    {
        isUsingIceBullet = true;
        inventory.icePower--;
        yield return new WaitForSeconds(5);
        isUsingIceBullet = false;
    }

    private IEnumerator SwitchToFireBullet()
    {
        isUsingFireBullet = true;
        inventory.firePower--;
        yield return new WaitForSeconds(5);
        isUsingFireBullet = false;
    }

    private IEnumerator SwitchToPoisonBullet()
    {
        isUsingPoisonBullet = true;
        inventory.poisonPower--;
        yield return new WaitForSeconds(5);
        isUsingPoisonBullet = false;
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
        }
        else if (other.CompareTag("Red Key"))
        {
            inventory.AddKey("Red");
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Green Key"))
        {
            inventory.AddKey("Green");
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Coin"))
        {
            int coinAmount = Random.Range(10, 21); // Generates a random amount between 10 and 20
            inventory.AddCoin(coinAmount);
            Destroy(other.gameObject);
        }

        else if (other.CompareTag("Fire Power"))
        {
            inventory.AddFirePower();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Ice Power"))
        {
            inventory.AddIcePower();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Poison Power"))
        {
            inventory.AddPoisonPower();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Shop"))
        {
            nearbyShop = other.GetComponent<Shop>();
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
                if (nearbyDoorTag == "Blue Door" && inventory.HasKey("Blue"))
            {
                Destroy(nearbyDoor);
                inventory.RemoveKey("Blue");
                Debug.Log("Blue door opened and destroyed!");
            }
            else if (nearbyDoorTag == "Red Door" && inventory.HasKey("Red"))
            {
                Destroy(nearbyDoor);
                inventory.RemoveKey("Red");
                Debug.Log("Red door opened and destroyed!");
            }
            else if (nearbyDoorTag == "Green Door" && inventory.HasKey("Green"))
            {
                Destroy(nearbyDoor);
                inventory.RemoveKey("Green");
                Debug.Log("Green door opened and destroyed!");
            }
            else
            {
                Debug.Log("Cannot open door: Missing key or wrong door type.");
            }
            }
            else if (nearbyShop != null)
            {
                nearbyShop.Interact();
                UnlockCursor();
            }
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

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
