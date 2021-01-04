using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    [Header("Movement")]
    private float speed = 12f;
    [SerializeField]
    private float gravity = -9.81f;
    [SerializeField]
    private float jumpheight = 3f;
    [SerializeField]
    private float sprintSpeed = 2f;
    Vector3 characterVelocity;

    [SerializeField]
    [Header("Rotation")]
    private float mouseSensivity = 100f;
    float xRotation = 0f;

    [SerializeField]
    [Header("Jump")]
    private float jumpForce = 9f;
    [SerializeField]
    private float groundDistance;
    [SerializeField]
    private LayerMask groundmask;

    [Header("Crouch")]
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 playerScale;
    [SerializeField]
    private float slideForce = 400;

    [Header("Extra")]
    public Transform groundcheck;
    public Transform playerCam;
    public Transform player;

    public bool isGrounded { get; private set; }
    public bool isCrouching { get; private set; }

    CharacterController m_Controller;
    PlayerInput m_playerInput;

    private void Start()
    {
        m_Controller = GetComponent<CharacterController>();
        m_playerInput = GetComponent<PlayerInput>();

        playerScale = transform.localScale;
    }

    private void Update()
    {
        Movement();
        MouseRotation();
        Crouch();
    }

    //Player Movement
    void Movement()
    {
        //What is Ground
        isGrounded = Physics.CheckSphere(groundcheck.position, groundDistance, groundmask);

        //Velocity Of Character
        if (isGrounded && characterVelocity.y < 0)
        {
            characterVelocity.y = -2f;
        }

        //Inputs
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        //Move Character
        m_Controller.Move(move * speed * Time.deltaTime);
        characterVelocity.y += gravity * Time.deltaTime;
        m_Controller.Move(characterVelocity * Time.deltaTime);

        //Jump
        if (m_playerInput.GetJumpInputDown() && isGrounded)
        {
            characterVelocity.y = Mathf.Sqrt(jumpheight * -2f * gravity);
        }

        //Sprint
        if (m_playerInput.GetSprintInputHeld())
        {
            m_Controller.Move(move * speed * sprintSpeed * Time.deltaTime);
        }
    }


    //Camera and Player Rotation
    void MouseRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensivity * Time.deltaTime;

        //Invert Camera Y and Stop Camera at 90
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Rotation
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);
    }

    //Crouch
    void Crouch()
    {
        //When Crouch Key is Pressed
        if (m_playerInput.GetCrouchInputDown())
        {
            transform.localScale = crouchScale;
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        }
        //When Crouch Key is Released
        if (m_playerInput.GetCrouchInputReleased())
        {
            transform.localScale = playerScale;
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        }
    }

}
