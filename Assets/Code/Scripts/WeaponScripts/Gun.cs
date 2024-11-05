using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private bool isCowboy;
    private AimSystem aimSystem;
    public GameObject bulletPrefab;
    public GameObject fireBoltPrefab;
    public GameObject boltPrefab;
    public Transform firePoint;
    public float fireForce = 30f;

    public ChainLightningScript chainLightningEffect;

    private void Start()
    {
        aimSystem = GetComponentInParent<AimSystem>();
    }
    private void Update()
    {
        if (aimSystem is null)
        {
            Debug.Log("AimSystem is null");
        }
        else
            isCowboy = aimSystem.isCowboy;
    }
    public void Fire(){
        
        GameObject bullet = null;
        if (isCowboy)
        {
            bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
        else
        {
            bullet = Instantiate(fireBoltPrefab, firePoint.position, firePoint.rotation);
        }
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.right * fireForce, ForceMode2D.Impulse);
        Destroy(bullet, 5f);
    }
     public void FireLightning(){
        
        GameObject bullet = Instantiate(boltPrefab, firePoint.position, firePoint.rotation);

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.isChainLightning = true;
        
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.right * fireForce, ForceMode2D.Impulse);


        //Destroy(bullet, 5f);
    }


}

