using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Actor
{
    public int ammo = 6;
    public int coins = 0;
    public bool canJump;
    public bool canShoot = true;
    public bool redKey;
    public bool greenKey;
    public bool blueKey;
    private Vector2 moveInput;
    private Vector2 lookInput;
    public float lookSpeed = 1f;
    public float jumpForce = 5f;
    public GameObject bulletPrefab; 
    public Transform shooter;      
    public float bulletSpeed = 20f;

    private float xRotation = 0f;
    public Transform playerCamera;
    private Rigidbody rb;

    private GameObject nearbyDoor;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = 5;
        ammo = 6;
        canJump = true;
        canShoot = true;
        Cursor.lockState = CursorLockMode.Locked;
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
        transform.Rotate(Vector3.up * lookInput.x * Time.deltaTime);
        xRotation -= lookInput.y * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        lookInput = Vector2.zero;
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

    private void Shoot()
    {
        if (bulletPrefab != null && shooter != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, shooter.position, shooter.rotation);

            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                bulletRb.velocity = shooter.forward * bulletSpeed;
            }
            else
            {
                Debug.LogWarning("Bullet prefab does not have a Rigidbody component!");
            }
        }
        else
        {
            Debug.LogWarning("Bullet prefab or shooter is not assigned!");
        }
    }


    private IEnumerator Reload()
    {
        canShoot = false;
        yield return new WaitForSeconds(2); // Adjust reload time as needed
        ammo = 6;
        canShoot = true;
    }

    //checking if we're by a door 
    void OnTriggerEnter(Collider other)
    {
        // Check if the player is near the blue door
        if (other.CompareTag("Blue Door"))
        {
            nearbyDoor = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        //clear if the player leaves the vicinity of the door
        if (other.CompareTag("Blue Door"))
        {
            nearbyDoor = null;
        }
    }
    public void Interact(InputAction.CallbackContext it)
    {
        if (it.phase == InputActionPhase.Started && blueKey && nearbyDoor != null)
        {
            Destroy(nearbyDoor);
            Debug.Log("Blue door opened!"); //remove
            blueKey = false; 
        }
    }
}
