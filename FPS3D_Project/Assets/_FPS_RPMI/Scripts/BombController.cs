using UnityEngine;
using UnityEngine.SceneManagement;

public class BombController : MonoBehaviour
{
    [Header("Defuse Settings")]
    public float defuseTime = 5f;

    [Header("External Timer")]
    public Timer timerScript;

    [Header("Light")]
    public Light bombLight;
    public BombFeedback flickerScript;

    [Header("Audio")]
    public AudioSource audioSource;

    [Header("Scene")]
    public string nextSceneName;
    public float delayBeforeLoad = 3f;

    private float timer = 0f;
    private bool isDefusing = false;
    private bool isDefused = false;

    void Update()
    {
        if (isDefused) return;

        if (isDefusing)
        {
            timer += Time.deltaTime;

            if (timer >= defuseTime)
            {
                Defuse();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isDefused) return;

        if (other.CompareTag("Interactor"))
        {
            isDefusing = true;
            timer = 0f;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactor"))
        {
            CancelDefuse();
        }
    }

    void CancelDefuse()
    {
        isDefusing = false;
        timer = 0f;
    }

    void Defuse()
    {
        isDefusing = false;
        isDefused = true;

        //  detener timer
        if (timerScript != null)
            timerScript.enabled = false;

        //  parar parpadeo
        if (flickerScript != null)
            flickerScript.enabled = false;

        //  luz verde
        if (bombLight != null)
        {
            bombLight.color = Color.green;
            bombLight.intensity = 2f;
        }

        //  parar sonido
        if (audioSource != null)
            audioSource.Stop();

        Debug.Log(" Bomba desactivada");

        //  cargar escena con delay
        Invoke(nameof(LoadNextScene), delayBeforeLoad);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}