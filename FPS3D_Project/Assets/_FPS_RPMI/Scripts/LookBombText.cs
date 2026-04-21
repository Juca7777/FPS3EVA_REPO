using UnityEngine;

public class BombLookPrompt : MonoBehaviour
{
    [Header("References")]
    public Camera cam;
    public GameObject promptUI;
    public BombController bomb;

    [Header("Settings")]
    public float range = 3f;
    public LayerMask bombLayer;

    void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, range, bombLayer))
        {
            if (hit.collider.GetComponent<BombController>() != null)
            {
                promptUI.SetActive(true);
                return;
            }
        }

        promptUI.SetActive(false);
    }
}