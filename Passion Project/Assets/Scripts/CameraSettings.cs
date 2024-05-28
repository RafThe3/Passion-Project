using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class CameraSettings : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Slider slider;
    [SerializeField] private TMPro.TextMeshProUGUI sensText;

    private FirstPersonController controller;

    private void Awake()
    {
        controller = FindObjectOfType<FirstPersonController>();
    }

    private void Start()
    {
        slider.value = PlayerPrefs.GetFloat("Sens");
    }

    private void Update()
    {
        ChangeFOV();
    }

    private void ChangeFOV()
    {
        sensText.text = $"{controller.GetRotationSpeed}";
        PlayerPrefs.SetFloat("Sens", controller.GetRotationSpeed);
    }

    public void AddSliderValue()
    {
        slider.value++;
    }

    public void DecreaseSliderValue()
    {
        slider.value--;
    }
}
