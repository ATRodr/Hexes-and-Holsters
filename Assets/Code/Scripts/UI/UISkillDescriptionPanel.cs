using UnityEngine;
using UnityEngine.UIElements;
using Code.Scripts.SkillTreeSystem;
using System.Collections;
using System.Collections.Generic;
using System;

public class UISkillDescriptionPanel : MonoBehaviour
{
    private UIManager uiManager;
    private ScriptableSkill assignedSkill;
    private VisualElement skillImage;
    private Label skillNameLabel, skillDescriptionLabel, skillCostLabel, skillPreReqLabel;
    private Button purchaseSkillButton;
    

    private void OnEnable()
    {
        UITalentButton.OnSkillButtonClicked += PopulateLabelText;
    }

    private void OnDisable()
    {
        UITalentButton.OnSkillButtonClicked -= PopulateLabelText;
        if (purchaseSkillButton != null) purchaseSkillButton.clicked -= PurchaseSkill;
    }

    private IEnumerator Start()
    {
        while (MainManager.Instance == null || MainManager.Instance.uiManager == null)
        {
            yield return null;
        }
        uiManager = MainManager.Instance.uiManager;
        GatherLabelReferences();
        var skill = uiManager.SkillLibrary.GetSkillsOfTier(1)[0];
        PopulateLabelText(skill);
    }

    private void GatherLabelReferences()
    {
        skillImage = uiManager.UIDocument.rootVisualElement.Q<VisualElement>("Icon");
        skillNameLabel = uiManager.UIDocument.rootVisualElement.Q<Label>("SkillNameLabel");
        skillDescriptionLabel = uiManager.UIDocument.rootVisualElement.Q<Label>("SkillDescriptionLabel");
        skillCostLabel = uiManager.UIDocument.rootVisualElement.Q<Label>("SkillCostLabel");
        skillPreReqLabel = uiManager.UIDocument.rootVisualElement.Q<Label>("PreReqLabel");
        purchaseSkillButton = uiManager.UIDocument.rootVisualElement.Q<Button>("BuySkillButton");
        purchaseSkillButton.clicked += PurchaseSkill;
    }

    private void PurchaseSkill()
    {
        if (uiManager.PlayerSkillManager.CanAffordSkill(assignedSkill))
        {
            uiManager.PlayerSkillManager.UnlockSkill(assignedSkill);
            PopulateLabelText(assignedSkill);
        }
    }

    private void PopulateLabelText(ScriptableSkill skill)
    {
        if (skill == null) return;

        assignedSkill = skill;
        if (assignedSkill.SkillIcon) skillImage.style.backgroundImage = new StyleBackground(assignedSkill.SkillIcon);
        skillNameLabel.text = assignedSkill.SkillName;
        skillDescriptionLabel.text = assignedSkill.SkillDescription;
        skillCostLabel.text = "Cost: " + assignedSkill.Cost.ToString();

        if (assignedSkill.SkillPrerequisites.Count > 0)
        {
            skillPreReqLabel.text = "Prerequisites: ";
            foreach (var preReq in assignedSkill.SkillPrerequisites)
            {
                string punctuation = preReq == assignedSkill.SkillPrerequisites[assignedSkill.SkillPrerequisites.Count - 1] ? "" : ", ";
                skillPreReqLabel.text += " " + preReq.SkillName + punctuation;
            }
        }
        else
        {
            skillPreReqLabel.text = "Prerequisites: None";
        }

        // if the skill is active
        if (uiManager.PlayerSkillManager.isSkillActive(assignedSkill))
        {
            purchaseSkillButton.text = "Active";
            purchaseSkillButton.SetEnabled(false);
        }
        // if the skill is inactive and skill is unlocked
        else if (uiManager.PlayerSkillManager.IsSkillUnlocked(assignedSkill))
        {
            purchaseSkillButton.text = "Activate";
            purchaseSkillButton.SetEnabled(true);
        }
        // if the skill is inactive and skill is locked and prerequisites are not met
        else if (!uiManager.PlayerSkillManager.PreReqsMet(assignedSkill))
        {
            purchaseSkillButton.text = "Prerequisites Not Met";
            purchaseSkillButton.SetEnabled(false);
        }
        // if the skill is inactive and skill is locked and prerequisites are met and player cannot afford the skill
        else if (!uiManager.PlayerSkillManager.CanAffordSkill(assignedSkill))
        {
            purchaseSkillButton.text = "Insufficient Skill Points";
            purchaseSkillButton.SetEnabled(false);
        }
        else
        {
            purchaseSkillButton.text = "Purchase";
            purchaseSkillButton.SetEnabled(true);
        }

    }
}