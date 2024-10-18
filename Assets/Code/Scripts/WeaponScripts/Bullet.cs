using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void start()
    {
        Debug.Log("Destroy");
        Destroy(gameObject, .4f);
    }
    private void OnCollisionEnter2D(Collision2D collision){

        if(collision.gameObject.TryGetComponent<Enemy>(out Enemy enemyComponent)){
            enemyComponent.TakeDamage(1, gameObject);
        }
        Destroy(gameObject);
       
        
    }
}
