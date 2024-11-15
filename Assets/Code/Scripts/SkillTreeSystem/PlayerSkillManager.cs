using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Code.Scripts.SkillTreeSystem
{

    // LRU Cache (Least Recently Used) keeps track of the oldest skill
    // activated and replaces it with new skill activated if player has
    // > 2 skills activated, also keeps track of the skill's ability number
    // to know which ability to fire
    class LRUCache
    {
        public int capacity;
        public Dictionary<ScriptableSkill, int> cache;
        public LRUCache()
        {
            capacity = 2;
            cache = new Dictionary<ScriptableSkill, int>();
        }
        public void Add(ScriptableSkill skill)
        {
            if (cache.ContainsKey(skill)) return;

            if (cache.Count == capacity)
            {
                int abilityNumber = cache[cache.Keys.First()];
                cache.Remove(cache.Keys.First());

                cache.Add(skill, abilityNumber);
            }
            else
            {
                cache.Add(skill, cache.Count + 1);
            }

        }
        
        // returns ability name based on ability number (1 or 2)
        public ScriptableSkill GetSkill(int number)
        {
            foreach (ScriptableSkill skill in cache.Keys)
            {
                if (cache[skill] == number)
                {
                    return skill;
                }
            }
            return null;
        }
    }
    public class PlayerSkillManager : MonoBehaviour
    {
        // Start is called before the first frame update
        private MainManager MainManager;
        // unlockable abilities
        private int chainLightningLevel, destructiveWaveLevel, dynamiteDashLevel, goldenGunLevel, shieldOfFaithLevel;
        private int skillPoints;
        private LRUCache activeCowboySkills;
        private LRUCache activeWizardSkills;        
        public int ChainLightning => chainLightningLevel;
        public int DestructiveWave => destructiveWaveLevel;
        public int DynamiteDash => dynamiteDashLevel;
        public int GoldenGun => goldenGunLevel;
        public int ShieldOfFaith => shieldOfFaithLevel;
        
        public int SkillPoints => skillPoints;

        public UnityAction OnSkillPointsChanged;

        private List<ScriptableSkill> unlockedSkills = new List<ScriptableSkill>();
        
        //used for calling abilities and controlling player
        private PlayerController playerController;

        private IEnumerator Start()
        {
            MainManager.Instance.playerSkillManager = this;
            while (MainManager.Instance == null || MainManager.Instance.playerController == null)
            {
                yield return null;
            }
            playerController = MainManager.Instance.playerController;
            activeCowboySkills = new LRUCache();
            activeWizardSkills = new LRUCache();
            skillPoints = 10;
            chainLightningLevel = 0;
            destructiveWaveLevel = 0;
            dynamiteDashLevel = 0;
            goldenGunLevel = 0;
            shieldOfFaithLevel = 0;
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
            ModifyStats(skill);
            unlockedSkills.Add(skill);
            ActivateSkill(skill);
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
                    case StatTypes.shieldOfFaith:
                        ModifyStat(ref shieldOfFaithLevel, data);
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
        public bool isSkillActive(ScriptableSkill skill)
        {
            return skill.isCowboySkill ? activeCowboySkills.cache.ContainsKey(skill) : activeWizardSkills.cache.ContainsKey(skill);
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

        public void ActivateSkill(ScriptableSkill skill)
        {
            Debug.Log("Activating skill: " + skill.SkillName);
            if (skill.isCowboySkill)
                activeCowboySkills.Add(skill);
            else
                activeWizardSkills.Add(skill);            
        }

        IEnumerator shieldOfFaith()
        {
            playerController.playerHealth.isInvincible = true;
            playerController.healthBar.DrawHearts(true);
            yield return new WaitForSeconds(5);
            playerController.playerHealth.isInvincible = false;
            playerController.healthBar.DrawHearts(false);
        }
        IEnumerator dynamiteDash()
        {
            Vector2 pos = transform.position;
            Quaternion rot = transform.rotation;
            Debug.Log("Dynamite Dash");
            Instantiate(playerController.dynamite, pos, rot);
            StartCoroutine(playerController.Dash(0.16f, 27f));
            yield return new WaitForSeconds(0.75f);
            Instantiate(playerController.explosion, pos, rot);
        }

        public void castCowboyAbility(int number, ref float lastTimeActivated)
        {
            // find key in activeCowboySkills that has value number
            ScriptableSkill skill = activeCowboySkills.GetSkill(number);

            // dont have skill
            if (skill == null) return;

            string skillName = skill.SkillName;
            int cooldown = skill.CoolDown;

            if (Time.time - lastTimeActivated < cooldown) return;

            lastTimeActivated = Time.time;

            // switch on the skill name and normalize it
            switch (skillName.ToLower().Replace(" ", ""))
            {
                case "dynamitedash":
                    StartCoroutine(dynamiteDash());
                    Debug.Log("DynoDashh");
                    break;
                case "goldengun":
                    Debug.Log("Golden Gun");
                    break;
            }
        }
        public void castWizardAbility(int number, ref float lastTimeActivated)
        {
            // find key in activeWizardSkills that has value 1
            ScriptableSkill skill = activeWizardSkills.GetSkill(number);

            // dont have skill
            if (skill == null) return;

            string skillName = skill.SkillName;
            int cooldown = skill.CoolDown;
            if (Time.time - lastTimeActivated < cooldown) return;
            lastTimeActivated = Time.time;

            // switch on the skill name and normalize it
            switch (skillName.ToLower().Replace(" ", ""))
            {
                case "shieldoffaith":
                    StartCoroutine(shieldOfFaith());
                    Debug.Log("Shield of Faith");
                    break;
                case "destructivewave":
                    Debug.Log("Destructive Wave");
                    break;
            }
        }
    }
}