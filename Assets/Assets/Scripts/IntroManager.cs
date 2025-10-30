using UnityEngine;
using UnityEngine.SceneManagement; // ¡Muy importante para cambiar de escena!
using DentedPixel; // Para usar LeanTween

public class SceneLoader : MonoBehaviour
{
    [Header("Grupos de Canvas")]
    public CanvasGroup menuCanvasGroup;     // El CanvasGroup del menú principal
    public CanvasGroup introCanvasGroup;    // El CanvasGroup de tu pantalla de introducción

    [Header("Configuración de Tiempos")]
    public float fadeTime = 0.7f;     // Tiempo para el fade-out del menú y fade-in del intro
    public float introDuration = 3.0f;  // Cuánto tiempo se mostrará la intro ANTES de cargar

    [Header("Escena a Cargar")]
    public string sceneToLoadName; // El nombre EXACTO de tu escena de juego (ej. "Nivel1")

    /// <summary>
    /// Esta función pública es la que conectarás al botón "Iniciar".
    /// </summary>
    public void StartGameSequence()
    {
        // 0. Revisar si el nombre de la escena está vacío
        if (string.IsNullOrEmpty(sceneToLoadName))
        {
            Debug.LogError("¡No has especificado un 'Scene To Load Name' en el Inspector!");
            return;
        }

        // 1. Desactivar la interacción del menú inmediatamente para evitar doble clic
        if (menuCanvasGroup != null)
        {
            menuCanvasGroup.interactable = false;
        }

        // 2. Animar el fade-out del menú principal
        LeanTween.alphaCanvas(menuCanvasGroup, 0f, fadeTime)
            .setEase(LeanTweenType.easeInQuad);

        // 3. Animar el fade-in de la pantalla de introducción
        // La hacemos visible y bloqueamos clicks (si es necesario)
        if (introCanvasGroup != null)
        {
            introCanvasGroup.alpha = 0f; // Asegurarse de que empieza en 0
            introCanvasGroup.blocksRaycasts = true; // Para que "atrape" los clics

            LeanTween.alphaCanvas(introCanvasGroup, 1f, fadeTime)
                .setDelay(0.1f) // Un pequeño retraso para que se sienta mejor
                .setEase(LeanTweenType.easeOutQuad);
        }

        // 4. Preparar la carga de la siguiente escena
        // Calculamos el tiempo total de espera:
        // El tiempo que tarda en aparecer la intro + el tiempo que debe durar
        float totalWaitTime = fadeTime + introDuration;

        // Usamos delayedCall para ejecutar una función después de ese tiempo
        LeanTween.delayedCall(totalWaitTime, () =>
        {
            // Esta es la función que se ejecutará después del tiempo de espera
            LoadNextScene();
        });
    }

    /// <summary>
    /// Carga la escena especificada en 'sceneToLoadName'.
    /// </summary>
    private void LoadNextScene()
    {
        Debug.Log($"Cargando escena: {sceneToLoadName}...");
        SceneManager.LoadScene(sceneToLoadName);
    }

    // --- Asegurarse de que el intro esté oculto al empezar ---
    void Start()
    {
        if (introCanvasGroup != null)
        {
            introCanvasGroup.alpha = 0f;
            introCanvasGroup.interactable = false;
            introCanvasGroup.blocksRaycasts = false;
        }
    }
}
