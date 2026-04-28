using UnityEngine;

public class BombFeedback : MonoBehaviour
{
    public Light luzRoja;
    public AudioSource audioSource;
    public AudioClip pitido;

    public Timer timer;

    public float intensidadMax = 10f;
    public float rangoMin = 3f;
    public float rangoMax = 5f;

    private float tiempoPulso = 0f;
    private bool haPitido = false;

    void Update()
    {
        // SI HA EXPLOTADO → APAGAR TODO
        if (timer.haExplotado)
        {
            luzRoja.intensity = 0f;
            audioSource.Stop();
            enabled = false;
            return;
        }

        float tiempoRestante = timer.tiempo;
        float tiempoMax = 30f;

        // Duración del pulso (más rápido al final)
        float duracionPulso = Mathf.Lerp(1f, 0.2f, 1f - (tiempoRestante / tiempoMax));

        tiempoPulso += Time.deltaTime / duracionPulso;

        if (tiempoPulso >= 1f)
        {
            tiempoPulso = 0f;
            haPitido = false;
        }

        //  CURVA SUAVE
        float pulso = Mathf.Sin(tiempoPulso * Mathf.PI);

        // LUZ
        luzRoja.intensity = pulso * intensidadMax;
        luzRoja.range = Mathf.Lerp(rangoMin, rangoMax, pulso);

        //  PITIDO en el pico
        if (pulso > 0.9f && !haPitido)
        {
            audioSource.PlayOneShot(pitido);
            haPitido = true;
        }
    }
}