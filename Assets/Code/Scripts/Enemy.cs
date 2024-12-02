using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    public bool isChasing = false;
    public bool isMoving = false; //movement for animation
    [SerializeField] public bool isGrillos;
    private bool hasLOS = false;
    public bool HasLOS => HasLOS;
    [SerializeField] GameObject target;
    [SerializeField] public float health, maxHealth = 4f;
    [SerializeField] public float attackRate = 1f;
    [SerializeField] public bool isMelle = false;
    [SerializeField] public bool isMagic = false;
    [SerializeField] public string uniqueID; // unique ID for every enemy
    public bool isTamed = false;
        
    float nextAttack = 0f;

    public NavMeshAgent agent;

    private void Start()
    {
        
        // Persistent Enemy Death Code
        // Create unique ID for enemy
        if (string.IsNullOrEmpty(uniqueID))
        {
            uniqueID = gameObject.scene.name + "_" + transform.position.ToString();
        }

        // Check if unique enemy is marked as dead
        if (PlayerPrefs.GetInt(uniqueID, 0) == 1)
        {
            Destroy(gameObject);
            return;
        }

        GetComponent<SpriteRenderer>().enabled = false;
        rb = GetComponent<Rigidbody2D>();
        
        target = GameObject.Find("REALPlayerPrefab");
        health = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        
        if(isTamed)
            target = FindSecondClosestEnemy();
        else    
            target = GameObject.Find("REALPlayerPrefab");
        if (target == null)
        {
            target = GameObject.Find("REALPlayerPrefab");
        }

        if (isChasing)
        {
            agent.SetDestination(target.transform.position);
        }

        Vector3 agentVelocity = agent.velocity; // NavMeshAgent velocity is in 3D
        if (agentVelocity.magnitude > 0.1f) // Adjust the threshold as needed
        {
            // Debug.Log("Enemy is moving");
            isMoving = true;
        }
        else
        {
            // Debug.Log("Enemy is stationary.");
            isMoving = false;
        }
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
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!isMelle || isTamed) return;
        
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
            //MarkAsKilled();
            
            Destroy(gameObject);
        }
    }
    
    //does not destroy GameObject that collides with the enemy
    public void TakeDamage(float damageAmt){
        health -= damageAmt;
        if(health <= 0){
            // This should be uncommented in final version
            //MarkAsKilled();
            Destroy(gameObject);
        }
    }
    private void MarkAsKilled()
    {
        // Using PlayerPrefs we mark enemy as dead
        PlayerPrefs.SetInt(uniqueID, 1);

        // This is so that we can delete only the persistent enemy data later when the game resets
        string enemyID = PlayerPrefs.GetString("EnemyID", "");
        if (!enemyID.Contains(uniqueID))
        {
            PlayerPrefs.SetString("EnemyID", enemyID + uniqueID + ";");
        }

        PlayerPrefs.Save();
    }
    
}
