using TMPro;
using UnityEngine;

public class UI_InGame : MonoBehaviour
{
    private UI ui;
    private UI_Pause pauseUI;
    private UI_Animator uiAnimator;

    [SerializeField] private TextMeshProUGUI healthPointsText;
    [SerializeField] private TextMeshProUGUI currencyText;
    [Space]
    [SerializeField] private TextMeshProUGUI waveTimerText;
    [SerializeField] private float waveTimerOffset;
    [SerializeField] UI_TextBlinkEffect waveTimerTextBlinkEffect;

    private void Awake()
    {
        uiAnimator = GetComponentInParent<UI_Animator>();
        ui = GetComponentInParent<UI>();
        pauseUI = ui.GetComponentInChildren<UI_Pause>(true);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            ui.SwitchTo(pauseUI.gameObject);
    }

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
    public void EnableWaveTimer(bool enable)
    {
        Transform waveTimerTransform = waveTimerText.transform.parent;

        float yOffset = enable ? -waveTimerOffset : waveTimerOffset;

        Vector3 offset = new Vector3(0, yOffset);

        uiAnimator.ChangePosition(waveTimerTransform, offset);
        waveTimerTextBlinkEffect.EnableBlink(enable);
    }

    public void ForceWaveButton()
    {
        WaveManager waveManager = FindAnyObjectByType<WaveManager>();
        waveManager.ForceNextWave();
    }
}
