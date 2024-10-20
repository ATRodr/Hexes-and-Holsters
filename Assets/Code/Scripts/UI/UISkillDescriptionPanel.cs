using UnityEngine;
using UnityEngine.UIElements;
using Code.Scripts.SkillTreeSystem;

public class UISkillDescriptionPanel : MonoBehaviour
{
    private UIManager uiManager;
    private ScriptableSkill assignedSkill;
    private VisualElement skillImage;
    private Label skillNameLabel, skillDescriptionLabel, skillCostLabel, skillPreReqLabel;
    private Button purchaseSkillButton;
    

    private void Awake()
    {
        uiManager = GetComponent<UIManager>();
    }

    private void OnEnable()
    {
        UITalentButton.OnSkillButtonClicked += PopulateLabelText;
    }

    private void OnDisable()
    {
        UITalentButton.OnSkillButtonClicked -= PopulateLabelText;
        if (purchaseSkillButton != null) purchaseSkillButton.clicked -= PurchaseSkill;
    }

    private void Start()
    {
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

        if (uiManager.PlayerSkillManager.IsSkillUnlocked(assignedSkill))
        {
            purchaseSkillButton.text = "Purchased";
            purchaseSkillButton.SetEnabled(false);
        }
        else if (!uiManager.PlayerSkillManager.PreReqsMet(assignedSkill))
        {
            purchaseSkillButton.text = "Prerequisites Not Met";
            purchaseSkillButton.SetEnabled(false);
        }
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