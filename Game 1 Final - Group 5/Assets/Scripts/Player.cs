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
    public float lookSpeed = 1f; 
    public float jumpForce = 5f; 
    private float xRotation = 0f;
    
    public Transform playerCamera;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    { 
        rb = GetComponent<Rigidbody>();
        speed = 5;
        ammo = 6;
        canJump = true;
        Cursor.lockState = CursorLockMode.Locked;  //cursor stays in the middle of the screen
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
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
        //rotate camera with cursor movement, lock rotation to (-)90 degrees when going up or down 
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

    }

    public void Interact(InputAction.CallbackContext it)
    {

    }
}
