using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject boltPrefab;
    public Transform firePoint;
    public float fireForce = 30f;

    public ChainLightningScript chainLightningEffect;

    public void Fire(){
        
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
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

