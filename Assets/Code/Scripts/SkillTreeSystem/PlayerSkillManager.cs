using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Scripts.SkillTreeSystem
{
    public class PlayerSkillManager : MonoBehaviour
    {
        // Start is called before the first frame update

        // unlockable abilities
        private int chainLightningLevel, destructiveWaveLevel, dynamiteDashLevel, goldenGunLevel;
        private int skillPoints;
        
        public int ChainLightning => chainLightningLevel;
        public int DestructiveWave => destructiveWaveLevel;
        public int DynamiteDash => dynamiteDashLevel;
        public int GoldenGun => goldenGunLevel;
        
        public int SkillPoints => skillPoints;

        public UnityAction OnSkillPointsChanged;

        private List<ScriptableSkill> unlockedSkills = new List<ScriptableSkill>();

        //used for calling abilities and controlling player
        private PlayerController playerController;
        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
            skillPoints = 10;
            chainLightningLevel = 0;
            destructiveWaveLevel = 0;
            dynamiteDashLevel = 0;
            goldenGunLevel = 0;
        }
        
        public void GainSkillPoint()
        {
            skillPoints++;
            OnSkillPointsChanged?.Invoke();
        }

        public bool CanAffordSkill(ScriptableSkill skill)
        {
            return skillPoints >= skill.Cost;
        }

        public void UnlockSkill(ScriptableSkill skill)
        {
            if (!CanAffordSkill(skill)) return;
            ModifyStats(skill);
            unlockedSkills.Add(skill);
            skillPoints -= skill.Cost;
            OnSkillPointsChanged?.Invoke();
        }

        private void ModifyStats(ScriptableSkill skill)
        {
            foreach (var data in skill.UpgradeData)
            {
                switch (data.StatType)
                {
                    case StatTypes.chainLightning:
                        ModifyStat(ref chainLightningLevel, data);
                        break;
                    case StatTypes.destructiveWave:
                        ModifyStat(ref destructiveWaveLevel, data);
                        break;
                    case StatTypes.dynamiteDash:
                        ModifyStat(ref dynamiteDashLevel, data);
                        break;
                    case StatTypes.goldenGun:
                        ModifyStat(ref goldenGunLevel, data);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public bool IsSkillUnlocked(ScriptableSkill skill)
        {
            return unlockedSkills.Contains(skill);
        }

        public bool PreReqsMet(ScriptableSkill skill)
        {
            return skill.SkillPrerequisites.Count == 0 || skill.SkillPrerequisites.All(IsSkillUnlocked);
        }

        private void ModifyStat(ref int stat, UpgradeData data)
        {
            bool isPercentage = data.IsPercentage;
            if (isPercentage)
            {
                stat += (int) (stat * data.SkillIncreaseAmount / 100f);
            }
            else
            {
                stat += data.SkillIncreaseAmount;
            }
        }
        IEnumerator shieldOfFaith()
        {
            playerController.nextShieldOfFaith = Time.time + 15f;
            playerController.playerHealth.isInvincible = true;
            playerController.healthBar.DrawHearts();
            yield return new WaitForSeconds(5);
            playerController.playerHealth.isInvincible = false;
            playerController.healthBar.DrawHearts();
        }
        IEnumerator dynamiteDash()
        {
            playerController.nextDynamiteDash = Time.time + 15f;
            Vector2 pos = transform.position;
            Quaternion rot = transform.rotation;
            Debug.Log("Dynamite Dash");
            Instantiate(playerController.dynamite, pos, rot);
            StartCoroutine(playerController.Dash(0.16f, 27f));
            yield return new WaitForSeconds(0.75f);
            Instantiate(playerController.explosion, pos, rot);
        }
        public void cowboyAbility()
        {
            StartCoroutine(dynamiteDash());
            Debug.Log("Cowboy Ability");
        }
        public void wizardAbility()
        {
            StartCoroutine(shieldOfFaith());
            Debug.Log("Wizard Ability");
        }
    }
}