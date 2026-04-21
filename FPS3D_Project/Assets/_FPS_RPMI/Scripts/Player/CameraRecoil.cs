using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    public static CameraRecoil Instance;

    [Header("Recoil Settings")]
    public float recoilReturnSpeed = 6f;
    public float recoilSnappiness = 12f;

    private Vector2 currentRecoil;
    private Vector2 targetRecoil;
    private Vector2 recoilVelocity;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        targetRecoil = Vector2.Lerp(targetRecoil, Vector2.zero, Time.deltaTime * recoilReturnSpeed);

        currentRecoil = Vector2.SmoothDamp(
            currentRecoil,
            targetRecoil,
            ref recoilVelocity,
            0.05f
        );

        transform.localRotation = Quaternion.Euler(
            -currentRecoil.y,
            currentRecoil.x,
            0f
        );
    }

    public void AddRecoil(Vector2 recoil)
    {
        targetRecoil += recoil;
    }
}