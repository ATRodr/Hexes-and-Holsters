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
        private int chainLightning, destructiveWave, dynamiteDash, goldenGun;
        private int skillPoints;
        
        public int ChainLightning => chainLightning;
        public int DestructiveWave => destructiveWave;
        public int DynamiteDash => dynamiteDash;
        public int GoldenGun => goldenGun;
        
        public int SkillPoints => skillPoints;

        public UnityAction OnSkillPointsChanged;

        private List<ScriptableSkill> unlockedSkills = new List<ScriptableSkill>();

        private void Awake()
        {
            skillPoints = 10;
            chainLightning = 0;
            destructiveWave = 0;
            dynamiteDash = 0;
            goldenGun = 0;
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
            foreach (UpgradeData data in skill.UpgradeData)
            {
                switch (data.StatType)
                {
                    case StatTypes.chainLightning:
                        ModifyStat(ref chainLightning, data);
                        break;
                    case StatTypes.destructiveWave:
                        ModifyStat(ref destructiveWave, data);
                        break;
                    case StatTypes.dynamiteDash:
                        ModifyStat(ref dynamiteDash, data);
                        break;
                    case StatTypes.goldenGun:
                        ModifyStat(ref goldenGun, data);
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
    }
}
