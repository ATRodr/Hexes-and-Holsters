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
        Debug.Log(isCowboy);
    }
    public void Fire(){
        GameObject bullet = null;
        if (isCowboy)
        {
            bullet = Instantiate(bulletPrefab, gunFirePoint.position, gunFirePoint.rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(gunFirePoint.right * fireForce, ForceMode2D.Impulse);
        }
        else
        {
            Debug.Log("FIREBOLT in Gun.cs");
            bullet = Instantiate(fireBoltPrefab, orbFirePoint.position, orbFirePoint.rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(orbFirePoint.right * fireForce, ForceMode2D.Impulse);
        }
        Destroy(bullet, 5f);
    }
}

