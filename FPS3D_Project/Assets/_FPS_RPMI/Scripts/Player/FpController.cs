using UnityEngine;
using UnityEngine.InputSystem;

public class FpController : MonoBehaviour
{
    #region General Varialbes
    [Header("Movement & Look")]
    [SerializeField] GameObject camHolder;
    [SerializeField] float speed = 5f;
    [SerializeField] float sprintSpeed = 8f;
    [SerializeField] float crouchSpeed = 3f;
    [SerializeField] float maxForce = 1f;
    [SerializeField] float sensibility = 0.1f;

    [Header("Jump & GroundCheck")]
    [SerializeField] float jumpForce = 5f;
    [SerializeField] bool isGrounded;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.3f;
    [SerializeField] LayerMask groundLayer;

    [Header("Player State Bools")]
    [SerializeField] bool isSprinting;
    [SerializeField] bool isCrouching;

    #endregion

    //Variables de referencia privadas
    Rigidbody rb;

    //Variables para el input
    Vector2 moveInput;
    Vector2 lookInput;
    float lookRotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Lock del cursor del raton
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //GroundCheck
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        //Dibujar rayo ficticio en escena para determianr la pos de la camara
        Debug.DrawRay(camHolder.transform.position, camHolder.transform.forward * 100, Color.purple);
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void LateUpdate()
    {
        CameraLook();
    }

    void CameraLook()
    {
        //Rotacion horizontal del cuerpo del personaje
        transform.Rotate(Vector3.up * lookInput.x * sensibility);
        //Rotacion vertical de la camara
        lookRotation += (-lookInput.y * sensibility);
        lookRotation = Mathf.Clamp(lookRotation, -90, 90);
        camHolder.transform.localEulerAngles = new Vector3(lookRotation, 0f, 0f);
    }

    void Movement()
    {
        Vector3 currentVelocity = rb.linearVelocity; // calcular la velocidad acutal del rb constantemente
        Vector3 targetVelocity = new Vector3(moveInput.x, 0, moveInput.y); //velocidad a alcanzar 
        targetVelocity *= isCrouching ? crouchSpeed : (isSprinting ? sprintSpeed : speed);
        //convertir direccion local en global

        targetVelocity = transform.TransformDirection(targetVelocity);

        //calcular el cambio de velocidad(aceleracion)
        Vector3 velocityChange = (targetVelocity - currentVelocity);
        velocityChange = new Vector3(velocityChange.x, 0f, velocityChange.z);
        velocityChange = Vector3.ClampMagnitude(velocityChange, maxForce);

        //Aplicar la fuerza de movimiento/aceleracion
        rb.AddForce(velocityChange, ForceMode.VelocityChange);

    }

    void Jump()
    {
        if (isGrounded) rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    #region
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed) Jump();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            isCrouching = !isCrouching;
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed && !isCrouching) isSprinting = true;
        if (context.canceled) isSprinting = false; 
    }
    #endregion
}
