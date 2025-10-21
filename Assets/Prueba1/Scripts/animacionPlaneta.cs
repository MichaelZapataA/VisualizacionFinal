using UnityEngine;

public class PlanetaFlotante : MonoBehaviour
{
    [Header("Configuración de Rotación")]
    [Tooltip("Velocidad a la que el planeta gira sobre sí mismo.")]
    public float velocidadRotacion = 20.0f;
    [Tooltip("Eje de rotación (Vector3.up es el eje Y).")]
    public Vector3 ejeRotacion = Vector3.up;
    [Tooltip("Velocidad para volver a la rotación original cuando se detiene.")]
    public float velocidadVuelta = 2.0f;

    [Header("Configuración de Flotación")]
    [Tooltip("Velocidad del movimiento vertical (qué tan rápido sube y baja).")]
    public float velocidadFlotacion = 1.0f;
    [Tooltip("Qué tan alto y bajo llega el planeta desde su punto inicial.")]
    public float alturaFlotacion = 0.5f;

    // Esta variable es pública para que 'FocusControl' pueda cambiarla
    [HideInInspector]
    public bool rotacionActiva = true; 

    private Vector3 posicionInicial;
    private Quaternion rotacionInicial; // Guardamos la rotación inicial

    void Start()
    {
        // Guardamos las posiciones iniciales
        posicionInicial = transform.localPosition;
        rotacionInicial = transform.localRotation;
    }

    void Update()
    {
        // --- 1. ROTACIÓN ---
        if (rotacionActiva)
        {
            // Gira normalmente
            transform.Rotate(ejeRotacion, velocidadRotacion * Time.deltaTime, Space.Self);
        }
        else
        {
            // Vuelve suavemente a su rotación inicial
            transform.localRotation = Quaternion.Slerp(
                transform.localRotation, 
                rotacionInicial, 
                velocidadVuelta * Time.deltaTime
            );
        }

        // --- 2. FLOTACIÓN ---
        // (La flotación siempre está activa)
        float offsetVertical = Mathf.Sin(Time.time * velocidadFlotacion) * alturaFlotacion;

        Vector3 nuevaPosicion = new Vector3(
            posicionInicial.x,
            posicionInicial.y + offsetVertical,
            posicionInicial.z
        );

        transform.localPosition = nuevaPosicion;
    }
}