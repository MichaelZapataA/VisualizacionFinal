using UnityEngine;

public class PlanetRotator : MonoBehaviour
{
    [Header("Rotación del Planeta (Este Objeto)")]
    public float mainDegreesPerSecond = 15f;
    public Vector3 mainRotationAxis = Vector3.up;

    [Header("Rotación del Componente Interno")]
    public Transform innerComponent; // Arrastra aquí el objeto hijo que girará al revés
    public float innerDegreesPerSecond = 30f;
    public Vector3 innerRotationAxis = Vector3.forward;

    // Update se llama en cada frame
    void Update()
    {
        // 1. Rota el objeto padre (el planeta)
        // Usa 'transform' (con 't' minúscula) para referirse al objeto al que está
        // adjunto este script.
        transform.Rotate(mainRotationAxis, mainDegreesPerSecond * Time.deltaTime);

        // 2. Rota el componente interno en dirección contraria
        if (innerComponent != null)
        {
            // Usamos un valor negativo para la velocidad y 'Rotate' en el 'Transform' hijo
            innerComponent.Rotate(innerRotationAxis, -innerDegreesPerSecond * Time.deltaTime);
        }
    }
}