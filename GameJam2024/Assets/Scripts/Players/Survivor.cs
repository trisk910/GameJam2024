using UnityEngine;

public class Survivor : MonoBehaviour, IDamageable
{
    [Header("Movement Settings")]
    public float speed = 5f;
   // public float jumpHeight = 1.5f;
    public float mouseSensitivity = 2f;
    public Transform cameraTransform;
   // public LayerMask groundMask;
   // public float groundCheckDistance = 0.2f;
    private CharacterController characterController;
    private float verticalVelocity = 0f;
    private float gravity = -9.81f;
    private float rotationX = 0f;
   // private bool isGrounded;

    [Header("Combat Settings")]
    public float Health { get; set; }
    public float MaxHealth { get; set; }
    public float pushForce = 3f;
    public float pushRate = 0.5f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        MaxHealth = 100f;
        Health = MaxHealth;
    }

    void Update()
    {
        //GroundCheck();
        MovePlayer();
        RotateCamera();
    }
    //Movement
    /*private void GroundCheck()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask);
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }
    }*/

    private void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        characterController.Move(move * speed * Time.deltaTime);

       /* if (isGrounded && Input.GetButtonDown("Jump"))
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }*/
        verticalVelocity += gravity * Time.deltaTime;
        characterController.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }

    private void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate the Survivor object horizontally
        transform.Rotate(Vector3.up * mouseX);

        // Update the vertical rotation for the camera
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        // Set the camera's local rotation based on the clamped vertical rotation
        cameraTransform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }

    //Combat
    public void TakeDamage(float amount)
    {
        Health -= amount;
        if (Health <= 0)
        {
            Health = 0;
            Die();
        }
        UnityEngine.Debug.Log($"{gameObject.name} took {amount} damage, remaining health: {Health}");
    }
    public void Heal(float amount)
    {
        Health += amount;
        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }
        UnityEngine.Debug.Log($"{gameObject.name} healed by {amount}, current health: {Health}");
    }

    public bool IsDead()
    {
        return Health <= 0;
    }

    private void Die()
    {
        UnityEngine.Debug.Log($"{gameObject.name} has died.");
    }

}
