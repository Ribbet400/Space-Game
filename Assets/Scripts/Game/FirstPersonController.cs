using UnityEngine;
using MLAPI;

[RequireComponent(typeof(GravityBody))]
public class FirstPersonController : NetworkBehaviour
{
    public float sensitivityX;
    public float sensitivityY;

    public float walkSpeed = 5;
    public float jumpForce = 200;
    public float superJumpForce = 2000;

    public LayerMask groundedMask;
    public bool isGrounded;

    Vector3 moveAmount;
    Vector3 smoothMoveVelocity;
    float verticalLookRotation;

    Transform cameraTransform;
    Rigidbody rb;

    public bool inGame = true;

    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb = GetComponent<Rigidbody>();
        cameraTransform = GetComponentInChildren<Camera>().transform;

        if (IsLocalPlayer)
        {
            // ???
        }
        else
        {
            cameraTransform.GetComponent<Camera>().enabled = false;
            cameraTransform.GetComponent<AudioListener>().enabled = false;
        }
    }
        
    // Update is called once per frame
    void Update()
    {
        if (IsLocalPlayer && inGame)
        {
            Look();
            Move();
            Jump();
        }
    }

    void Look()
    {
        // look
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Time.deltaTime * sensitivityX);
        verticalLookRotation += Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivityY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -60, 60);
        cameraTransform.localEulerAngles = Vector3.left * verticalLookRotation;
    }
    
    void Move()
    {
        // movement
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = new Vector3(inputX, 0, inputY).normalized;
        Vector3 targetMoveAmount = moveDirection * walkSpeed;
        moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, 0.15f);
    }

    void Jump()
    {
        // jump
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                rb.AddForce(transform.up * jumpForce);
            }
        }

        // super jump (to transfer planets)
        else if (Input.GetKeyDown(KeyCode.Q)) //  should change to button later, add cooldown etc.
        {
            if (isGrounded)
            {
                rb.AddForce(transform.up * superJumpForce);
            }
        }

        // grounded check
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1 + .1f, groundedMask)) //  1 is distance from capsule centre
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        // Application of movement
        Vector3 localMove = transform.TransformDirection(moveAmount) * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + localMove);
    }
}
