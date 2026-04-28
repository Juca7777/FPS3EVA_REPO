using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float tiempo = 30f;
    public TextMeshProUGUI textoTiempo;

    public GameObject explosionVFX;
    public Transform puntoExplosion;

    public CameraShake cameraShake;
    public ScreenFlash screenFlash;

    public bool haExplotado = false;

    private float parpadeoTimer = 0f;

    void Update()
    {
        if (haExplotado) return;

        // Reducir tiempo
        if (tiempo > 0)
        {
            tiempo -= Time.deltaTime;
        }

        // FORMATO 00:00
        int minutos = Mathf.FloorToInt(tiempo / 60f);
        int segundos = Mathf.FloorToInt(tiempo % 60f);
        textoTiempo.text = string.Format("{0:00}:{1:00}", minutos, segundos);

        //  PARPADEO PROGRESIVO EN LOS ÚLTIMOS 10s
        if (tiempo <= 10f)
        {
            float velocidad = Mathf.Lerp(3f, 10f, 1f - (tiempo / 10f));
            parpadeoTimer += Time.deltaTime * velocidad;

            if (Mathf.FloorToInt(parpadeoTimer) % 2 == 0)
                textoTiempo.color = Color.red;
            else
                textoTiempo.color = Color.white;
        }
        else
        {
            textoTiempo.color = Color.white;
        }

        //  EXPLOSIÓN
        if (tiempo <= 0 && !haExplotado)
        {
            Explode();
        }
    }

    void Explode()
    {
        haExplotado = true;

        Instantiate(explosionVFX, puntoExplosion.position, Quaternion.identity);

        //  EFECTOS PRO
        if (cameraShake != null)
            cameraShake.Shake();

        if (screenFlash != null)
            screenFlash.Flash();

        textoTiempo.enabled = false;

        Invoke(nameof(PerderNivel), 2f);
    }

    void PerderNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}