using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class MorphBullet : MonoBehaviour
{
    private GameObject PolyParti;
    bool canCollide = true;
    void Start()
    {
        PolyParti = Resources.Load<GameObject>("PolyParti");
        if(PolyParti == null)
                Debug.LogError("PolyParti not found");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (canCollide && collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if(collision.gameObject.TryGetComponent<Enemy>(out Enemy enemyComponent))
            {
                StartCoroutine(handleTame(enemyComponent));
            }
        }
        //I really don't want to refactor this so instead we are going to avoid desroying the bullet until CoRout is finished
        //set Game Object to be invisible and make sure it can't collide
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        canCollide = false; 
    }
    IEnumerator handleTame(Enemy enemy)
    {
        if(enemy.isGrillos) 
            yield break;
        GameObject parti = Instantiate(PolyParti, enemy.transform.position, enemy.transform.rotation);
        parti.transform.SetParent(enemy.transform);
        enemy.GetComponent<FireAtPlayer>().isTamed = true;
        enemy.isTamed = true;
        yield return new WaitForSeconds(10f);
        //ensure enemy is still there before we start getting silly
        if(enemy == null)
        {
            Debug.Log("Enemy in Question is gone as fuck");
            yield break;
        }
        enemy.GetComponent<FireAtPlayer>().isTamed = false;
        enemy.isTamed = false;
        Destroy(parti);
        Destroy(gameObject); // Destroy bullet after Coroutine is finished
    }
}
