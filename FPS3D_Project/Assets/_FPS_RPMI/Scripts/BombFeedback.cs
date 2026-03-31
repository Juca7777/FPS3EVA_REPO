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
        float tiempoRestante = timer.tiempo;
        float tiempoMax = 30f;

        // Duración de cada pulso (más corto al final)
        float duracionPulso = Mathf.Lerp(1f, 0.2f, 1f - (tiempoRestante / tiempoMax));

        // Avanza el pulso normalizado (0 → 1)
        tiempoPulso += Time.deltaTime / duracionPulso;

        if (tiempoPulso >= 1f)
        {
            tiempoPulso = 0f;
            haPitido = false; // reset para siguiente pulso
        }

        //  CURVA SUAVE tipo latido
        float pulso = Mathf.Sin(tiempoPulso * Mathf.PI); // curva perfecta de latido

        //  LUZ SUAVE
        luzRoja.intensity = pulso * intensidadMax;
        luzRoja.range = Mathf.Lerp(rangoMin, rangoMax, pulso);

        //  PITIDO (solo una vez por pulso, en el pico)
        if (pulso > 0.9f && !haPitido)
        {
            audioSource.PlayOneShot(pitido);
            haPitido = true;
        }
    }
}