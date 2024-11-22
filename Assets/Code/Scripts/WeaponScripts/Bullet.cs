using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    public bool isFireBolt = false;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
<<<<<<< HEAD
            
            Enemy enemyComponent = collision.gameObject.GetComponent<Enemy>();
            if (enemyComponent.isMagic && !isFireBolt || isFireBolt && !enemyComponent.isMagic)
=======
            if(collision.gameObject.TryGetComponent<Enemy>(out Enemy enemyComponent))
>>>>>>> 925b08e8ea967518042aac7cb88ef4f207a4d6c8
            {
                if (enemyComponent.isMagic && !isFireBolt || isFireBolt && !enemyComponent.isMagic)
                {
                    // if magic enemy is being hit by bullet take full damage
                    enemyComponent.TakeDamage(1, gameObject);
                }
                else
                {
                    // if non magic enemy hit by bullet, take half damage
                    enemyComponent.TakeDamage(0.5f, gameObject);
                }
            }
        }
        Destroy(gameObject); // Destroy bullet after collision
    }
}