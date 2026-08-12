using UnityEngine;

public class UI_BuildButtons : MonoBehaviour
{
    [SerializeField] private float yPositionOffset;
    [SerializeField] private float openAnimationDuration = .1f;

    private bool isBuildMenuActive;
    private UI_Animator uiAnim;

    private UI_BuildButtonOnHoverEffect[] buildButtons;

    private void Awake()
    {
        uiAnim = GetComponentInParent<UI_Animator>();
        buildButtons = GetComponentsInChildren<UI_BuildButtonOnHoverEffect>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
            ShowBuildButtons();
    }

    public void ShowBuildButtons()
    {
        isBuildMenuActive = !isBuildMenuActive;

        float yOffset = isBuildMenuActive ? yPositionOffset : -yPositionOffset;
        float methodDelay = isBuildMenuActive ? openAnimationDuration : 0;

        uiAnim.ChangePosition(transform, new Vector3(0, yOffset), openAnimationDuration);

        Invoke(nameof(ToggleButtonMovement), methodDelay);
    }

    private void ToggleButtonMovement()
    {
        foreach(var button in buildButtons)
        {
            button.ToggleMovement(isBuildMenuActive);
        }
    }
}
