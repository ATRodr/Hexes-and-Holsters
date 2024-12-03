using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{
    
    private PlayerController playerController;

    private float lastAbilityActivationTime = 0f;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        // if (Time.time - lastAbilityActivationTime < cooldown) return;

    }
    
}
