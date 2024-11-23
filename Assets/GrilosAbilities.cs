using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GrilosAbilities : MonoBehaviour
{
    private PlayerController playerController;

    private TriggerZoneScript triggerZoneScript;

    private float lastAbilityActivationTime = 0f;

    private float lastRaiseDeadTime = 0f;

    [SerializeField] private float abilityCooldown = 10f;
    [SerializeField] private float raiseDeadCooldown = 30f;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject enemyPrefab;

    void Start()
    {
        triggerZoneScript = GameObject.Find("TriggerZone").GetComponent<TriggerZoneScript>();
        SpriteRenderer sprite = enemyPrefab.GetComponent<SpriteRenderer>();
        sprite.sortingLayerName = "Player";
        sprite.sortingOrder = 0;
        Enemy enemyScript = enemyPrefab.GetComponent<Enemy>();
        enemyScript.isMelle = true;
        enemyScript.isMagic = true;
        enemyScript.attackRate = 2f;
        enemyScript.health = 4f;
    }

    // Update is called once per frame
    void Update()
    {
        // if one or less skeleton alive, 50% chance to raise dead
        // every 30 seconds

        if (Time.time - lastRaiseDeadTime > raiseDeadCooldown)
        {
            if (triggerZoneScript == null)
            {
                Debug.LogError("TriggerZoneScript is null");
                return;
            }
            // check amount of Enemy game objects
            int count = 0;
            foreach (GameObject enemy in triggerZoneScript.ObjectsInTrigger)
            {
                if (LayerMask.NameToLayer("Enemy") == enemy.layer)
                {
                    count++;
                }
            }
            Debug.Log("Count: " + count);
            // if one or less skeleton alive, 50% chance to raise dead
            if (count <= 1 && Random.Range(0, 2) == 1)
            {
                lastRaiseDeadTime = Time.time;
                StartCoroutine(RaiseDead());
            }
        }

        if (Time.time - lastAbilityActivationTime < abilityCooldown) return;

    }

    IEnumerator Dawn()
    {
        // Dawn ability

        yield return null;
    }

    IEnumerator RaiseDead()
    {
        // Raise Dead ability
        // set spawner object to ON
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
}
