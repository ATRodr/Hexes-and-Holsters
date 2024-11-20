using UnityEngine;
using System.Collections;

public class CircleColliderGrow : MonoBehaviour
{
    public float growSpeed = 1f; // Speed at which the collider grows
    public float maxRadius = 5f; // Maximum radius of the collider

    private CircleCollider2D circleCollider;

    void Start()
    {
        // Get the CircleCollider2D component
        circleCollider = GetComponent<CircleCollider2D>();
        Destroy(gameObject, 1f);
    }

    void Update()
    {
        // Grow the radius over time
        if (circleCollider.radius < maxRadius)
        {
            circleCollider.radius += 15 * Time.deltaTime;
            Physics2D.SyncTransforms();
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<Enemy>(out Enemy enemyComponent))
        {
            enemyComponent.TakeDamage(3f);

            //this conditional sucks but its to make sure enemy is alive since it is destroyed in TakeDamage if dead
            if(enemyComponent != null)
            {
                //check if navmesh agent is placed on navmesh during knockback to avoid errors
                if(enemyComponent.agent.isOnNavMesh)
                    enemyComponent.agent.isStopped = true;

                // knockback stuff. Duck tape because I'm lazy and it works
                Vector2 pushDirection = ((Vector2)collision.transform.position - (Vector2)transform.position).normalized;
                StartCoroutine(KnockbackCoroutine(collision.transform, pushDirection, 3.7f, 0.4f));
                
                if(enemyComponent.agent.isOnNavMesh)
                    enemyComponent.agent.isStopped = false;
            }
        }
    }
    private IEnumerator KnockbackCoroutine(Transform enemy, Vector2 direction, float knockbackDistance, float duration)
    {
        // Store the start and target positions
        Vector2 startPosition = enemy.position;
        Vector2 targetPosition = startPosition + direction * knockbackDistance;


        float elapsedTime = 0f;

        // Move the enemy towards the end outside of the explosion area from curreent position in explosion smoothly 
        while (elapsedTime < duration)
        {
            if(enemy == null)
            {
                yield break;
            }
            enemy.position = Vector2.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        enemy.position = targetPosition; // Ensure final position is exact
        }
}