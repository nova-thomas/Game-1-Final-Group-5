using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Actor
{
    public int ammo;
    public bool canJump;
    public bool canShoot;
    public bool redKey;
    public bool greenKey;
    public bool blueKey;
    private Vector2 moveInput;
    private Vector2 lookInput;
    public float lookSpeed = 1f;  // Adjust the sensitivity of camera look
    public float jumpForce = 5f;  // Force of the jump
    private float xRotation = 0f;
    
    [SerializeField] private Transform playerCamera;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        speed = 5;
        ammo = 6;
        canJump = true;
        Cursor.lockState = CursorLockMode.Locked;  // Locks cursor to center and hides it
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate direction based on the player's facing direction
        Vector3 moveDirection = transform.forward * moveInput.y + transform.right * moveInput.x;

        // Apply movement
        transform.position += moveDirection * speed * Time.deltaTime;

        // Apply camera rotation
        LookAround();
    }

    public void Move(InputAction.CallbackContext mv)
    {
        // Get input from the InputAction (e.g., from a joystick or WASD keys)
        if (mv.phase == InputActionPhase.Performed)
        {
            moveInput = mv.ReadValue<Vector2>();
        }
        else if (mv.phase == InputActionPhase.Canceled)
        {
            moveInput = Vector2.zero; // Stop movement when input is released
        }
    }

    public void Look(InputAction.CallbackContext lk)
    {
        // Get mouse input for camera look only when input is being actively performed
        if (lk.phase == InputActionPhase.Performed)
        {
            lookInput = lk.ReadValue<Vector2>() * lookSpeed;
        }
    }

    private void LookAround()
    {
        // Rotate player body on the Y axis (left/right rotation)
        transform.Rotate(Vector3.up * lookInput.x * Time.deltaTime);

        // Rotate camera on the X axis (up/down rotation)
        xRotation -= lookInput.y * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);  // Limit vertical rotation to prevent over-rotation
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Reset look input to zero so rotation stops when the cursor stops moving
        lookInput = Vector2.zero;
    }

    public void Jump(InputAction.CallbackContext jm)
    {
        // Check if player is allowed to jump
        if (jm.phase == InputActionPhase.Started && canJump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canJump = false;  // Disable jump until the player is back on the ground
        }
    }

   new private void OnCollisionEnter(Collision collision)
    {
        // Re-enable jumping when the player touches the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true;
        }
    }

    public void Fire(InputAction.CallbackContext fr)
    {

    }

    public void Interact(InputAction.CallbackContext it)
    {

    }
}
