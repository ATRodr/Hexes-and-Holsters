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
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemyComponent))
        {
            if (isChainLightning)
            {
                Debug.Log($"Hit: {collision.gameObject.name}, Layer: {collision.gameObject.layer}");
                enemyComponent.TakeDamage(0, gameObject);
                Instantiate(chainLightningEffect, collision.collider.transform.position, Quaternion.identity);
            }
            else
                enemyComponent.TakeDamage(1, gameObject);

        }

        Destroy(gameObject); // Destroy bullet after collision
    }
}