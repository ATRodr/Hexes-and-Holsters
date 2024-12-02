using System.Collections;
using System.Collections.Generic;
using Code.Scripts.SkillTreeSystem;
using UnityEngine;
using UnityEngine.UI;

public class CooldownUIController : MonoBehaviour
{
    private PlayerSkillManager playerSkillManager;
    private AimSystem aimSystem;
    private Ultimates ultimates;
    private Image ability1BW;
    private Image ability2BW;
    private Image ultimateBW;
    private Image ability1;
    private Image ability2;
    private Image ultimate;
    [SerializeField] private Sprite[] diceIcons;
    [SerializeField] private Sprite[] cowboyUltIcons;
    [SerializeField] private Sprite[] wizardUltIcons;

    [Header("Ability 1")]
    public Image abilityImage1;
    public float cooldown1 = 0;
    public bool isCooldown1 = false;

    [Header("Ability 2")]
    public Image abilityImage2;
    public float cooldown2 = 0;
    public bool isCooldown2 = false;

    [Header("Ultimate")]
    public Image ultimateImage;
    public float cooldown3 = 0;
    public bool isCooldown3 = false;
    
    void Start()
    {
        playerSkillManager = GameObject.Find("REALPlayerPrefab").GetComponent<PlayerSkillManager>();
        aimSystem = GameObject.Find("REALPlayerPrefab").GetComponent<AimSystem>();
        ultimates = GameObject.Find("REALPlayerPrefab").GetComponent<Ultimates>();
        ability1 = GameObject.Find("Ability1").GetComponent<Image>();
        ability2 = GameObject.Find("Ability2").GetComponent<Image>();
        ultimate = GameObject.Find("Ultimate").GetComponent<Image>();
        ability1BW = GameObject.Find("Ability1BW").GetComponent<Image>();
        ability2BW = GameObject.Find("Ability2BW").GetComponent<Image>();
        ultimateBW = GameObject.Find("UltimateBW").GetComponent<Image>();
        abilityImage1.fillAmount = 0;
        abilityImage2.fillAmount = 0;
        ultimateImage.fillAmount = 0;
        ability1.sprite = null;
        ability1BW.sprite = null;
        ability2.sprite = null;
        ability2BW.sprite = null;
        ultimate.sprite = diceIcons[0];
        ultimateBW.sprite = diceIcons[0];
        cooldown1 = 0;
        cooldown2 = 0;
        cooldown3 = ultimates.COWBOY_COOLDOWN;
        isCooldown3 = true;
        ultimateImage.fillAmount = 1;
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
        if (Input.GetKeyDown(KeyCode.Q) && isCooldown3 == false)
        {
            // if ult has not been rolled and is ready to roll
            if ((!ultimates.cowboyUltReady && ultimates.cowboyUltCooldown <= 0 && aimSystem.isCowboy) || (!ultimates.wizardUltReady && ultimates.wizardUltCooldown <= 0 && !aimSystem.isCowboy))
            {
                // roll cowboy ult
                UpdateUltimate(rolledWhileCowboy:aimSystem.isCowboy);
                return;
            }
            else if (ultimates.cowboyUltReady || ultimates.wizardUltReady)
            {
                // fire cowboy ult
                return;
            }
            else 
            {
                ultimate.sprite = diceIcons[0];
                ultimateBW.sprite = diceIcons[0];
                isCooldown3 = true;
                ultimateImage.fillAmount = 1;
            }
        }

        if (isCooldown3)
        {

            ultimateImage.fillAmount -= 1 / cooldown3 * Time.deltaTime;

            if (ultimateImage.fillAmount <= 0)
            {
                ultimateImage.fillAmount = 0;
                isCooldown3 = false;
            }
        }
        
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
    public void UpdateUltimate(int ult = 0, bool fromSwap = false, bool rolledWhileCowboy = false)
    {
        if (ultimates.cowboyUltReady && aimSystem.isCowboy)
        {
            ultimate.sprite = cowboyUltIcons[ultimates.cowboyUlt-1];
            return;
        }
        if (ultimates.wizardUltReady && !aimSystem.isCowboy)
        {
            ultimate.sprite = wizardUltIcons[ultimates.wizardUlt-1];
            return;
        }
        if (fromSwap)
        {
            ultimate.sprite = diceIcons[0];
            ultimateBW.sprite = diceIcons[0];
            return;
        }

        switch (ult)
        {
            case 0:
                // roll for ultimate
                StartCoroutine(RollUltimate(rolledWhileCowboy));
                break;
            case 1:
                // if cowboy, set cowboy ult 1 sprite, else set wizard ult 1 sprite
                if (aimSystem.isCowboy && rolledWhileCowboy)
                {
                    ultimate.sprite = cowboyUltIcons[0];
                    ultimateBW.sprite = cowboyUltIcons[0];
                }
                else if (!aimSystem.isCowboy && !rolledWhileCowboy)
                {
                    ultimate.sprite = wizardUltIcons[0];
                    ultimateBW.sprite = wizardUltIcons[0];
                }
                else
                {
                    return;
                }
                break;
            case 2:
                if (aimSystem.isCowboy && rolledWhileCowboy)
                {
                    ultimate.sprite = cowboyUltIcons[1];
                    ultimateBW.sprite = cowboyUltIcons[1];
                }
                else if (!aimSystem.isCowboy && !rolledWhileCowboy)
                {
                    ultimate.sprite = wizardUltIcons[1];
                    ultimateBW.sprite = wizardUltIcons[1];
                }
                else
                {
                    return;
                }
                break;
            case 3:
                if (aimSystem.isCowboy && rolledWhileCowboy)
                {
                    ultimate.sprite = cowboyUltIcons[2];
                    ultimateBW.sprite = cowboyUltIcons[2];
                }
                else if (!aimSystem.isCowboy && !rolledWhileCowboy)
                {
                    ultimate.sprite = wizardUltIcons[2];
                    ultimateBW.sprite = wizardUltIcons[2];
                }
                else
                {
                    return;
                }
                break;
        }
    }

    IEnumerator RollUltimate(bool rolledWhileCowboy)
    {
        float elapsedTime = 0f;
        while (elapsedTime < ultimates.rollTime)
        {
            if (aimSystem.isCowboy && rolledWhileCowboy)
            {
                ultimate.sprite = diceIcons[Random.Range(0, diceIcons.Length)];
                ultimateBW.sprite = ultimate.sprite;
            }
            else if (!aimSystem.isCowboy && !rolledWhileCowboy)
            {
                ultimate.sprite = diceIcons[Random.Range(0, diceIcons.Length)];
                ultimateBW.sprite = ultimate.sprite;
            }
            else
            {
                UpdateUltimate(fromSwap: true);
            }
            yield return new WaitForSeconds(0.1f);
            elapsedTime += 0.1f; // Increment elapsed time
        }
    }
}
