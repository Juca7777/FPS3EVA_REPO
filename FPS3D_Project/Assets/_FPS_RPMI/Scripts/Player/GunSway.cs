using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSway : MonoBehaviour
{
    [Header("References")]
    public FPSController playerController;
    public Rigidbody playerRb;

    [Header("Sway")]
    public float idleAmount = 0.002f;
    public float walkAmount = 0.005f;
    public float sprintAmount = 0.01f;
    public float crouchAmount = 0.0015f;

    [Header("Rotation")]
    public float rotationAmount = 0.3f;

    [Header("Smooth")]
    public float smooth = 12f;

    private Vector3 initialPos;
    private Quaternion initialRot;

    private Vector2 mouseInput;
    private float bobTimer;

    void Start()
    {
        initialPos = transform.localPosition;
        initialRot = transform.localRotation;

        if (playerRb == null && playerController != null)
            playerRb = playerController.GetComponent<Rigidbody>();
    }

    void Update()
    {
        //  mouse input (reducido)
        if (Mouse.current != null)
            mouseInput = Mouse.current.delta.ReadValue() * 0.15f;

        float speed = new Vector3(playerRb.linearVelocity.x, 0f, playerRb.linearVelocity.z).magnitude;

        bool crouch = playerController != null && playerController.IsCrouching;
        bool sprint = playerController != null && playerController.IsSprinting;

        // =========================
        //  ESTADOS
        // =========================
        float amount;
        float bobSpeed;

        if (crouch)
        {
            amount = crouchAmount;
            bobSpeed = 6f;
        }
        else if (sprint)
        {
            amount = sprintAmount;
            bobSpeed = 12f;
        }
        else if (speed > 0.1f)
        {
            amount = walkAmount;
            bobSpeed = 9f;
        }
        else
        {
            amount = idleAmount;
            bobSpeed = 3f;
        }

        // =========================
        //  BOB POR ESTADO
        // =========================
        float bobOffset = 0f;

        if (speed > 0.1f)
        {
            bobTimer += Time.deltaTime * bobSpeed;
            bobOffset = Mathf.Sin(bobTimer) * amount;
        }
        else
        {
            bobTimer = 0f;
        }

        // =========================
        //  POSITION SWAY
        // =========================
        Vector3 targetPos = initialPos;

        targetPos += new Vector3(
            -mouseInput.x,
            -mouseInput.y,
            0f
        ) * amount;

        targetPos.y += bobOffset;

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            targetPos,
            Time.deltaTime * smooth
        );

        // =========================
        //  ROTATION SWAY
        // =========================
        Quaternion targetRot = initialRot * Quaternion.Euler(
            -mouseInput.y * rotationAmount,
            mouseInput.x * rotationAmount,
            0f
        );

        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            targetRot,
            Time.deltaTime * smooth
        );
    }
}