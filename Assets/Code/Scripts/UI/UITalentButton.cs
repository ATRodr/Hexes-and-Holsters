using UnityEngine;
using UnityEngine.UIElements;
using Code.Scripts.SkillTreeSystem;
using UnityEngine.Events;

[System.Serializable]
public class UITalentButton
{
    private Button button;
    private ScriptableSkill skill;
    private bool isUnlocked = false;

    public static UnityAction<ScriptableSkill> OnSkillButtonClicked;

    public UITalentButton(Button button, ScriptableSkill skill)
    {
        this.button = button;
        this.button.clicked += OnClick;
        this.skill = skill;
        if (skill.SkillIcon) button.style.backgroundImage = new StyleBackground(skill.SkillIcon);
    }

    private void OnClick()
    {
        OnSkillButtonClicked?.Invoke(skill);    
    }
}    
