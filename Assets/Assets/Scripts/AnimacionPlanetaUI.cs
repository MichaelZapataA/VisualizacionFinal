using UnityEngine;
using DentedPixel; // Asegúrate de tener LeanTween

public class PlanetAnimationEnhanced : MonoBehaviour
{
    [Header("Configuración del Planeta")]
    public GameObject planetObject;           // El GameObject padre que levitará y rotará
    public GameObject innerComponentObject;   // El GameObject del componente interno que rotará al revés (opcional)

    [Header("Parámetros de Entrada")]
    public bool animateEntrance = true;
    public float entranceTime = 1.5f;
    public LeanTweenType entranceEaseType = LeanTweenType.easeOutQuad;
    public Vector3 startOffset = new Vector3(-20f, 0f, 0f); 

    [Header("Parámetros de Rotación Continua")]
    public float degreesPerSecond = 15f;        // Velocidad de rotación (grados por segundo)
    public Vector3 rotationAxis = Vector3.up; 

    [Header("Parámetros de Rotación Interna (Opcional)")]
    public float innerDegreesPerSecond = 30f;   // Velocidad de rotación interna
    public Vector3 innerRotationAxis = Vector3.forward;

    [Header("Parámetros de Levitación")]
    public float levitationHeight = 0.1f;     
    public float levitationTime = 1.5f;      // Tiempo que toma subir (o bajar)

    // Variables internas
    private Vector3 finalPosition;
    private Vector3 initialLevitationCenter;

    void Awake()
    {
        // Guardamos la posición actual del planeta en el editor como la final/centro
        finalPosition = planetObject.transform.position;
        initialLevitationCenter = planetObject.transform.position;

        if (animateEntrance)
        {
            // Colocamos el planeta en su posición inicial fuera de pantalla
            planetObject.transform.position = finalPosition + startOffset;
        }
    }

    void Start()
    {
        if (animateEntrance)
        {
            // Inicia la animación de entrada
            LeanTween.move(planetObject, finalPosition, entranceTime)
                .setEase(entranceEaseType)
                .setOnComplete(StartLevitation); // ¡Llama a levitar AL TERMINAR!
        }
        else
        {
            // Si no hay entrada, empieza a levitar de inmediato
            StartLevitation();
        }
        
        // La rotación (en Update) empezará a funcionar desde este mismo frame.
    }

    // Este método se llama en cada frame
    void Update()
    {
        // --- Rotación Continua ---
        // Esto se ejecutará desde el primer frame, haciendo que gire mientras entra.
        if (planetObject != null)
        {
            // Multiplicamos por Time.deltaTime para que la velocidad sea constante
            // independientemente de los frames por segundo
            planetObject.transform.Rotate(rotationAxis, degreesPerSecond * Time.deltaTime);
        }

        // --- Rotación Interna ---
        if (innerComponentObject != null)
        {
            // Usamos un valor negativo para la dirección contraria
            innerComponentObject.transform.Rotate(innerRotationAxis, -innerDegreesPerSecond * Time.deltaTime);
        }
    }

    // Esta función ahora solo se encarga de la levitación
    void StartLevitation()
    {
        // --- Levitación ---
        // Anima el planeta para que suba y baje continuamente
        LeanTween.moveY(planetObject, initialLevitationCenter.y + levitationHeight, levitationTime)
            .setEase(LeanTweenType.easeInOutSine) // Suavizado
            .setLoopPingPong(); // Ir arriba y abajo
    }
}