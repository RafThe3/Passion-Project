using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.UI;
using TMPro;

public class CameraSettings : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI settingText;
    [SerializeField] private int placeValueToRound = 10;
    [SerializeField] private string settingType = string.Empty;

    private void Start()
    {
        slider.value = PlayerPrefs.GetFloat(settingType);
    }

    private void Update()
    {
        if (placeValueToRound % 10 != 0)
        {
            throw new ArgumentException("Place value must be a value divisible by 10.");
        }

        if (settingType == string.Empty)
        {
            throw new ArgumentException("Setting type needs a name.");
        }

        if (!slider.wholeNumbers)
        {
            slider.value = Mathf.Round(slider.value * placeValueToRound) / placeValueToRound;
        }

        PlayerPrefs.SetFloat(settingType, slider.value);
        settingText.text = $"{PlayerPrefs.GetFloat(settingType) * placeValueToRound}";
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
