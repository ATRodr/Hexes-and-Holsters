using System.Collections;
using System.Collections.Generic;
using Code.Scripts.SkillTreeSystem;
using UnityEngine;
using UnityEngine.UI;

public class CooldownUIController : MonoBehaviour
{
    private PlayerSkillManager playerSkillManager;
    private AimSystem aimSystem;
    private Image ability1BW;
    private Image ability2BW;
    private Image ultimateBW;
    private Image ability1;
    private Image ability2;
    private Image ultimate;

    [Header("Ability 1")]
    public Image abilityImage1;
    public float cooldown1 = 0;
    public bool isCooldown1 = false;
    public KeyCode ability1Key;

    [Header("Ability 2")]
    public Image abilityImage2;
    public float cooldown2 = 0;
    public bool isCooldown2 = false;
    public KeyCode ability2Key;

    [Header("Ultimate")]
    public Image ultimateImage;
    public float cooldown3 = 0;
    public bool isCooldown3 = false;
    public KeyCode ultimateKey;
    
    void Start()
    {
        playerSkillManager = GameObject.Find("REALPlayerPrefab").GetComponent<PlayerSkillManager>();
        aimSystem = GameObject.Find("REALPlayerPrefab").GetComponent<AimSystem>();
        ability1 = GameObject.Find("Ability1").GetComponent<Image>();
        ability2 = GameObject.Find("Ability2").GetComponent<Image>();
        ultimate = GameObject.Find("Ultimate").GetComponent<Image>();
        ability1BW = GameObject.Find("Ability1BW").GetComponent<Image>();
        ability2BW = GameObject.Find("Ability2BW").GetComponent<Image>();
        ultimateBW = GameObject.Find("UltimateBW").GetComponent<Image>();
        abilityImage1.fillAmount = 0;
        abilityImage2.fillAmount = 0;
        ability1.sprite = null;
        ability1BW.sprite = null;
        cooldown1 = 0;
        ability2.sprite = null;
        ability2BW.sprite = null;
        cooldown2 = 0;
        UpdateCooldowns();
    }

    void Update()
    {
        Ablility1();
        Ablility2();
        Ultimate();
    }

    void Ablility1()
    {
        if (Input.GetKeyDown(KeyCode.E) && isCooldown1 == false)
        {
            isCooldown1 = true;
            abilityImage1.fillAmount = 1;
        }

        if (isCooldown1)
        {
            abilityImage1.fillAmount -= 1 / cooldown1 * Time.deltaTime;

            if (abilityImage1.fillAmount <= 0)
            {
                abilityImage1.fillAmount = 0;
                isCooldown1 = false;
            }
        }
    }
    void Ablility2()
    {
        if (Input.GetKeyDown(KeyCode.F) && isCooldown2 == false)
        {
            isCooldown2 = true;
            abilityImage2.fillAmount = 1;
        }

        if (isCooldown2)
        {
            abilityImage2.fillAmount -= 1 / cooldown2 * Time.deltaTime;

            if (abilityImage2.fillAmount <= 0)
            {
                abilityImage2.fillAmount = 0;
                isCooldown2 = false;
            }
        }
    }

    void Ultimate()
    {
        
    }

    public void UpdateCooldowns()
    {
        ScriptableSkill skillOne = aimSystem.isCowboy ? playerSkillManager.ActiveCowboySkills.GetSkill(1) :
            playerSkillManager.ActiveWizardSkills.GetSkill(1);
        ScriptableSkill skillTwo = aimSystem.isCowboy ? playerSkillManager.ActiveCowboySkills.GetSkill(2) :
            playerSkillManager.ActiveWizardSkills.GetSkill(2);
        
        // update skill icons and cooldowns
        if (skillOne != null)
        {
            ability1.sprite = skillOne.SkillIcon;
            ability1BW.sprite = skillOne.SkillIcon;
            cooldown1 = skillOne.CoolDown;
        }
        else
        {
            ability1.sprite = null;
            ability1BW.sprite = null;
            cooldown1 = 0;
        }

        if (skillTwo != null)
        {
            ability2.sprite = skillTwo.SkillIcon;
            ability2BW.sprite = skillTwo.SkillIcon;
            cooldown2 = skillTwo.CoolDown;  
        }
        else
        {
            ability2.sprite = null;
            ability2BW.sprite = null;
            cooldown2 = 0;
        }
    }
}
