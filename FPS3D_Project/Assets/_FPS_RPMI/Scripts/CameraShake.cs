using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float duracion = 0.5f;
    public float intensidad = 0.3f;

    private Vector3 posicionOriginal;
    private float tiempoRestante = 0f;

    void Start()
    {
        posicionOriginal = transform.localPosition;
    }

    void Update()
    {
        if (tiempoRestante > 0)
        {
            transform.localPosition = posicionOriginal + Random.insideUnitSphere * intensidad;
            tiempoRestante -= Time.deltaTime;
        }
        else
        {
            tiempoRestante = 0f;
            transform.localPosition = posicionOriginal;
        }
    }

    public void Shake()
    {
        tiempoRestante = duracion;
    }
}