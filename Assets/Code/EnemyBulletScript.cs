using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    public bool isGrillos = false;
    private Rigidbody2D rb;
    public float force = 20f;
    public bool shootAtPlayer = true; // Determines if the bullet should track the player

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if(!isGrillos){
            if (shootAtPlayer)
            {
                FireAtPlayer();
            }if(!shootAtPlayer){
                FireAtTarget();
            }
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
    void FireAtTarget(){
        //find the closest enemy and set velocity toward the enemy's position
        GameObject target = FindSecondClosestEnemy();
        if (target != null)
        {
            //change bullet to be on player layer so it can collide with enemy
            //need to wait a few ms to change layer so it doesn't collide with itself since nullet originates 0,0 wrt the enmy firing 
            //Yes this sucks but it will work for now
            StartCoroutine(ChangeLayer());
            Vector3 direction = target.transform.position - transform.position;
            rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
        }
    }
    IEnumerator ChangeLayer(){
        yield return new WaitForSeconds(0.05f);
        gameObject.layer = LayerMask.NameToLayer("Player Bullets");
    }
    GameObject FindSecondClosestEnemy(){
        GameObject[] enemies;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject enemy in enemies){
            //if the enemy is the one shooting the bullet, skip it. Or the closest enemy to the bullet will be enemy shooting it
            if(enemy.transform.position == position){
                continue;
            }
            Vector3 diff = enemy.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance){
                closest = enemy;
                distance = curDistance;
            }
        }
        return closest;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the bullet collides with the player, apply damage and destroy the bullet
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
        }
        if(collision.gameObject.TryGetComponent<Enemy>(out Enemy enemyComponent) && !shootAtPlayer){
            enemyComponent.TakeDamage(1 , gameObject);
        }
        Destroy(gameObject);
    }
}
