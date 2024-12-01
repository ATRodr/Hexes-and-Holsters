using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bullet : MonoBehaviour
{
    
    public bool isFireBolt = false;
    public int damageMultiplier = 1;    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if(collision.gameObject.TryGetComponent<Enemy>(out Enemy enemyComponent))
            {
                Debug.Log("Damage Multiplier: " + damageMultiplier);
                if (enemyComponent.isMagic && !isFireBolt || isFireBolt && !enemyComponent.isMagic)
                {
                    // if magic enemy is being hit by bullet take full damage
                    enemyComponent.TakeDamage(1 * damageMultiplier, gameObject);
                }
                else
                {
                    // if non magic enemy hit by bullet, take half damage
                    enemyComponent.TakeDamage(0.5f * damageMultiplier, gameObject);
                }
            }
        }
        Destroy(gameObject); // Destroy bullet after collision
    }
}