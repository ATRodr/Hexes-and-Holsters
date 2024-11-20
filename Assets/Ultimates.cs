using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Ultimates : MonoBehaviour
{
    private AimSystem aimSystem;
    [SerializeField]
    private Weapon weapon;
    private GameObject wave;
    private int cowboyUlt;
    private int wizardUlt;
    private float cowboyUltCooldown;
    private float wizardUltCooldown;
    public const float COWBOY_COOLDOWN = 3f;
    public const float WIZARD_COOLDOWN = 0f; //CHANGE BACK TO WHATEVER IT WAS
    public bool cowboyUltReady = false;
    public bool wizardUltReady = false;

    public void Start()
    {
        wave = Resources.Load<GameObject>("DestructiveWave");
        aimSystem = GetComponent<AimSystem>();
        cowboyUltCooldown = COWBOY_COOLDOWN;
        wizardUltCooldown = WIZARD_COOLDOWN;
        GameObject.FindObjectOfType<Weapon>();
        if(wave == null)
            Debug.LogError("Wave not found");
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
                            Debug.Log("Golden Gun");
                            break;
                        case 2:
                            Debug.Log("Gatling Gun");
                            StartCoroutine(GatlingGun());
                            break;
                        case 3:
                            Debug.Log("Bullwhip Spin");
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
                            break;
                        case 3:
                            Debug.Log("Hunger of Hadar");
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

        // random number between 1 and 20
        int roll = Random.Range(1, 3);

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
    IEnumerator DestructiveWave()
        {
            Instantiate(wave, transform.position, transform.rotation);
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
}
