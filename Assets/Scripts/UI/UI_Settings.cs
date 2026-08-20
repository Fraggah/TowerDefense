using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Settings : MonoBehaviour
{
    private CameraController camController;

    [Header("Keyboard Sensetivity")]
    [SerializeField] private Slider keyboardSenseSlider;
    [SerializeField] private TextMeshProUGUI keyboardSensText;
    [SerializeField] private string keyboardSenseParametr = "keyboardSens";

    [SerializeField] private float minKeyboardSens = 60;
    [SerializeField] private float maxKeyboardSens = 240;

    [Header("Mouse Sensetivity")]
    [SerializeField] private Slider mouseSenseSlider;
    [SerializeField] private TextMeshProUGUI mouseSensText;
    [SerializeField] private string mouseSenseParamter = "mouseSens";

    [SerializeField] private float minMouseSense = 1;
    [SerializeField] private float maxMouseSense = 10;

    private void Awake()
    {
        camController = FindFirstObjectByType<CameraController>();
    }

    public void KeyboardSensitivity(float value)
    {
        float newSensetivity = Mathf.Lerp(minKeyboardSens,maxKeyboardSens, value);
        camController.AdjustKeyboardSenseitivty(newSensetivity);

        keyboardSensText.text = Mathf.RoundToInt(value * 100) + "%";
    }

    public void MouseSensitivity(float value)
    {
        float newSenseitivty = Mathf.Lerp(minMouseSense,maxMouseSense, value);
        camController.AdjustMouseSensetivity(newSenseitivty);

        mouseSensText.text = Mathf.RoundToInt(value * 100) + "%";
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(keyboardSenseParametr, keyboardSenseSlider.value);
        PlayerPrefs.SetFloat(mouseSenseParamter, mouseSenseSlider.value);
    }

    private void OnEnable()
    {
        keyboardSenseSlider.value = PlayerPrefs.GetFloat(keyboardSenseParametr, .6f);
        mouseSenseSlider.value = PlayerPrefs.GetFloat(mouseSenseParamter, .6f);
    }
}
