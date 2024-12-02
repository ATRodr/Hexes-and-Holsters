using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class Bullet : MonoBehaviour
{
    [SerializeField] private AudioClip correctBulletHitSound;
    [SerializeField] private AudioClip incorrectBulletHitSound;
    public bool isFireBolt = false;
    public int damageMultiplier = 1;    
    public bool explodeOnImpact = false;
    private bool secondGen = false;
    public GameObject bulletPrefab;
    
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Collision detected. ExplodeOnImpact: {explodeOnImpact}");

        if (!isFireBolt && explodeOnImpact && !secondGen)
        {
            Vector2 impactPoint = transform.position;
            for (int i = 0; i < 8; i++)
            {
                float angle = i * 45f;
                Quaternion rotation = Quaternion.Euler(0, 0, angle);
                Vector2 direction = rotation * Vector2.right;

                GameObject newBullet = Instantiate(bulletPrefab, impactPoint, rotation);
                Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();
                rb.AddForce(direction * 30, ForceMode2D.Impulse);

                // Prevent new bullets from exploding
                newBullet.GetComponent<Bullet>().secondGen = true;
                Debug.Log("Instantiated new bullet with explodeOnImpact set to false.");
            }
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if(collision.gameObject.TryGetComponent<Enemy>(out Enemy enemyComponent))
            {
                Debug.Log("Damage Multiplier: " + damageMultiplier);
                if (enemyComponent.isMagic && !isFireBolt || isFireBolt && !enemyComponent.isMagic)
                {
                    SoundManager.Instance.PlaySoundFXClip(correctBulletHitSound, transform, 0.3f);
                    // if magic enemy is being hit by bullet take full damage
                    enemyComponent.TakeDamage(1 * damageMultiplier);
                }
                else
                {
                    SoundManager.Instance.PlaySoundFXClip(incorrectBulletHitSound, transform, 0.3f);
                    // if non magic enemy hit by bullet, take half damage
                    enemyComponent.TakeDamage(0.5f * damageMultiplier);
                }
            }
        }
        //wierd fix set these to be unactive so they don't linger
        if(!secondGen)
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject, 0.3f); // Destroy bullet after collision
    }
}