using Code.Scripts.SkillTreeSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] private ScriptableSkillLibrary skillLibrary;
    public ScriptableSkillLibrary SkillLibrary => skillLibrary;
    [SerializeField] private VisualTreeAsset uiTalentButton;
    private PlayerSkillManager playerSkillManager;
    public PlayerSkillManager PlayerSkillManager => playerSkillManager;

    private UIDocument  uiDocument;
    public UIDocument UIDocument => uiDocument;

    private VisualElement skillTopRow, skillMiddleRow, skillBottomRow;
    [SerializeField] private List<UITalentButton> talentButtons;

    private IEnumerator Start()
    {
        MainManager.Instance.uiManager = this;
        while (MainManager.Instance == null || MainManager.Instance.playerSkillManager == null)
        {
            yield return null; // Wait for the next frame
        }
        playerSkillManager = MainManager.Instance.playerSkillManager;
        uiDocument = GetComponent<UIDocument>();
        CreateSkillButtons();
    }

    private void CreateSkillButtons()
    {
        var root = uiDocument.rootVisualElement;
        skillTopRow = root.Q<VisualElement>("SkillRow1");
        skillMiddleRow = root.Q<VisualElement>("SkillRow2");
        skillBottomRow = root.Q<VisualElement>("SkillRow3");

        SpawnButtons(skillTopRow, skillLibrary.GetSkillsOfTier(1));
        SpawnButtons(skillMiddleRow, skillLibrary.GetSkillsOfTier(2));
        SpawnButtons(skillBottomRow, skillLibrary.GetSkillsOfTier(3));
    }

    private void SpawnButtons(VisualElement parent, List<ScriptableSkill> skills)
    {
        foreach (var skill in skills)
        {
            Button clonedButton = uiTalentButton.CloneTree().Q<Button>();
            talentButtons.Add(new UITalentButton(clonedButton, skill));
            parent.Add(clonedButton);
        }
    }
}