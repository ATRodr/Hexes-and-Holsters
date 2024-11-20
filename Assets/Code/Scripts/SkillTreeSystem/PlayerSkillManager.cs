using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
                    Debug.Log("Returning: " + skill.SkillName);
                    return skill;
                }
            }
            return null;
        }
    }
    public class PlayerSkillManager : MonoBehaviour
    {
        // Start is called before the first frame update

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
        
        private void Start()
        {
            playerController = GetComponent<PlayerController>();
            activeCowboySkills = new LRUCache();
            activeWizardSkills = new LRUCache();
            skillPoints = 10;
            chainLightningLevel = 0;
            destructiveWaveLevel = 0;
            dynamiteDashLevel = 0;
            goldenGunLevel = 0;
            shieldOfFaithLevel = 0;
            Debug.Log($"PlayerSkillManager instance: {this.GetInstanceID()}");

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
            if (skill.isCowboySkill)
                activeCowboySkills.Add(skill);
            else
                activeWizardSkills.Add(skill);            
        }
        IEnumerator ActivateGoldenGun()
        {
            playerController.aimSystem.goldenGunActive = true;
            Color originalColor = playerController.aimSystem.GoldenGun.GetComponent<SpriteRenderer>().color;
            yield return new WaitForSeconds(3f);

            for(int i = 0; i < 60; i++)
            {
                playerController.aimSystem.GoldenGun.GetComponent<SpriteRenderer>().color = Color.Lerp(originalColor, Color.black, Mathf.PingPong(Time.time * 2, 1));
                yield return new WaitForSeconds(0.05f);
            }
            playerController.aimSystem.GoldenGun.GetComponent<SpriteRenderer>().color = originalColor;
            playerController.aimSystem.goldenGunActive = false;
        }
        IEnumerator shieldOfFaith()
        {
            playerController.playerHealth.isInvincible = true;
            playerController.healthBar.DrawHearts();
            yield return new WaitForSeconds(5);
            playerController.playerHealth.isInvincible = false;
            playerController.healthBar.DrawHearts();
        }
        IEnumerator dynamiteDash()
        {
            Vector2 pos = transform.position;
            Quaternion rot = transform.rotation;
            Debug.Log("Dynamite Dash");
            Instantiate(playerController.dynamite, pos, rot);
            StartCoroutine(playerController.Dash(0.16f, 27f));
            yield return new WaitForSeconds(0.5f);
            Instantiate(playerController.explosion, pos, rot);
        }

        public void castCowboyAbility(int number, ref float lastTimeActivated)
        {
            // find key in activeCowboySkills that has value number
            ScriptableSkill skill = activeCowboySkills.GetSkill(number);
            // dont have skill
            if (skill == null) 
            {
                Debug.Log("dont have skill");
                return;
            };

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
                    StartCoroutine(ActivateGoldenGun());
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
            }
        }
    }
}