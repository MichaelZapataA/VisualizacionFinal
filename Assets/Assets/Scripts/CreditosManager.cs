using UnityEngine;
using DentedPixel; // ¡Recuerda tener LeanTween en tu proyecto!
using UnityEngine.SceneManagement; // Para el Application.Quit

public class MainMenuManager : MonoBehaviour
{
    [Header("MANEJO DE ESTADO")]
    public CanvasGroup menuCanvasGroup;     // Grupo con logo y botones "Iniciar", "Créditos", "Salir"
    public CanvasGroup creditsCanvasGroup;  // Grupo con el texto de créditos y botón "Atrás"

    [Header("PLANETA Y COMPONENTES")]
    public GameObject planetObject;           // El GameObject padre del planeta
    public GameObject innerComponentObject;   // (Opcional) El componente interno

    [Header("ANIMACIÓN: Entrada")]
    public float entranceTime = 1.5f;
    public LeanTweenType entranceEase = LeanTweenType.easeOutQuad;
    public Vector3 planetStartOffset = new Vector3(-20f, 0f, 0f); // Dónde empieza el planeta

    [Header("ANIMACIÓN: Transición")]
    public float transitionTime = 0.7f;
    public Vector3 planetExitPosition;      // Dónde va el planeta al ver créditos (ej. X=20)
    public LeanTweenType transitionEase = LeanTweenType.easeInBack;

    [Header("ANIMACIÓN: Rotación Continua")]
    public float degreesPerSecond = 15f;
    public Vector3 rotationAxis = Vector3.up;
    public float innerDegreesPerSecond = 30f;
    public Vector3 innerRotationAxis = Vector3.forward;

    [Header("ANIMACIÓN: Levitación")]
    public float levitationHeight = 0.1f;
    public float levitationTime = 1.5f;

    // --- Variables Internas ---
    private Vector3 planetStartPosition; // Dónde "vive" el planeta en el menú
    private LTDescr levitationTween;     // Para poder pausar la levitación

    void Awake()
    {
        // --- 1. Guardar posiciones y establecer estado inicial ---
        
        // Guardamos la posición final del planeta (donde está en el editor)
        planetStartPosition = planetObject.transform.position;

        // Movemos el planeta a su posición inicial (fuera de pantalla)
        planetObject.transform.position = planetStartPosition + planetStartOffset;

        // --- 2. Configurar Canvas Groups ---
        // Menú: Empieza invisible pero se animará
        menuCanvasGroup.alpha = 0f;
        menuCanvasGroup.interactable = false;

        // Créditos: Empieza invisible y se queda así por ahora
        creditsCanvasGroup.alpha = 0f;
        creditsCanvasGroup.interactable = false;
        creditsCanvasGroup.blocksRaycasts = false;
    }

    void Start()
    {
        // --- 3. Iniciar la animación de entrada ---
        // (La rotación en Update() empezará automáticamente)

        // Animar el planeta a su sitio
        LeanTween.move(planetObject, planetStartPosition, entranceTime)
            .setEase(entranceEase)
            .setOnComplete(StartLevitation); // Empezar a levitar cuando llegue

        // Animar el menú para que aparezca
        LeanTween.alphaCanvas(menuCanvasGroup, 1f, entranceTime * 0.8f) // Un poco más rápido
            .setDelay(0.3f) // Espera un poco
            .setOnComplete(() => {
                menuCanvasGroup.interactable = true;
            });
    }

    void Update()
    {
        // --- 4. Rotación continua (siempre activa) ---
        if (planetObject != null)
        {
            planetObject.transform.Rotate(rotationAxis, degreesPerSecond * Time.deltaTime);
        }

        if (innerComponentObject != null)
        {
            innerComponentObject.transform.Rotate(innerRotationAxis, -innerDegreesPerSecond * Time.deltaTime);
        }
    }

    // --- 5. Funciones de Levitación ---
    void StartLevitation()
    {
        // Inicia y guarda la animación de levitación
        levitationTween = LeanTween.moveY(planetObject, planetStartPosition.y + levitationHeight, levitationTime)
            .setEase(LeanTweenType.easeInOutSine)
            .setLoopPingPong();
    }

    void StopLevitation()
    {
        // Detiene la animación de levitación
        if (levitationTween != null)
        {
            LeanTween.cancel(levitationTween.id);
        }
    }

    // --- 6. FUNCIONES PARA BOTONES (Públicas) ---

    public void OnCreditsButtonPressed()
    {
        // Detener levitación
        StopLevitation();

        // Desactivar menú
        menuCanvasGroup.interactable = false;

        // Mover planeta y ocultar menú
        LeanTween.move(planetObject, planetExitPosition, transitionTime).setEase(transitionEase).setOnComplete(StartLevitation);
        LeanTween.alphaCanvas(menuCanvasGroup, 0f, transitionTime/2f);

        // Mostrar créditos
        LeanTween.alphaCanvas(creditsCanvasGroup, 1f, transitionTime / 2f)
            .setDelay(0.1f) // Pequeño retraso
            .setOnComplete(() =>
            {
                creditsCanvasGroup.interactable = true;
                creditsCanvasGroup.blocksRaycasts = true;
            });
        
    }

    public void OnBackButtonPressed() // Botón "Atrás" desde los créditos
    {
        // Detener levitación
        StopLevitation();
        
        // Desactivar créditos
        creditsCanvasGroup.interactable = false;
        creditsCanvasGroup.blocksRaycasts = false;

        // Mover planeta de vuelta y ocultar créditos
        LeanTween.move(planetObject, planetStartPosition, transitionTime)
            .setEase(transitionEase)
            .setOnComplete(StartLevitation); // Reiniciar levitación al llegar

        LeanTween.alphaCanvas(creditsCanvasGroup, 0f, transitionTime/2f);

        // Mostrar menú
        LeanTween.alphaCanvas(menuCanvasGroup, 1f, transitionTime/2f)
            .setDelay(0.1f)
            .setOnComplete(() => {
                menuCanvasGroup.interactable = true;
            });
    }

    public void OnQuitButtonPressed()
    {
        Debug.Log("Saliendo de la aplicación...");

#if UNITY_EDITOR
        // Detiene el modo Play en el editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Cierra la aplicación (en un juego compilado)
        Application.Quit();
#endif
    }
}