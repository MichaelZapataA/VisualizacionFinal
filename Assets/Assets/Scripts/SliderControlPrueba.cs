using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderControlPrueba : MonoBehaviour
{

    public Renderer targetRenderer;

    public string materialPropertyName = "_DissolveAmount";

    public TextMeshProUGUI sliderValueText;

    private Material materialInstance;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (targetRenderer != null)
        {
            materialInstance = targetRenderer.material;
        }
        else
        {
            Debug.LogError("No se asignó un Renderer objetivo.");
        }

        
        if (sliderValueText != null && materialInstance != null)
        {
            sliderValueText.text = (materialInstance.GetFloat(materialPropertyName) * 100).ToString("F0") + "%";
        }
    }

    public void SetDissolveAmount(float value)
    {
        if (materialInstance != null)
        {
            materialInstance.SetFloat(materialPropertyName, value);
        }

        if (sliderValueText != null)
        {
            sliderValueText.text = (value*100).ToString("F0") + "%";
        }
    }
}
