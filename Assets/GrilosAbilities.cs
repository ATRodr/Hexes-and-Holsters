using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GrilosAbilities : MonoBehaviour
{
    private PlayerController playerController;

    private TriggerZoneScript triggerZoneScript;

    private float lastAbilityActivationTime = 0f;

    private float lastRaiseDeadTime = 0f;

    private DawnScript dawnScript;

    private bool playerInRoom = false;

    private FireAtPlayer fireAtPlayer;
    
    [SerializeField] private float abilityCooldown = 15f;
    [SerializeField] private float raiseDeadCooldown = 30f;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject enemyBullet;
    [SerializeField] private Transform[] bulletSpawnPoints;
    [SerializeField] private int bullletCircleCycles = 2;

    void Start()
    {
        triggerZoneScript = GameObject.Find("TriggerZone").GetComponent<TriggerZoneScript>();
        dawnScript = GameObject.Find("DawnCollider").GetComponent<DawnScript>();
        playerController = GameObject.Find("REALPlayerPrefab").GetComponent<PlayerController>();
        fireAtPlayer = GetComponent<FireAtPlayer>();
        SpriteRenderer sprite = enemyPrefab.GetComponent<SpriteRenderer>();
        sprite.sortingLayerName = "Player";
        sprite.sortingOrder = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (triggerZoneScript == null)
        {
            Debug.LogError("TriggerZoneScript is null");
            return;
        }
        if (!triggerZoneScript.playerInRoom)
        {
            return;
        }

        // if one or less goon alive, 50% chance to raise dead
        // every some seconds (count is 2 because boss is also in the trigger)

        if (Time.time - lastRaiseDeadTime > raiseDeadCooldown && triggerZoneScript.enimiesInRoom <= 2)
        {
            lastRaiseDeadTime = Time.time;
            if (Random.Range(0, 2) == 1)
            {
                StartCoroutine(RaiseDead());
            }
        }

        if (Time.time - lastAbilityActivationTime < abilityCooldown) return;

        int randomAttack = Random.Range(0, 2);
        switch (randomAttack)
        {
            case 0:
                StartCoroutine(Dawn());
                break;
            case 1:
                StartCoroutine(bullletCircle());
                break;
        }
        lastAbilityActivationTime = Time.time;
    }

    IEnumerator Dawn()
    {
        // Dawn ability
        dawnScript.ActivateDawn(playerController.GameObject().transform.position);
        yield break;
    }

    IEnumerator RaiseDead()
    {
        // Raise Dead ability
        Debug.Log("Raise Dead");
        for (int i = 0; i < 4; i++)
        {
            Transform spawnpoint = spawnPoints[i];
            GameObject enemy = Instantiate(enemyPrefab, spawnpoint.position, Quaternion.identity);
            NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.enabled = true;
            }
        }
        yield return null;
    }

    IEnumerator bullletCircle()
    {
        fireAtPlayer.firingEnabled = false;
        // Bullet Circle ability

        // allternate between firing bullet at
        // points 0-3 and 4-7

        for (int i = 0; i < bullletCircleCycles; i++)
        {
            // First set of firing 
            for (int j = 0; j < 5; j++)
            {
                GameObject b1 = Instantiate(enemyBullet, bulletSpawnPoints[0].position, Quaternion.identity);
                GameObject b2 = Instantiate(enemyBullet, bulletSpawnPoints[1].position, Quaternion.identity);
                GameObject b3 = Instantiate(enemyBullet, bulletSpawnPoints[2].position, Quaternion.identity);
                GameObject b4 = Instantiate(enemyBullet, bulletSpawnPoints[3].position, Quaternion.identity);

                b1.GetComponent<EnemyBulletScript>().isGrillos = true;
                b2.GetComponent<EnemyBulletScript>().isGrillos = true;
                b3.GetComponent<EnemyBulletScript>().isGrillos = true;
                b4.GetComponent<EnemyBulletScript>().isGrillos = true;

                b1.GetComponent<Rigidbody2D>().AddForce(bulletSpawnPoints[0].up.normalized * 18f, ForceMode2D.Impulse);
                b2.GetComponent<Rigidbody2D>().AddForce(bulletSpawnPoints[1].right.normalized * 18f, ForceMode2D.Impulse);
                b3.GetComponent<Rigidbody2D>().AddForce(-bulletSpawnPoints[2].up.normalized * 18f, ForceMode2D.Impulse);
                b4.GetComponent<Rigidbody2D>().AddForce(-bulletSpawnPoints[3].right.normalized * 18f, ForceMode2D.Impulse);

                yield return new WaitForSeconds(0.5f);
            }

            // Second set of firing 
            for (int j = 0; j < 5; j++)
            {
                GameObject b1 = Instantiate(enemyBullet, bulletSpawnPoints[4].position, Quaternion.identity);
                GameObject b2 = Instantiate(enemyBullet, bulletSpawnPoints[5].position, Quaternion.identity);
                GameObject b3 = Instantiate(enemyBullet, bulletSpawnPoints[6].position, Quaternion.identity);
                GameObject b4 = Instantiate(enemyBullet, bulletSpawnPoints[7].position, Quaternion.identity);

                b1.GetComponent<EnemyBulletScript>().isGrillos = true;
                b2.GetComponent<EnemyBulletScript>().isGrillos = true;
                b3.GetComponent<EnemyBulletScript>().isGrillos = true;
                b4.GetComponent<EnemyBulletScript>().isGrillos = true;

                b1.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1, 1).normalized * 18f, ForceMode2D.Impulse); // Up-left
                b2.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 1).normalized * 18f, ForceMode2D.Impulse);  // Up-right
                b3.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, -1).normalized * 18f, ForceMode2D.Impulse); // Down-right
                b4.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1, -1).normalized * 18f, ForceMode2D.Impulse); // Down-left


                yield return new WaitForSeconds(0.5f);
            }
        }

        fireAtPlayer.firingEnabled = true;
        yield break;
    }

}
