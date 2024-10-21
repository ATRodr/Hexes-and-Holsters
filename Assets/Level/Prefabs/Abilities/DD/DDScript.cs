using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DDScript : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 0.8f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Colliding");
        if(collision.gameObject.TryGetComponent<Enemy>(out Enemy enemyComponent))
        {
            Debug.Log("Hit enemy");
            enemyComponent.TakeDamage(3, gameObject);

        }
    }
}
