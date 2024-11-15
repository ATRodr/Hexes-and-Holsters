using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Ultimates : MonoBehaviour
{
    private AimSystem aimSystem;
    private int cowboyUlt;
    private int wizardUlt;
    private float cowboyUltCooldown;
    private float wizardUltCooldown;
    public const float COWBOY_COOLDOWN = 3f;
    public const float WIZARD_COOLDOWN = 3f;
    public bool cowboyUltReady = false;
    public bool wizardUltReady = false;

    public IEnumerator Start()
    {
        while (MainManager.Instance == null || MainManager.Instance.aimSystem == null)
        {
            yield return null;
        }

        aimSystem = MainManager.Instance.aimSystem;
        cowboyUltCooldown = COWBOY_COOLDOWN;
        wizardUltCooldown = WIZARD_COOLDOWN;
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
}
