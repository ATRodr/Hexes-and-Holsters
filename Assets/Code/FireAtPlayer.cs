using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAtPlayer : MonoBehaviour
{
    [SerializeField] private float fireRate;
    public GameObject bullet;
    public Transform bulletPos;
    private float timer;
    private GameObject player;
    private bool hasLOS = false;

    private int PlayerLayer; //not hardcoded for Wizard ult so we can switch which enemies the enemy shoots.
    private int EnemyLayer; //needed in future to implement wizard ult(enemy shot should witch layers and shoot other enemies)
    private int ForegroundLayer;
    
    void Start()
    {
        player  = GameObject.FindGameObjectWithTag("Player");
        PlayerLayer = LayerMask.GetMask("Player");
        ForegroundLayer = LayerMask.GetMask("Foreground");
        EnemyLayer = LayerMask.NameToLayer("Enemy");
    }

    void Update()
    {
        Vector3 difference = transform.position - player.transform.position;
        difference.Normalize();
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        

        timer += Time.deltaTime;
        if (timer >= 1/fireRate)
        {
            // see if enemy has LOS of player
            RaycastHit2D ray = Physics2D.Raycast(
                transform.position,
                player.transform.position - transform.position,
                Mathf.Infinity,
                PlayerLayer | ForegroundLayer
            );
            if (ray.collider != null)
            {
                hasLOS = ray.collider.CompareTag("Player");
                if (hasLOS)
                {
                    Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);

                    timer = 0;
                    shoot();
                }
                else
                {
                    Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);
                }
            }
        } 
    }

    void shoot()
    {
        Instantiate(bullet, bulletPos.position, Quaternion.identity);
    }
}
