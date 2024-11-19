using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private bool isCowboy;
    private AimSystem aimSystem;

    public GameObject bulletPrefab;
    public GameObject fireBoltPrefab;

    public Transform gunFirePoint;
    public Transform orbFirePoint;

    public float fireForce = 30f;


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
            if(aimSystem.goldenGunActive)
            {
                bulletPrefab.GetComponent<SpriteRenderer>().color = Color.yellow;
                bullet = Instantiate(bulletPrefab, gunFirePoint.position, gunFirePoint.rotation);
                bullet.GetComponent<Rigidbody2D>().AddForce(gunFirePoint.right * fireForce, ForceMode2D.Impulse);
            }else{
                bulletPrefab.GetComponent<SpriteRenderer>().color = Color.gray;
                bullet = Instantiate(bulletPrefab, gunFirePoint.position, gunFirePoint.rotation);
                bullet.GetComponent<Rigidbody2D>().AddForce(gunFirePoint.right * fireForce, ForceMode2D.Impulse);
            }
        }
        else
        {
            bullet = Instantiate(fireBoltPrefab, orbFirePoint.position, orbFirePoint.rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(orbFirePoint.right * fireForce, ForceMode2D.Impulse);
        }
        Destroy(bullet, 5f);
    }
}

