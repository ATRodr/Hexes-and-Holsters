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
    void Start()
    {
        player  = GameObject.FindGameObjectWithTag("Player");

        PlayerLayer = LayerMask.NameToLayer("Player");
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
            RaycastHit2D ray = Physics2D.Raycast(transform.position, player.transform.position - transform.position, distance:Mathf.Infinity, 1 << PlayerLayer);
            if (ray.collider != null)
            {
                //Debug.Log("Ray collider not null");
                hasLOS = ray.collider.CompareTag("Player");
                if (hasLOS)
                {
                    timer = 0;
                    shoot();
                }
                else
                {
                    Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);
                }
            }
            else
            {
                // Debug.Log("raycast null");
            }
        } 
    }

    void shoot()
    {
        Instantiate(bullet, bulletPos.position, Quaternion.identity);
    }
}
