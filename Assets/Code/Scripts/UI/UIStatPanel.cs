using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIStatPanel : MonoBehaviour
{
    private Label chainLightningLabel, destructiveWaveLabel, dynamiteDashLabel, goldenGunLabel;
    private Label skillPointsLabel;

    private UIManager uiManager;

    private IEnumerator Start()
    {
        while (MainManager.Instance == null || MainManager.Instance.uiManager == null)
        {
            yield return null;
        }
        uiManager = MainManager.Instance.uiManager;
        uiManager.PlayerSkillManager.OnSkillPointsChanged += PopulateLabelText;
        GatherLabelReferences();
        PopulateLabelText();
    }

    private void GatherLabelReferences()
    {
        // NEED TO UPDATE THIS FOR ALL ABILITIES LATER
        chainLightningLabel = uiManager.UIDocument.rootVisualElement.Q<Label>("ChainLightning");
        destructiveWaveLabel = uiManager.UIDocument.rootVisualElement.Q<Label>("DestructiveWave");
        dynamiteDashLabel = uiManager.UIDocument.rootVisualElement.Q<Label>("DynamiteDash");
        goldenGunLabel = uiManager.UIDocument.rootVisualElement.Q<Label>("GoldenGun");
        skillPointsLabel = uiManager.UIDocument.rootVisualElement.Q<Label>("SkillPoints");
    }

    private void PopulateLabelText()
    {
        skillPointsLabel.text = "Skill Points: " + uiManager.PlayerSkillManager.SkillPoints.ToString();
        chainLightningLabel.text = "Shield of Faith: " + (uiManager.PlayerSkillManager.ShieldOfFaith > 0 ? "Level " + uiManager.PlayerSkillManager.ShieldOfFaith.ToString() : "Locked");
        destructiveWaveLabel.text = "Destructive Wave: " + (uiManager.PlayerSkillManager.DestructiveWave > 0 ? "Level " + uiManager.PlayerSkillManager.DestructiveWave.ToString() : "Locked");
        dynamiteDashLabel.text = "Dynamite Dash: " + (uiManager.PlayerSkillManager.DynamiteDash > 0 ? "Level " + uiManager.PlayerSkillManager.DynamiteDash.ToString() : "Locked");
        goldenGunLabel.text = "Golden Gun: " + (uiManager.PlayerSkillManager.GoldenGun > 0 ? "Level " + uiManager.PlayerSkillManager.GoldenGun.ToString() : "Locked");
    }
}