using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private AudioClip goldenGunSound, bulletFired, fireBoltFired;
    private bool isCowboy;
    private AimSystem aimSystem;

    public GameObject bulletPrefab;
    public GameObject fireBoltPrefab;

    public Transform gunFirePoint;
    public Transform orbFirePoint;

    public float fireForce = 30f;
    public bool isPolyShot = false;
    public bool expodingBullets = false;


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
                SoundManager.Instance.PlaySoundFXClip(goldenGunSound, transform, 0.3f);
            }
            else
            {
                bulletPrefab.GetComponent<SpriteRenderer>().color = Color.red;
                bullet = Instantiate(bulletPrefab, gunFirePoint.position, gunFirePoint.rotation);
                bullet.GetComponent<Rigidbody2D>().AddForce(gunFirePoint.right * fireForce, ForceMode2D.Impulse);
                SoundManager.Instance.PlaySoundFXClip(bulletFired, transform, 0.3f);
            }
            if(expodingBullets)
            {
                bullet.GetComponent<Bullet>().explodeOnImpact = true;
            }else
            {
                bullet.GetComponent<Bullet>().explodeOnImpact = false;
            }
            bullet.GetComponent<Rigidbody2D>().AddForce(gunFirePoint.right * fireForce, ForceMode2D.Impulse);
        }
        else
        {
            bullet = Instantiate(fireBoltPrefab, orbFirePoint.position, orbFirePoint.rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(orbFirePoint.right * fireForce, ForceMode2D.Impulse);
            SoundManager.Instance.PlaySoundFXClip(fireBoltFired, transform, 0.3f);
        }
        Destroy(bullet, 5f);
    }
    public void FireGat()
    {
        // Offsets for bullet positions, relative to the gunFirePoint's local space
        Vector3 offset1 = gunFirePoint.TransformDirection(new Vector3(-0.3f, 0.2f, 0f));
        Vector3 offset2 = gunFirePoint.TransformDirection(new Vector3(-0.3f, -0.2f, 0f));

        // Instantiate bullets at their respective positions
        GameObject bullet1 = Instantiate(bulletPrefab, gunFirePoint.position + offset1, gunFirePoint.rotation);
        GameObject bullet2 = Instantiate(bulletPrefab, gunFirePoint.position, gunFirePoint.rotation);
        GameObject bullet3 = Instantiate(bulletPrefab, gunFirePoint.position + offset2, gunFirePoint.rotation);

        // Calculate the firing direction
        Vector2 fireDirection = gunFirePoint.right.normalized; // Ensure the direction is normalized

        // Apply force to bullets
        bullet1.GetComponent<Rigidbody2D>().AddForce((fireDirection + (Vector2)offset1).normalized * fireForce, ForceMode2D.Impulse);
        bullet2.GetComponent<Rigidbody2D>().AddForce(fireDirection * fireForce, ForceMode2D.Impulse);
        bullet3.GetComponent<Rigidbody2D>().AddForce((fireDirection + (Vector2)offset2).normalized * fireForce, ForceMode2D.Impulse);
    }
    public void setDamageMultiplier(int damageMultiplier){
        Debug.Log("Setting damage multiplier to: " + damageMultiplier);
        bulletPrefab.GetComponent<Bullet>().damageMultiplier = damageMultiplier;
    }
}

