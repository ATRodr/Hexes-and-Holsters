using UnityEngine;
using System.Collections.Generic;

namespace Code.Scripts.SkillTreeSystem
{
    [CreateAssetMenu(fileName = "New Skill", menuName = "Skill System/New Skill", order = 0)]
    public class ScriptableSkill : ScriptableObject
    {
        public List<UpgradeData> UpgradeData = new List<UpgradeData>();
        public bool IsAbility;
        public bool isCowboySkill;
        public string SkillName;
        [TextArea(1, 4)] public string SkillDescription;
        public Sprite SkillIcon;
        public List<ScriptableSkill> SkillPrerequisites = new List<ScriptableSkill>();
        public int SkillTier;
        public int Cost;
        public int CoolDown;
        private void OnValidate()
        {
            if (SkillName == "") SkillName = name;
        }
    }


    [System.Serializable]
    public class UpgradeData
    {
        public StatTypes StatType;
        public int SkillIncreaseAmount;
        public bool IsPercentage;
    }

    public enum StatTypes
    {
        russianRoulette,
        dynamiteDash, 
        goldenGun,
        shieldOfFaith,
        polyMorph,
        slowEnemy
    }
}