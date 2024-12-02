using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bullet : MonoBehaviour
{
    [SerializeField] private AudioClip correctBulletHitSound;
    [SerializeField] private AudioClip incorrectBulletHitSound;
    public bool isFireBolt = false;
    public int damageMultiplier = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if(collision.gameObject.TryGetComponent<Enemy>(out Enemy enemyComponent))
            {
                Debug.Log("Damage Multiplier: " + damageMultiplier);
                if (enemyComponent.isMagic && !isFireBolt || isFireBolt && !enemyComponent.isMagic)
                {
                    SoundManager.Instance.PlaySoundFXClip(correctBulletHitSound, transform, 0.3f);
                    // if magic enemy is being hit by bullet take full damage
                    enemyComponent.TakeDamage(1 * damageMultiplier, gameObject);
                }
                else
                {
                    SoundManager.Instance.PlaySoundFXClip(incorrectBulletHitSound, transform, 0.3f);
                    // if non magic enemy hit by bullet, take half damage
                    enemyComponent.TakeDamage(0.5f * damageMultiplier, gameObject);
                }
            }
        }
        Destroy(gameObject); // Destroy bullet after collision
    }
}