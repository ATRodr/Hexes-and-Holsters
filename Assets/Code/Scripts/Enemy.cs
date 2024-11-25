using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isChasing = false;
    private bool hasLOS = false;
    public bool HasLOS => HasLOS;
    [SerializeField] Transform target;
    [SerializeField] public float health, maxHealth = 4f;
    [SerializeField] public float attackRate = 1f;
    [SerializeField] public bool isMelle = false;
    [SerializeField] public bool isMagic = false;
    
        
    [SerializeField]
    public int xpValue = 1;
    float nextAttack = 0f;

    public NavMeshAgent agent;
    private PlayerController playerController;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = GameObject.Find("REALPlayerPrefab").GetComponent<PlayerController>();
        target = GameObject.Find("REALPlayerPrefab").transform;
        health = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        //agent.updateUpAxis = false;
    }

    private void Update()
    {
        if (target == null)
        {
            target = GameObject.Find("REALPlayerPrefab").transform;
        }

        if (isChasing)
        {
            agent.SetDestination(target.position);
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
            isChasing = true;
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
            isChasing = false;
    }
    //Overloaded methods for giving damage to the enemy
    
    //Destroys GameObject that collides with the enemy
    public void TakeDamage(float damageAmt, GameObject Bullet){
        health -= damageAmt;
        Destroy(Bullet);

        if(health <= 0){
            givePoints(xpValue);
            Destroy(gameObject);
        }
    }
    
    //does not destroy GameObject that collides with the enemy
    public void TakeDamage(float damageAmt){
        health -= damageAmt;
        if(health <= 0){
            givePoints(xpValue);
            Destroy(gameObject);
        }
    }
    public void givePoints(int points){
        playerController.skillManager.GainSkillPoint(points);
    }
}
