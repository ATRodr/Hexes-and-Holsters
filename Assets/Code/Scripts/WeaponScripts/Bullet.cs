using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool isFireBolt = false;
    // Boolean to check if it's a chain lightning bolt
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Colliding");
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Enemy enemyComponent = collision.gameObject.GetComponent<Enemy>();
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
        Destroy(gameObject); // Destroy bullet after collision
    }
}