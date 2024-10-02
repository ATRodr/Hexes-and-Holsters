using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public void onCollisionEnter2D(){
        Destroy(gameObject, 2f);
        //Check if hitting enemy
        //Do damage
        Console.WriteLine("Bullet hit something");
    }
}
