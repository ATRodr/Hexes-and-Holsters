using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.PlasticSCM.Editor.WebApi;
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
        private GameObject ShieldOfFaithParti;
        private GameObject RussianRouletteParti;
        private GameObject PolyBullet;
        // unlockable abilities
        private int dynamiteDashLevel, goldenGunLevel, shieldOfFaithLevel, russianRoulleteLevel, slowEnemyLevel, polyMorphLevel;
        private int skillPoints;
        private LRUCache activeCowboySkills;
        private LRUCache activeWizardSkills;        
        public int DynamiteDash => dynamiteDashLevel;
        public int GoldenGun => goldenGunLevel;
        public int ShieldOfFaith => shieldOfFaithLevel;
        public int russianRoulette => russianRoulleteLevel;
        public int PolyMorph => polyMorphLevel;
        public int SlowEnemy => slowEnemyLevel;
        public int SkillPoints => skillPoints;
        private PlayerHealth playerHealth;

        public UnityAction OnSkillPointsChanged;

        private List<ScriptableSkill> unlockedSkills = new List<ScriptableSkill>();
        
        //used for calling abilities and controlling player
        private PlayerController playerController;
        
        private void Start()
        {
            playerHealth = GetComponent<PlayerHealth>();
            playerController = GetComponent<PlayerController>();
            activeCowboySkills = new LRUCache();
            activeWizardSkills = new LRUCache();
            skillPoints = 100;
            dynamiteDashLevel = 0;
            goldenGunLevel = 0;
            shieldOfFaithLevel = 0;
            russianRoulleteLevel = 0;
            slowEnemyLevel = 0;
            polyMorphLevel = 0;
            Debug.Log($"PlayerSkillManager instance: {this.GetInstanceID()}");
            ShieldOfFaithParti = Resources.Load<GameObject>("ShieldOfFaithParti");
            RussianRouletteParti = Resources.Load<GameObject>("RussianRouletteParti");
            PolyBullet = Resources.Load<GameObject>("PolyBullet");
            if(ShieldOfFaithParti == null)
                Debug.LogError("ShieldOfFaithParti not found");
            if(RussianRouletteParti == null)
                Debug.LogError("RussianRouletteParti not found");
            if(PolyBullet == null)
                Debug.LogError("PolyBullet not found");
        }
        
        public void GainSkillPoint(int amount)
        {
            //shouldn't ever give negative skill points
            if(amount < 0) return; 

            skillPoints += amount;
            Debug.Log(amount + " of skill point gained from killing Enemy. Total skill points: " + skillPoints);
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
                    case StatTypes.dynamiteDash:
                        ModifyStat(ref dynamiteDashLevel, data);
                        break;
                    case StatTypes.goldenGun:
                        ModifyStat(ref goldenGunLevel, data);
                        break;
                    case StatTypes.shieldOfFaith:
                        ModifyStat(ref shieldOfFaithLevel, data);
                        break;
                    case StatTypes.russianRoulette:
                        ModifyStat(ref russianRoulleteLevel, data);
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
            playerController.weapon.setDamageMultiplier(4);
            Color originalColor = playerController.aimSystem.GoldenGun.GetComponent<SpriteRenderer>().color;
            yield return new WaitForSeconds(3f);

            for(int i = 0; i < 60; i++)
            {
                playerController.aimSystem.GoldenGun.GetComponent<SpriteRenderer>().color = Color.Lerp(originalColor, Color.black, Mathf.PingPong(Time.time * 2, 1));
                yield return new WaitForSeconds(0.05f);
            }
            playerController.aimSystem.GoldenGun.GetComponent<SpriteRenderer>().color = originalColor;
            playerController.weapon.setDamageMultiplier(4);
            playerController.aimSystem.goldenGunActive = false;
        }
        IEnumerator RussianRoulette()
        {
            if(UnityEngine.Random.Range(1, 3) == 1)
            {
                Debug.Log("HIT RR BAD NO GOOD");
                playerHealth.TakeDamage(1f);
                //find cam script and shake it. Yes this is messy but oh well
                 GameObject cameraObject = GameObject.FindGameObjectWithTag("MainCamera");

                if (cameraObject != null)
                {
                    // Get the CameraShake script attached to the camera
                    CameraFollow cameraShake = cameraObject.GetComponent<CameraFollow>();

                    if (cameraShake != null)
                    {
                        // Call the Shake function
                        StartCoroutine(cameraShake.Shake(0.2f, 1.5f)); // 0.2 seconds, 3 magnitude
                    }
                }
                else
                {
                    Debug.LogWarning("Main Camera not found!");
                }
            }
            else
            {
                playerController.weapon.setDamageMultiplier(2);
                playerHealth.isInvincible = true;
                GameObject rrEffect = Instantiate(RussianRouletteParti, transform.position, transform.rotation);
                rrEffect.transform.SetParent(transform);
                yield return new WaitForSeconds(5f);
                playerHealth.isInvincible = false;
                Destroy(rrEffect); // Destroys the particle effect after wait finished  
                playerController.weapon.setDamageMultiplier(1);
            }    
        }
        IEnumerator slowEnemy()
        {
            //find list of enemies and slow them down
            Enemy[] enemies  = FindObjectsOfType<Enemy>();
            float[] originalSpeeds = new float[enemies.Length];
            foreach (Enemy enemy in enemies)
            {
                //find index of enemy and save that speed at that index so we can reset it back to normal after ability is done
                originalSpeeds[Array.IndexOf(enemies, enemy)] = enemy.agent.speed;
                //find nav mesh agent set speed to lower, change it back after.
                enemy.agent.speed = 1;
                enemy.attackRate = 2; //slow down attack by a second
            }
            yield return new WaitForSeconds(5); //wait some time then unslow them
            //reset all the speeds back to normal
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].agent.speed = 5;
                enemies[i].attackRate = 1; //reset attack rate
            }


        }
        void polyMorph()
        {
            //FirePolyBullet. Maybe add heat seeking if possible
            GameObject PolyShot = Instantiate(PolyBullet, transform.position, transform.rotation);
            PolyShot.GetComponent<Rigidbody2D>().AddForce(playerController.weapon.orbFirePoint.right * 10, ForceMode2D.Impulse);
        }
        IEnumerator shieldOfFaith()
        {
            playerController.playerHealth.isInvincible = true;
            playerController.healthBar.DrawHearts();
            GameObject shieldEffect = Instantiate(ShieldOfFaithParti, transform.position, transform.rotation);
            shieldEffect.transform.SetParent(transform);
            yield return new WaitForSeconds(5);
            Destroy(shieldEffect); // Destroys the particle effect after waut finished
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

            Debug.Log("Skill Name: " + skillName.ToLower().Replace(" ", ""));
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
                case"russianroullete":
                    Debug.Log("Russian Roulette FOUND");
                    StartCoroutine(RussianRoulette());
                    Debug.Log("Russian Roulette");
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
                    //StartCoroutine(shieldOfFaith()); 
                    polyMorph(); //remove and uncomment, delete, temporary testing
                    Debug.Log("Shield of Faith");
                    break;
            }
        }
    }
}