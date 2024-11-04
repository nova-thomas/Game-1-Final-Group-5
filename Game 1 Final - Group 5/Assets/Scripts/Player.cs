using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Actor
{
    public int ammo = 6;
    public bool canJump;
    public bool canShoot = true;
    public bool redKey;
    public bool greenKey;
    public bool blueKey;
    private Vector2 moveInput;
    private Vector2 lookInput;
    public float lookSpeed = 1f;
    public float jumpForce = 5f;
    public GameObject bulletPrefab; // Assign this in Inspector
    public Transform shooter;       // Assign the Shooter empty object in Inspector
    public float bulletSpeed = 20f;

    private float xRotation = 0f;
    public Transform playerCamera;
    private Rigidbody rb;

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
            // Instantiate the bullet and set its position and rotation to match the shooter's
            GameObject bullet = Instantiate(bulletPrefab, shooter.position, shooter.rotation);

            // Apply force in the forward direction
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                bulletRb.velocity = shooter.forward * bulletSpeed; // Move bullet forward from shooter
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

    public void Interact(InputAction.CallbackContext it)
    {
        // Implement interaction logic
    }
}
