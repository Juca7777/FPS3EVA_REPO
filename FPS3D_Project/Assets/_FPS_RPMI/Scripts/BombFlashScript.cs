using UnityEngine;
using UnityEngine.UI;

public class ScreenFlash : MonoBehaviour
{
    public Image imagenFlash;
    public float duracion = 0.5f;

    private float tiempo;

    public void Flash()
    {
        tiempo = duracion;
    }

    void Update()
    {
        if (tiempo > 0)
        {
            tiempo -= Time.deltaTime;

            float alpha = tiempo / duracion;
            imagenFlash.color = new Color(1f, 1f, 1f, alpha);
        }
        else
        {
            imagenFlash.color = new Color(1f, 1f, 1f, 0f);
        }
    }
}