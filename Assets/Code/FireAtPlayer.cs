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

    void Start()
    {
        player  = GameObject.FindGameObjectWithTag("Player");
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
            RaycastHit2D ray = Physics2D.Raycast(transform.position, player.transform.position - transform.position, distance:Mathf.Infinity, layerMask:1);
            if (ray.collider != null)
            {
                hasLOS = ray.collider.CompareTag("Player");
                if (hasLOS)
                {
                    Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
                    Debug.Log("Player in sight");
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
                Debug.Log("raycast null");
            }
        }
    }

    void shoot()
    {
        Instantiate(bullet, bulletPos.position, Quaternion.identity);
    }
}
