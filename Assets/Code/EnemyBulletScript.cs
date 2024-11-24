using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public float force = 20f;
    public bool shootAtPlayer = true; // Determines if the bullet should track the player

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (shootAtPlayer)
        {
            FireAtPlayer();
        }
        
        Destroy(gameObject, 2f);
    }

    void FireAtPlayer()
    {
        // Find the player object and set velocity toward the player's position
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 direction = player.transform.position - transform.position;
            rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the bullet collides with the player, apply damage and destroy the bullet
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
        }
        Destroy(gameObject);
    }
}
