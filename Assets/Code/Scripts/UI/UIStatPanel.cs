using UnityEngine;
using UnityEngine.UIElements;

public class UIStatPanel : MonoBehaviour
{
    private Label shieldOfFaithLabel, timeSlowLabel, charmPersonLabel, dynamiteDashLabel, goldenGunLabel, russianRouletteLabel;
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
        // NEED TO UPDATE THIS FOR ALL ABILITIES LATER
        shieldOfFaithLabel = uiManager.UIDocument.rootVisualElement.Q<Label>("ShieldOfFaith");
        timeSlowLabel = uiManager.UIDocument.rootVisualElement.Q<Label>("TimeSlow");
        charmPersonLabel = uiManager.UIDocument.rootVisualElement.Q<Label>("CharmPerson");
        dynamiteDashLabel = uiManager.UIDocument.rootVisualElement.Q<Label>("DynamiteDash");
        goldenGunLabel = uiManager.UIDocument.rootVisualElement.Q<Label>("GoldenGun");
        russianRouletteLabel = uiManager.UIDocument.rootVisualElement.Q<Label>("RussianRoullete");
        skillPointsLabel = uiManager.UIDocument.rootVisualElement.Q<Label>("SkillPoints");
    }

    private void PopulateLabelText()
    {
        skillPointsLabel.text = "Skill Points: " + uiManager.PlayerSkillManager.SkillPoints.ToString();
        shieldOfFaithLabel.text = "Shield of Faith: " + (uiManager.PlayerSkillManager.ShieldOfFaith > 0 ? "Level " + uiManager.PlayerSkillManager.ShieldOfFaith.ToString() : "Locked");
        dynamiteDashLabel.text = "Dynamite Dash: " + (uiManager.PlayerSkillManager.DynamiteDash > 0 ? "Level " + uiManager.PlayerSkillManager.DynamiteDash.ToString() : "Locked");
        goldenGunLabel.text = "Golden Gun: " + (uiManager.PlayerSkillManager.GoldenGun > 0 ? "Level " + uiManager.PlayerSkillManager.GoldenGun.ToString() : "Locked");
        russianRouletteLabel.text = "Russian Roulette: " + (uiManager.PlayerSkillManager.russianRoulette > 0 ? "Level " + uiManager.PlayerSkillManager.russianRoulette.ToString() : "Locked");
        timeSlowLabel.text = "Time Slow: " + (uiManager.PlayerSkillManager.SlowEnemy > 0 ? "Level " + uiManager.PlayerSkillManager.SlowEnemy.ToString() : "Locked");
        charmPersonLabel.text = "Charm Person: " + (uiManager.PlayerSkillManager.PolyMorph > 0 ? "Level " + uiManager.PlayerSkillManager.PolyMorph.ToString() : "Locked");
    }
}