using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float health, maxHealth = 4f;
    [SerializeField] float attackRate = 1f;
    [SerializeField] bool isMelle = true;
    
    float nextAttack = 0f;

    NavMeshAgent agent;

    private void Start()
    {
        health = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        //agent.updateUpAxis = false;
    }

    private void Update()
    {
        agent.SetDestination(target.position);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!isMelle) return;
        
        if(collision.gameObject.CompareTag("Player") && Time.time > nextAttack)
        {
            nextAttack = Time.time + attackRate;
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
        }
    }

    public void TakeDamage(float damageAmt, GameObject Bullet){
        health -= damageAmt;
        Destroy(Bullet);

        if(health <= 0){
            Destroy(gameObject);
        }
    }
    
    public void TakeDamage(float damageAmt){
        health -= damageAmt;
        if(health <= 0){
            Destroy(gameObject);
        }
    }
    
}
