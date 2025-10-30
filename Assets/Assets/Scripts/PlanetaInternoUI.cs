using UnityEngine;
using DentedPixel; // Asegúrate de tener LeanTween en tu proyecto

/// <summary>
/// Este script gestiona la transición animada entre dos paneles.
/// Oculta uno, lo desactiva, y luego activa y muestra el otro.
/// </summary>
public class PanelSwitcher : MonoBehaviour
{
    [Header("Paneles a Intercambiar")]
    public GameObject panelToHide; // El panel que está visible y quieres ocultar
    public GameObject panelToShow; // El panel que está inactivo y quieres mostrar

    [Header("Configuración de Animación")]
    public float fadeTime = 0.4f; // Duración del fade (rápido y suave)
    public LeanTweenType easeType = LeanTweenType.easeOutQuad;

    /// <summary>
    /// Esta es la función pública que conectarás a tu botón.
    /// </summary>
    public void PerformSwitch()
    {
        if (panelToHide == null || panelToShow == null)
        {
            Debug.LogError("PanelSwitcher: Faltan referencias a los paneles en el Inspector.");
            return;
        }

        // --- 1. Esconder el panel actual ---

        // Buscamos su CanvasGroup. Si no tiene, se lo añadimos.
        CanvasGroup hideCG = panelToHide.GetComponent<CanvasGroup>();
        if (hideCG == null)
        {
            hideCG = panelToHide.AddComponent<CanvasGroup>();
        }

        // Desactivar interacción para que no se pueda clickear mientras desaparece
        hideCG.interactable = false;
        hideCG.blocksRaycasts = false;

        // Animar el fade-out
        LeanTween.alphaCanvas(hideCG, 0f, fadeTime)
            .setEase(easeType)
            .setOnComplete(OnHideAnimationComplete); // Llama a la siguiente función al terminar
    }

    /// <summary>
    /// Esta función se llama automáticamente cuando el panel "hide" ha terminado de desaparecer.
    /// </summary>
    private void OnHideAnimationComplete()
    {
        // --- 2. Desactivar el panel viejo ---
        // (Como lo pediste: "luego lo desactive")
        panelToHide.SetActive(false);

        // --- 3. Activar el panel nuevo ---
        // (Como lo pediste: "y active otro panel")
        panelToShow.SetActive(true);

        // --- 4. Mostrar el panel nuevo ---

        // Buscamos o añadimos su CanvasGroup
        CanvasGroup showCG = panelToShow.GetComponent<CanvasGroup>();
        if (showCG == null)
        {
            showCG = panelToShow.AddComponent<CanvasGroup>();
        }

        // Lo configuramos para que empiece invisible (porque acaba de activarse)
        showCG.alpha = 0f;
        // Y nos aseguramos de que sea interactivo
        showCG.interactable = true;
        showCG.blocksRaycasts = true;

        // Animar el fade-in
        LeanTween.alphaCanvas(showCG, 1f, fadeTime).setEase(easeType);
    }
}
