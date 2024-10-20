using Code.Scripts.SkillTreeSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Code.Scripts.SkillTreeSystem;

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

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        playerSkillManager = FindObjectOfType<PlayerSkillManager>();
    }

    private void Start()
    {
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