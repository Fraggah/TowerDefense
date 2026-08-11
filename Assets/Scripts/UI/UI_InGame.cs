using TMPro;
using UnityEngine;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthPointsText;
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private TextMeshProUGUI waveTimerText;


    public void UpdateHealthPointsUI(int value, int maxValue)
    {
        int newValue = maxValue - value;
        healthPointsText.text = "Threat: " + newValue + "/" + maxValue;
    }

    public void UpdateCurrencyUI(int value)
    {
        currencyText.text = "scrap: " + value;
    }

    public void UpdateWaveTimerUI(float value) => waveTimerText.text = "seconds: " + value.ToString("00");
    public void EnableWaveTimer(bool enable) => waveTimerText.transform.parent.gameObject.SetActive(enable);

    public void ForceWaveButton()
    {
        WaveManager waveManager = FindAnyObjectByType<WaveManager>();
        waveManager.ForceNextWave();
    }
}
