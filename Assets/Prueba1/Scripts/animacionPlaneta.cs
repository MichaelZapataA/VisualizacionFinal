using UnityEngine;

public class PlanetaFlotante : MonoBehaviour
{
    [Header("Configuración de Rotación")]
    [Tooltip("Velocidad a la que el planeta gira sobre sí mismo.")]
    public float velocidadRotacion = 20.0f;
    
    [Tooltip("Eje de rotación (Vector3.up es el eje Y).")]
    public Vector3 ejeRotacion = Vector3.up;

    [Header("Configuración de Flotación")]
    [Tooltip("Velocidad del movimiento vertical (qué tan rápido sube y baja).")]
    public float velocidadFlotacion = 1.0f;
    
    [Tooltip("Qué tan alto y bajo llega el planeta desde su punto inicial.")]
    public float alturaFlotacion = 0.5f;

    // Guardamos la posición inicial para calcular la flotación
    private Vector3 posicionInicial;

    void Start()
    {
        // Guardamos la posición inicial local del planeta
        // Usamos localPosition por si el planeta es hijo de otro objeto (como un "SistemaSolar")
        posicionInicial = transform.localPosition;
    }

    void Update()
    {
        // --- 1. ROTACIÓN ---
        // Gira el objeto alrededor de su 'ejeRotacion'
        // Multiplicamos por Time.deltaTime para que la velocidad sea constante
        // sin importar los fotogramas por segundo (FPS).
        transform.Rotate(ejeRotacion, velocidadRotacion * Time.deltaTime);


        // --- 2. FLOTACIÓN ---
        // Usamos una función seno (Mathf.Sin) para crear un movimiento suave de ola.
        // Time.time es el tiempo en segundos desde que inició el juego.
        // Multiplicarlo por 'velocidadFlotacion' hace que la ola vaya más rápido o lento.
        float offsetVertical = Mathf.Sin(Time.time * velocidadFlotacion) * alturaFlotacion;

        // Creamos la nueva posición
        // Empezamos desde la 'posicionInicial' y solo modificamos el eje Y.
        Vector3 nuevaPosicion = new Vector3(
            posicionInicial.x,
            posicionInicial.y + offsetVertical, // Aquí aplicamos la flotación
            posicionInicial.z
        );

        // Aplicamos la nueva posición al transform local.
        transform.localPosition = nuevaPosicion;
    }
}