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
