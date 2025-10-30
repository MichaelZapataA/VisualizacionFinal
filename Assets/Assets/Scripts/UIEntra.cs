using UnityEngine;
using DentedPixel; // Asegúrate de tener LeanTween

/// <summary>
/// Anima un elemento de la UI para que entre a la pantalla desde una dirección
/// seleccionada en el inspector (derecha, izquierda, arriba o abajo).
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class AnimateFromSide : MonoBehaviour
{
    // 1. Creamos un 'enum' para las opciones del menú desplegable
    public enum AnimationDirection
    {
        FromRight,
        FromLeft,
        FromTop,
        FromBottom
    }

    [Header("Configuración de Animación")]
    public AnimationDirection direction = AnimationDirection.FromRight; // ¡Aquí está el menú desplegable!
    public float animationTime = 0.8f;
    public float delay = 0f;
    public float offsetAmount = 1200f; // Píxeles de distancia (se aplicará a X o Y)
    public LeanTweenType easeType = LeanTweenType.easeOutExpo;

    private RectTransform rectTransform;
    private Vector2 finalPosition; // La posición donde lo dejas en el editor
    private Vector2 startPosition; // La posición inicial (fuera de pantalla)

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        // 2. Guardar la posición final
        finalPosition = rectTransform.position;

        // 3. Calcular la posición inicial basado en la dirección elegida
        startPosition = finalPosition; // Empezamos copiando la posición final

        switch (direction)
        {
            case AnimationDirection.FromRight:
                startPosition.x = finalPosition.x + offsetAmount;
                break;
            case AnimationDirection.FromLeft:
                startPosition.x = finalPosition.x - offsetAmount;
                break;
            case AnimationDirection.FromTop:
                startPosition.y = finalPosition.y + offsetAmount;
                break;
            case AnimationDirection.FromBottom:
                startPosition.y = finalPosition.y - offsetAmount;
                break;
        }

        // 4. Mover el objeto a su posición inicial (fuera de pantalla)
        rectTransform.anchoredPosition = startPosition;
    }

    void Start()
    {
        // 5. Iniciar la animación de entrada
        AnimateIn();
    }

    /// <summary>
    /// Inicia la animación de entrada.
    /// </summary>
    public void AnimateIn()
    {
        // Mueve el objeto desde 'startPosition' hasta 'finalPosition'
        LeanTween.move(rectTransform, finalPosition, animationTime)
            .setDelay(delay)
            .setEase(easeType);
    }

    /// <summary>
    /// (Opcional) Anima el objeto para que salga de la pantalla.
    /// </summary>
    public void AnimateOut()
    {
        // Mueve el objeto de vuelta a su 'startPosition'
        LeanTween.move(rectTransform, startPosition, animationTime)
            .setDelay(delay)
            .setEase(LeanTweenType.easeInExpo); // Un 'ease' de salida
    }
}
