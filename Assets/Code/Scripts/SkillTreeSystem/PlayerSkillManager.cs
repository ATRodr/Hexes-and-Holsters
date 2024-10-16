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

        public int ChainLightningLevel => chainLightningLevel;
        public int DestructiveWaveLevel => destructiveWaveLevel;
        public int DynamiteDashLevel => dynamiteDashLevel;
        public int GoldenGunLevel => goldenGunLevel;
        
        public bool ChainLightning => chainLightningLevel > 0;
        public bool DestructiveWave => destructiveWaveLevel > 0;
        public bool DynamiteDash => dynamiteDashLevel > 0;
        public bool GoldenGun => goldenGunLevel > 0;
        
        public int SkillPoints => skillPoints;

        public UnityAction OnSkillPointsChanged;

        private List<ScriptableSkill> unlockedSkills = new List<ScriptableSkill>();

        private void Awake()
        {
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
            foreach (UpgradeData data in skill.UpgradeData)
            {
                switch (data.statType)
                {
                    case StatType.chainLightningLevel:
                        ModifyStat(ref chainLightningLevel, data);
                        break;
                    case StatType.destructiveWaveLevel:
                        ModifyStat(ref destructiveWaveLevel, data);
                        break;
                    case StatType.dynamiteDashLevel:
                        ModifyStat(ref dynamiteDashLevel, data);
                        break;
                    case StatType.goldenGunLevel:
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
    }
}
