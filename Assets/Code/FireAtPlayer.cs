using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAtPlayer : MonoBehaviour
{
    [SerializeField] public float fireRate;
    public GameObject bullet;
    public Transform bulletPos;
    public bool firingEnabled = true;

    private float timer;
    private GameObject player;
    public bool hasLOS = false;
    private int PlayerLayer; //not hardcoded for Wizard ult so we can switch which enemies the enemy shoots.
    private int EnemyLayer; //needed in future to implement wizard ult(enemy shot should witch layers and shoot other enemies)
    private int ForegroundLayer;
    public bool isTamed = false;
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
        //transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        

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
                if (hasLOS && firingEnabled)
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
        //should shoot at player if Boolean(shootAtPlayer) in EnemyBulletScript is true. Else, shoot at closest enemy.
        GameObject bulletInstance = Instantiate(bullet, bulletPos.position, Quaternion.identity);
        if(isTamed)
            bulletInstance.GetComponent<EnemyBulletScript>().shootAtPlayer = false;
        else
            bulletInstance.GetComponent<EnemyBulletScript>().shootAtPlayer = true;
    }   
}
