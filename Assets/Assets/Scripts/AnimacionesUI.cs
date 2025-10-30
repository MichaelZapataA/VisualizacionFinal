using UnityEngine;

public class AnimacionesUI : MonoBehaviour
{
    [SerializeField] private GameObject Titulo;

    [SerializeField] private GameObject Botones;

    private void Start()
    {
        //Animaciones Botones
        Botones.GetComponent<RectTransform>().GetComponent<CanvasGroup>().alpha = 0f;
        LeanTween.alphaCanvas(Botones.GetComponent<RectTransform>().GetComponent<CanvasGroup>(), 1f, 1f).setDelay(2f).setEaseInOutSine();

        //Animaciones Titulo
        LeanTween.scale(Titulo, new Vector3(1f, 1f, 1f), 1f).setEaseOutElastic();
        LeanTween.scale(Titulo, new Vector3(0.7f, 0.7f, 0.7f), 1f).setDelay(1.5f).setEaseInOutSine();
        LeanTween.moveY(Titulo, 22f, 1f).setDelay(1.3f).setEaseInOutSine();
        LeanTween.moveX(Titulo, 40f, 1f).setDelay(1.3f).setEaseInBack();

    }
}

