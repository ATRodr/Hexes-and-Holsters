using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float health, maxHealth = 4f;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerStats>().TakeDamage(1);
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
