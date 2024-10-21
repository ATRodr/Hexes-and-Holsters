using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    // Boolean to check if it's a chain lightning bolt
    public bool isChainLightning = false;
    public GameObject chainLightningEffect;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Colliding");
        if(collision.gameObject.TryGetComponent<Enemy>(out Enemy enemyComponent))
        {
            Debug.Log("Hit enemy");
            if (isChainLightning)
            {
                Debug.Log($"Hit: {collision.gameObject.name}, Layer: {collision.gameObject.layer}");
                enemyComponent.TakeDamage(5, gameObject);
                Instantiate(chainLightningEffect, collision.collider.transform.position, Quaternion.identity);
            }
            else
                enemyComponent.TakeDamage(1, gameObject);

        }

        Destroy(gameObject); // Destroy bullet after collision
    }
}