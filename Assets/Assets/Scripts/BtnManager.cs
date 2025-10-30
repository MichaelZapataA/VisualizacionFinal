using UnityEngine;
using UnityEngine.SceneManagement; // ¡Necesario para cambiar de escena!

public class ReturnToMenu : MonoBehaviour
{
    [Header("Configuración")]
    public string menuSceneName = "MainMenu"; // Escribe el nombre exacto de tu escena de menú

    /// <summary>
    /// Esta función pública la conectarás a tu botón.
    /// </summary>
    public void LoadMainMenuScene()
    {
        if (string.IsNullOrEmpty(menuSceneName))
        {
            Debug.LogError("¡No has puesto un nombre de escena en el Inspector!");
            return;
        }

        Debug.Log($"Volviendo a la escena: {menuSceneName}");
        
        // Carga la escena del menú principal
        SceneManager.LoadScene(menuSceneName);
    }
}