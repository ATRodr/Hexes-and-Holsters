using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    public static event Action OnPlayerDamaged;
    public static event Action OnPlayerDeath;
    public float health, maxHealth;
    public bool isInvincible = false;

    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float amt){
        if(isInvincible){
            return;
        }
        health -= amt;
        OnPlayerDamaged?.Invoke();

        if(health <= 0){
            health = 0;
            // Debug.Log("You Ded");
            OnPlayerDeath?.Invoke();
        }
    }
}
