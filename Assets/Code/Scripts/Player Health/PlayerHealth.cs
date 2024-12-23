using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;  
using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    private AudioClip cowboyHurtSound, wizardHurtSound, playerDeathSound;
    private AimSystem aimSystem;
    public static event Action OnPlayerDamaged;
    public static event Action OnPlayerDeath;
    [SerializeField]public float health, maxHealth;
    public bool isInvincible = false;

    private void Start()
    {
        health = maxHealth;
        aimSystem = GameObject.Find("REALPlayerPrefab").GetComponent<AimSystem>();
        cowboyHurtSound = Resources.Load<AudioClip>("CowboyHurtGavin");
        wizardHurtSound = Resources.Load<AudioClip>("WizardHurtMo");
        playerDeathSound = Resources.Load<AudioClip>("Die");
    }

    public void TakeDamage(float amt, bool isRR=false){
        if(isInvincible){
            return;
        }
        health -= amt;
        if (!isRR)
        {
            if (aimSystem.isCowboy)
            {
                SoundManager.Instance.PlaySoundFXClip(cowboyHurtSound, transform, 0.3f);
            }
            else
            {
                SoundManager.Instance.PlaySoundFXClip(wizardHurtSound, transform, 0.3f);
            }
        }

        OnPlayerDamaged?.Invoke();

        if(health <= 0){
            health = 0;
            SoundManager.Instance.PlaySoundFXClip(playerDeathSound, transform, 0.3f);
            // Debug.Log("You Ded");
            SceneManager.LoadScene("Main Menu");
            Destroy(GameObject.Find("REALPlayerPrefab"));
            Destroy(GameObject.Find("UI and Health"));
            Destroy(GameObject.Find("Music"));
        }
    }
}
