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
                bulletPrefab.GetComponent<SpriteRenderer>().color = Color.red;
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
    public void FireGat(){
        GameObject bullet1 = null;
        GameObject bullet2 = null;
        GameObject bullet3 = null;

        Vector3 offset1 = new Vector3(-0.3f, 0.2f, 0f);
        Vector3 offset2 = new Vector3(-0.3f, -0.2f, 0f);
        Quaternion rotationQ = new Quaternion(20, 0, 0, 0);

        bullet1 = Instantiate(bulletPrefab, gunFirePoint.position + offset1, gunFirePoint.rotation);
        bullet2 = Instantiate(bulletPrefab, gunFirePoint.position, gunFirePoint.rotation);
        bullet3 = Instantiate(bulletPrefab, gunFirePoint.position + offset2, gunFirePoint.rotation);

        bullet1.GetComponent<Rigidbody2D>().AddForce((gunFirePoint.right + offset1) * 5, ForceMode2D.Impulse);
        bullet2.GetComponent<Rigidbody2D>().AddForce(gunFirePoint.right * 5, ForceMode2D.Impulse);
        bullet3.GetComponent<Rigidbody2D>().AddForce((gunFirePoint.right + offset2) * 5 + offset2, ForceMode2D.Impulse);
    }
    public void setDamageMultiplier(int damageMultiplier){
        Debug.Log("Setting damage multiplier to: " + damageMultiplier);
        bulletPrefab.GetComponent<Bullet>().damageMultiplier = damageMultiplier;
    }
}

