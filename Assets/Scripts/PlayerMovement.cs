using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private LayerMask groundMask = 1;
    [SerializeField] private float groundCheckDistance = 1.1f;
    
    private Rigidbody rb;
    private Camera playerCamera;
    private float xRotation = 0f;
    private bool isGrounded;
    public bool frozen;

    void Start()
    {
        frozen = false;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        
        rb.linearDamping = 1f;
        rb.angularDamping = 5f;
        
        rb.useGravity = true;
        
        SetupCamera();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void SetupCamera()
    {
        if (Camera.main == null)
        {
            GameObject cameraObj = new GameObject("Player Camera");
            playerCamera = cameraObj.AddComponent<Camera>();
            cameraObj.transform.SetParent(transform);
            cameraObj.transform.localPosition = new Vector3(0, 0.5f, 0);
        }
        else
        {
            playerCamera = Camera.main;
            playerCamera.transform.SetParent(transform);
            playerCamera.transform.localPosition = new Vector3(0, 0.5f, 0);
        }
    }

    void Update()
    {
        if (frozen) return;
        HandleMouseLook();
        HandleInput();
        CheckGrounded();
    }

    void FixedUpdate()
    {
        if (frozen) return;
        HandleMovement();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = transform.right * horizontal + transform.forward * vertical;
        direction.Normalize();
        Vector3 moveForce = direction * moveSpeed;
        rb.linearVelocity = new Vector3(moveForce.x, rb.linearVelocity.y, moveForce.z);
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        transform.Rotate(Vector3.up * mouseX);
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void HandleInput()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursor();
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void CheckGrounded()
    {
        Vector3 rayStart = transform.position + Vector3.up * 0.1f;
        isGrounded = Physics.Raycast(rayStart, Vector3.down, groundCheckDistance, groundMask);
        
        Debug.DrawRay(rayStart, Vector3.down * groundCheckDistance, isGrounded ? Color.green : Color.red);
    }

    private void ToggleCursor()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; 
            Cursor.visible = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector3 rayStart = transform.position + Vector3.up * 0.1f;
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawRay(rayStart, Vector3.down * groundCheckDistance);
    }
}
