using UnityEngine;
using UnityEngine.UIElements;

public class UIStatPanel : MonoBehaviour
{
    private Label chainLightningLabel, destructiveWaveLabel, dynamiteDashLabel, goldenGunLabel;
    private Label skillPointsLabel;

    private UIManager uiManager;

    private void Awake()
    {
        uiManager = GetComponent<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("UIStatPanel: UIManager not found!");
        }
    }

    void Start()
    {
        GatherLabelReferences();
        uiManager.PlayerSkillManager.OnSkillPointsChanged += PopulateLabelText;
        PopulateLabelText();
    }

    private void GatherLabelReferences()
    {
        chainLightningLabel = uiManager.UIDocument.rootVisualElement.Q<Label>("ChainLightning");
        destructiveWaveLabel = uiManager.UIDocument.rootVisualElement.Q<Label>("DestructiveWave");
        dynamiteDashLabel = uiManager.UIDocument.rootVisualElement.Q<Label>("DynamiteDash");
        goldenGunLabel = uiManager.UIDocument.rootVisualElement.Q<Label>("GoldenGun");
        skillPointsLabel = uiManager.UIDocument.rootVisualElement.Q<Label>("SkillPoints");

        Debug.Log(chainLightningLabel);
        Debug.Log(destructiveWaveLabel);
        Debug.Log(dynamiteDashLabel);
        Debug.Log(goldenGunLabel);
        Debug.Log(skillPointsLabel);
    }

    private void PopulateLabelText()
    {
        skillPointsLabel.text = "Skill Points: " + uiManager.PlayerSkillManager.SkillPoints.ToString();
        chainLightningLabel.text = "Chain Lightning: " + (uiManager.PlayerSkillManager.ChainLightning > 0 ? "Level " + uiManager.PlayerSkillManager.ChainLightning.ToString() : "Locked");
        destructiveWaveLabel.text = "Destructive Wave: " + (uiManager.PlayerSkillManager.DestructiveWave > 0 ? "Level " + uiManager.PlayerSkillManager.DestructiveWave.ToString() : "Locked");
        dynamiteDashLabel.text = "Dynamite Dash: " + (uiManager.PlayerSkillManager.DynamiteDash > 0 ? "Level " + uiManager.PlayerSkillManager.DynamiteDash.ToString() : "Locked");
        goldenGunLabel.text = "Golden Gun: " + (uiManager.PlayerSkillManager.GoldenGun > 0 ? "Level " + uiManager.PlayerSkillManager.GoldenGun.ToString() : "Locked");
    }
}
