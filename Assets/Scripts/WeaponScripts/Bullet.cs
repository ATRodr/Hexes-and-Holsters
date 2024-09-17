using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void onCollisionEnter2D(Collision2D collision){
        Destroy(gameObject);
        //Check if hitting enemy
        //Do damage
    }
}
