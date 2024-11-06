using UnityEngine;

public class Survivor : MonoBehaviour
{
    public float speed = 5f;
    public float jumpHeight = 1.5f;
    public float mouseSensitivity = 2f;
    public Transform cameraTransform;
    public LayerMask groundMask;
    public float groundCheckDistance = 0.2f;

    private CharacterController characterController;
    private float verticalVelocity = 0f;
    private float gravity = -9.81f;
    private float rotationX = 0f;
    private bool isGrounded;

    public float followSpeed = 5f;
    public float rotationSpeed = 10f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        GroundCheck();
        MovePlayer();
        RotateCamera();
    }

    private void GroundCheck()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask);
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }
    }

    private void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        characterController.Move(move * speed * Time.deltaTime);

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

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
}
