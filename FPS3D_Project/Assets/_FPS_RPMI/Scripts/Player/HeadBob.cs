using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    public FPSController playerController;
    public Camera cam;

    [Header("Headbob")]
    public float walkSpeed = 10f;
    public float runSpeed = 15f;
    public float crouchSpeed = 6f;

    public float walkIntensity = 0.04f;
    public float runIntensity = 0.07f;
    public float crouchIntensity = 0.02f;

    [Header("Lean")]
    public float leanAngle = 6f;
    public float leanSpeed = 6f;

    [Header("FOV")]
    public float normalFov = 60f;
    public float runFov = 75f;
    public float fovSpeed = 5f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip paso;

    private float tiempo;
    private Vector3 posicionInicial;
    private float ultimoPaso;

    void Start()
    {
        posicionInicial = transform.localPosition;

        if (rb == null)
            rb = GetComponentInParent<Rigidbody>();
    }

    void Update()
    {
        float velocidadActual = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z).magnitude;

        bool moviendose = velocidadActual > 0.1f;

        bool isCrouching = playerController != null && playerController.IsCrouching;
        bool isSprinting = playerController != null && playerController.IsSprinting;

        // =========================
        //  INTENSIDAD HEADBOB
        // =========================
        float speed;
        float intensity;

        if (isCrouching)
        {
            speed = crouchSpeed;
            intensity = crouchIntensity;
        }
        else if (velocidadActual >= runSpeed || isSprinting)
        {
            speed = runSpeed;
            intensity = runIntensity;
        }
        else
        {
            speed = walkSpeed;
            intensity = walkIntensity;
        }

        // Sprint boost extra
        if (isSprinting)
            intensity *= 1.2f;

        // =========================
        //  HEADBOB
        // =========================
        if (moviendose)
        {
            tiempo += Time.deltaTime * speed;

            float offsetY = Mathf.Sin(tiempo * 2f) * intensity;
            float offsetX = Mathf.Cos(tiempo) * intensity;

            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                posicionInicial + new Vector3(offsetX, offsetY, 0f),
                Time.deltaTime * 10f
            );

            //  PASOS
            float fasePaso = Mathf.Sin(tiempo * 2f);

            float delay = isCrouching ? 0.5f : (isSprinting ? 0.25f : 0.35f);

            if (fasePaso > 0.95f && Time.time - ultimoPaso > delay)
            {
                ultimoPaso = Time.time;

                if (audioSource && paso)
                    audioSource.PlayOneShot(paso);
            }
        }
        else
        {
            tiempo = 0f;

            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                posicionInicial,
                Time.deltaTime * 5f
            );
        }

        // =========================
        // LEAN
        // =========================
        float inputX = Input.GetAxis("Horizontal");

        float targetLean = -inputX * leanAngle;

        Quaternion targetRot = Quaternion.Euler(
            transform.localEulerAngles.x,
            transform.localEulerAngles.y,
            targetLean
        );

        transform.localRotation = Quaternion.Lerp(
            transform.localRotation,
            targetRot,
            Time.deltaTime * leanSpeed
        );

        // =========================
        //  FOV DINÁMICO
        // =========================
        if (cam != null)
        {
            float targetFov = isSprinting ? runFov : normalFov;

            cam.fieldOfView = Mathf.Lerp(
                cam.fieldOfView,
                targetFov,
                Time.deltaTime * fovSpeed
            );
        }
    }
}