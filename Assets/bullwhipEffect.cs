using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullwhipEffect : MonoBehaviour
{
    //damage per second cause its easier to understand for the setting of the damage
    public float damagePerSecond;

    //will be calculated based on the damage per second
    private float damagePerFrame; 
    // Start is called before the first frame update
    void Start()
    {
        setDamagePerSecond(3f);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<Enemy>(out Enemy enemyComponent))
        {
            float disFromCenter = (enemyComponent.transform.position - transform.position).magnitude;
            if(disFromCenter > 4)
                return;
            //Debug.Log("within Bull Whip");
            if(enemyComponent.agent.isOnNavMesh)
                enemyComponent.agent.speed /= 3;
            enemyComponent.TakeDamage(damagePerFrame); //change back to damagePerFrame after testing
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<Enemy>(out Enemy enemyComponent))
        {
            if(enemyComponent != null && enemyComponent.agent.isOnNavMesh)
            {
                enemyComponent.agent.speed *= 3;

            }
        }
    }
    public void setDamagePerSecond(float damage)
    {
        //set damage
        damagePerSecond = damage;

        //calculate damage per frame
        damagePerFrame = damagePerSecond / 50;

    }
}
