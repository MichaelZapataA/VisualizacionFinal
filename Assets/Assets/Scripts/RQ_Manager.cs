using UnityEngine;
using UnityEngine.SceneManagement; // Necesario si quisieras cambiar de escena, aunque aquí no lo usamos

public class QuitGameManager : MonoBehaviour
{
    // Esta es la función pública que conectarás al botón
    public void QuitApplication()
    {
        // Si estás ejecutando el juego en el editor de Unity
#if UNITY_EDITOR
        // Detiene la simulación de "Play"
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Si estás en una aplicación compilada (PC, Mac, Linux, etc.)
        Application.Quit();
#endif

        // (Opcional) Agrega un log para confirmar que se hizo clic
        Debug.Log("El usuario ha intentado salir de la aplicación.");
    }
}