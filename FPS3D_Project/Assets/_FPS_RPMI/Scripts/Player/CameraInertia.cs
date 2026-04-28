using UnityEngine;

public class CameraInertia : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;

    [Header("Inertia Settings")]
    public float amount = 0.015f;
    public float smooth = 6f;

    private Vector3 initialLocalPos;

    void Start()
    {
        initialLocalPos = transform.localPosition;

        if (rb == null)
            rb = GetComponentInParent<Rigidbody>();
    }

    void Update()
    {
        //  velocidad horizontal del jugador
        Vector3 velocity = rb.linearVelocity;
        velocity.y = 0f;

        //  offset contrario al movimiento (efecto inercia)
        Vector3 targetOffset = -velocity * amount;

        Vector3 targetPos = initialLocalPos + targetOffset;

        //  suavizado
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            targetPos,
            Time.deltaTime * smooth
        );
    }
}