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

    [SerializeField] private float abilityCooldown = 15f;
    [SerializeField] private float raiseDeadCooldown = 30f;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject enemyPrefab;

    void Start()
    {
        triggerZoneScript = GameObject.Find("TriggerZone").GetComponent<TriggerZoneScript>();
        dawnScript = GameObject.Find("DawnCollider").GetComponent<DawnScript>();
        playerController = GameObject.Find("REALPlayerPrefab").GetComponent<PlayerController>();
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

        int randomAttack = Random.Range(0, 0);
        switch (randomAttack)
        {
            case 0:
                StartCoroutine(Dawn());
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
