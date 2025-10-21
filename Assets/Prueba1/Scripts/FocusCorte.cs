using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Para el Slider
using TMPro; // Si usas TextMeshPro

public class FocusControl : MonoBehaviour
{
    [Header("Objetos a Controlar")]
    [Tooltip("La cámara principal que se va a mover.")]
    public Transform camaraTransform;
    [Tooltip("Arrastra aquí el Panel del Slider (que debe tener un componente CanvasGroup).")]
    public CanvasGroup panelDelSliderCanvasGroup;
    [Tooltip("El slider que quieres animar.")]
    public Slider sliderDelPlaneta; 
    
    [Header("Puntos de Foco")]
    [Tooltip("Arrastra aquí el 'GameObject' vacío que marca el destino del primer plano.")]
    public Transform posicionPrimerPlano;
    
    [Header("Control del Planeta")]
    [Tooltip("Arrastra aquí el Planeta que tiene el script 'PlanetaFlotante'.")]
    public PlanetaFlotante scriptDelPlaneta; 

    [Header("Configuración de Animación")]
    [Tooltip("Velocidad del movimiento de la cámara (más alto = más rápido).")]
    public float velocidadMovimientoCamara = 2.0f;
    [Tooltip("Velocidad del desvanecido (más alto = más rápido).")]
    public float velocidadFade = 3.0f;
    [Tooltip("Velocidad con la que el slider se anima a 0 o 1.")]
    public float velocidadReseteoSlider = 3.0f;

    // Variables privadas para guardar el estado original
    private Vector3 posOriginalCamara;
    private Quaternion rotOriginalCamara;
    private bool estaEnPrimerPlano = false;

    void Start()
    {
        // Guardamos la posición inicial de la cámara
        if (camaraTransform == null)
        {
            camaraTransform = Camera.main.transform;
        }
        posOriginalCamara = camaraTransform.position;
        rotOriginalCamara = camaraTransform.rotation;
        
        // Nos aseguramos que el panel esté oculto al iniciar
        if(panelDelSliderCanvasGroup != null)
        {
            panelDelSliderCanvasGroup.alpha = 0;
            panelDelSliderCanvasGroup.interactable = false;
            panelDelSliderCanvasGroup.blocksRaycasts = false;
        }
    }

    /// <summary>
    /// Esta es la función pública que debes llamar desde tu Botón (en OnClick).
    /// </summary>
    public void ToggleFocoPlaneta()
    {
        // Detenemos cualquier animación anterior para evitar conflictos
        StopAllCoroutines(); 

        if (!estaEnPrimerPlano)
        {
            // --- 1. Mover al Planeta ---
            
            // Animamos el slider a 0
            if (sliderDelPlaneta != null)
            {
                StartCoroutine(AnimateSlider(1.0f));
            }

            // Movemos la cámara
            StartCoroutine(MoverCamara(posicionPrimerPlano.position, posicionPrimerPlano.rotation));
            
            // Mostramos el panel
            StartCoroutine(FadeCanvas(panelDelSliderCanvasGroup, 1.0f, true));
            
            // Le decimos al planeta que deje de rotar
            if (scriptDelPlaneta != null)
            {
                scriptDelPlaneta.rotacionActiva = false;
            }

            estaEnPrimerPlano = true;
        }
        else
        {
            // --- 2. Volver al Original ---
            
            // Animamos el slider a 1
            if (sliderDelPlaneta != null)
            {
                StartCoroutine(AnimateSlider(0.0f));
            }

            // Movemos la cámara
            StartCoroutine(MoverCamara(posOriginalCamara, rotOriginalCamara));
            
            // Ocultamos el panel
            StartCoroutine(FadeCanvas(panelDelSliderCanvasGroup, 0.0f, false));

            // Le decimos al planeta que vuelva a rotar
            if (scriptDelPlaneta != null)
            {
                scriptDelPlaneta.rotacionActiva = true; 
            }

            estaEnPrimerPlano = false;
        }
    }

    // --- COROUTINES (Las funciones que hacen la animación) ---

    IEnumerator MoverCamara(Vector3 posDestino, Quaternion rotDestino)
    {
        float tiempo = 0;
        Vector3 posInicio = camaraTransform.position;
        Quaternion rotInicio = camaraTransform.rotation;

        while (tiempo < 1.0f)
        {
            tiempo += Time.deltaTime * velocidadMovimientoCamara;
            camaraTransform.position = Vector3.Lerp(posInicio, posDestino, tiempo);
            camaraTransform.rotation = Quaternion.Slerp(rotInicio, rotDestino, tiempo);
            yield return null; // Espera al siguiente frame
        }

        camaraTransform.position = posDestino;
        camaraTransform.rotation = rotDestino;
    }

    IEnumerator FadeCanvas(CanvasGroup cg, float alphaDestino, bool hacerInteractuable)
    {
        float tiempo = 0;
        float alphaInicio = cg.alpha;

        if (hacerInteractuable)
        {
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }

        while (tiempo < 1.0f)
        {
            tiempo += Time.deltaTime * velocidadFade;
            cg.alpha = Mathf.Lerp(alphaInicio, alphaDestino, tiempo);
            yield return null;
        }

        cg.alpha = alphaDestino;

        if (!hacerInteractuable)
        {
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
    }

    IEnumerator AnimateSlider(float targetValue)
    {
        float valorInicio = sliderDelPlaneta.value;
        float rango = sliderDelPlaneta.maxValue - sliderDelPlaneta.minValue;
        
        // Ajustamos la velocidad para que sea más consistente sin importar el rango
        float velocidad = velocidadReseteoSlider * (rango > 0 ? rango : 1);

        while (Mathf.Abs(sliderDelPlaneta.value - targetValue) > 0.01f)
        {
            sliderDelPlaneta.value = Mathf.MoveTowards(
                sliderDelPlaneta.value, 
                targetValue, 
                velocidad * Time.deltaTime
            );
            yield return null; // Espera al siguiente frame
        }
        
        sliderDelPlaneta.value = targetValue;
    }
}