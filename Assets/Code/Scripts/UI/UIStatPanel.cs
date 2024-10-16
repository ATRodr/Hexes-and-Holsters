using System.Collections;
using System.Collections.Generic;
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
        uiManager.PlayerSkillManager.OnSkillPointsChanged += PopulateLabelText;
        GatherLabelReferences();
        PopulateLabelText();
    }

    private void GatherLabelReferences()
    {
        chainLightningLabel = uiManager.UIDocument.rootVisualElement.Q<Label>("ChainLightning");
        destructiveWaveLabel = uiManager.UIDocument.rootVisualElement.Q<Label>("DestructiveWave");
        dynamiteDashLabel = uiManager.UIDocument.rootVisualElement.Q<Label>("DynamiteDash");
        goldenGunLabel = uiManager.UIDocument.rootVisualElement.Q<Label>("GoldenGun");
        skillPointsLabel = uiManager.UIDocument.rootVisualElement.Q<Label>("SkillPoints");
    }

    private void PopulateLabelText()
    {
        skillPointsLabel.text = "Skill Points: " + uiManager.PlayerSkillManager.SkillPoints.ToString();

        chainLightningLabel.text = "Chain Lightning: " + (uiManager.PlayerSkillManager.ChainLightning ? uiManager.PlayerSkillManager.ChainLightningLevel : "Locked");
        destructiveWaveLabel.text = "Destructive Wave: " + (uiManager.PlayerSkillManager.DestructiveWave ? uiManager.PlayerSkillManager.DestructiveWaveLevel : "Locked");
        dynamiteDashLabel.text = "Dynamite Dash: " + (uiManager.PlayerSkillManager.DynamiteDash ? uiManager.PlayerSkillManager.DynamiteDashLevel : "Locked");
        goldenGunLabel.text = "Golden Gun: " + (uiManager.PlayerSkillManager.GoldenGun ? uiManager.PlayerSkillManager.GoldenGunLevel : "Locked");
    }
}
