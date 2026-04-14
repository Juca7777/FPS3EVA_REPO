using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    public static CameraRecoil Instance;

    [Header("Recoil Settings")]
    public float recoilReturnSpeed = 8f;
    public float recoilSnappiness = 10f;

    private Vector2 currentRecoil;
    private Vector2 targetRecoil;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        //  volver suavemente a 0
        targetRecoil = Vector2.Lerp(targetRecoil, Vector2.zero, Time.deltaTime * recoilReturnSpeed);
        currentRecoil = Vector2.Lerp(currentRecoil, targetRecoil, Time.deltaTime * recoilSnappiness);

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