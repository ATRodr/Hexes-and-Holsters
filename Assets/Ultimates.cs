using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Ultimates : MonoBehaviour
{
    private AimSystem aimSystem;
    [SerializeField] private AudioClip powerHealSound, destructiveWaveSound, dustStormSound, explodingBulletSound;
    [SerializeField]
    private Weapon weapon;
    private GameObject wave;
    private GameObject hadar;
    private GameObject bwSpin;
    private int cowboyUlt;
    private int wizardUlt;
    private float cowboyUltCooldown;
    private float wizardUltCooldown;
    public const float COWBOY_COOLDOWN = 3f;
    public const float WIZARD_COOLDOWN = 3f;
    public bool cowboyUltReady = false;
    public bool wizardUltReady = false;

    private PlayerHealth playerHealth;

    public void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        aimSystem = GetComponent<AimSystem>();
        cowboyUltCooldown = COWBOY_COOLDOWN;
        wizardUltCooldown = WIZARD_COOLDOWN;
        GameObject.FindObjectOfType<Weapon>();
        bwSpin = Resources.Load<GameObject>("BullWhipParent");
        hadar = Resources.Load<GameObject>("Hadar");
        wave = Resources.Load<GameObject>("DestructiveWave");
        if(hadar == null)
            Debug.LogError("hadar not found");
        if(wave == null)
            Debug.LogError("Wave not found");
        if(bwSpin == null)
            Debug.LogError("Bullwhip not found");
    }

    public void Update()
    {
        if (!cowboyUltReady)
        {
            cowboyUltCooldown -= Time.deltaTime;
        }
        if (!wizardUltReady)
        {
            wizardUltCooldown -= Time.deltaTime;
        }

        
        if(Input.GetKeyDown(KeyCode.Q))
        {
            // if is cowboy and ult ready or is wizard and ult ready
            if ((aimSystem.isCowboy && cowboyUltReady) || (!aimSystem.isCowboy && wizardUltReady))
            {
                // fire ult
                if (aimSystem.isCowboy)
                {
                    switch (cowboyUlt)
                    {
                        case 1:
                            Debug.Log("Exploding Bullets"); 
                            StartCoroutine(ExplodingBullets());
                            break;
                        case 2:
                            Debug.Log("Gatling Gun");
                            StartCoroutine(GatlingGun());
                            break;
                        case 3:
                            Debug.Log("Bullwhip Spin");
                            StartCoroutine(BullwhipSpin());
                            break;
                    }
                    cowboyUltReady = false;
                }
                else
                {
                    switch (wizardUlt)
                    {
                        case 1:
                            Debug.Log("Destructive Wave");
                            StartCoroutine(DestructiveWave());
                            break;
                        case 2:
                            Debug.Log("Power Word Heal");
                            StartCoroutine(PowerWordHeal());
                            break;
                        case 3:
                            Debug.Log("Hunger of Hadar");
                            StartCoroutine(HungerOfHadar());
                            break;
                    }
                    wizardUltReady = false;
                }
            }
            // if is cowboy and ult cooldown up or is wizard and ult cooldown up
            else if ((aimSystem.isCowboy && cowboyUltCooldown <= 0) || (!aimSystem.isCowboy && wizardUltCooldown <= 0))
            {
                // roll ult
                roll();
            }
        }
    }

    private void roll()
    {
        // roll ult

        // random number between 1 and 3
        int roll = Random.Range(1, 4);

        if (aimSystem.isCowboy)
        {
            cowboyUltReady = true;
            cowboyUltCooldown = COWBOY_COOLDOWN;
            cowboyUlt = roll;
            Debug.Log("Cowboy Ult: " + cowboyUlt);
        }
        else
        {
            wizardUltReady = true;
            wizardUltCooldown = WIZARD_COOLDOWN;
            wizardUlt = roll;
            Debug.Log("Wizard Ult: " + wizardUlt);
        }
    }
    IEnumerator PowerWordHeal()
    {
        // heal player
        if(Random.Range(1, 3) == 1)
            playerHealth.maxHealth += 1f;
        playerHealth.health = playerHealth.maxHealth;
        playerHealth.TakeDamage(0f, isRR: true);                  //this sucks but we must call it to update hearts.
        SoundManager.Instance.PlaySoundFXClip(powerHealSound, transform, 0.1f);
        yield return new WaitForSeconds(0.1f);
    }
    IEnumerator DestructiveWave()
        {
            Instantiate(wave, transform.position, transform.rotation);
            SoundManager.Instance.PlaySoundFXClip(destructiveWaveSound, transform, 0.1f);
            yield return new WaitForSeconds(0.1f);
        }
    IEnumerator GatlingGun()
    {
        // fire gatling gun
        Debug.Log("Gatling Gun Fired");
        for(int i = 0; i < 20;i++)
        {
            weapon.FireGat();
            yield return new WaitForSeconds(0.25f);
        }
    }
    IEnumerator HungerOfHadar()
    {
        Instantiate(hadar, transform.position, transform.rotation);
        yield return new WaitForSeconds(0.25f);
    }
    IEnumerator ExplodingBullets()
    {
        weapon.expodingBullets = true;
        SoundManager.Instance.PlaySoundFXClip(explodingBulletSound, transform, 0.1f);
        yield return new WaitForSeconds(10f);
        weapon.expodingBullets = false;
    }
    IEnumerator BullwhipSpin()
    {
        GameObject spin = Instantiate(bwSpin, transform.position, transform.rotation);
        spin.transform.SetParent(transform);
        playerHealth.isInvincible = true;
        SoundManager.Instance.PlaySoundFXClip(dustStormSound, transform, 0.08f);
        yield return new WaitForSeconds(10f);
        Destroy(spin);
        playerHealth.isInvincible = false;
    }
}
