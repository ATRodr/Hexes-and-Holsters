using UnityEngine;
using System.Collections.Generic;
namespace Code.Scripts.SkillTreeSystem
{
    [CreateAssetMenu(fileName = "New Skill", menuName = "Skill System/New Skill", order = 0)]
    public class ScriptableSkill : ScriptableObject
    {
        public List<UpgradeData> UpgradeData = new List<UpgradeData>();
        public bool IsAbility;
        public string SkillName;
        [TextArea(1, 4)] public string SkillDescription;
        public Sprite SkillIcon;
        public List<ScriptableSkill> SkillPrerequisites = new List<ScriptableSkill>();
        public int SkillTier;
        public int Cost;

        private void OnValidate()
        {
            if (SkillName == "") SkillName = name;
        }
    }


    [System.Serializable]
    public class UpgradeData
    {
        public StatType statType;
        public int SkillIncreaseAmount;
        public bool IsPercentage;
    }

    public enum StatType
    {
        chainLightningLevel, 
        destructiveWaveLevel, 
        dynamiteDashLevel, 
        goldenGunLevel
    }
}
