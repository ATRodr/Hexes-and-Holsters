using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SecondBossAbilities : MonoBehaviour
{
    private PlayerController playerController;

    private TriggerZoneScript triggerZoneScript;

    private float lastAbilityActivationTime = 0f;

    private bool playerInRoom = false;

    private FireAtPlayer fireAtPlayer;
    
    private float abilityCooldown = 15f;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float initialSpeed;
    float changeSpeedWait;
    float changeFactor;
    float BurstWait;

    private GameObject BossBullet;

    void Start()
    {
        //load boss bullet from resources
        BossBullet = Resources.Load<GameObject>("BossBullet");
        if(BossBullet == null)
            Debug.LogError("BossBullet not found in resources");
        playerController = GameObject.Find("REALPlayerPrefab").GetComponent<PlayerController>();
        fireAtPlayer = GetComponent<FireAtPlayer>();
        SpriteRenderer sprite = enemyPrefab.GetComponent<SpriteRenderer>();
        sprite.sortingLayerName = "Player";
        sprite.sortingOrder = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastAbilityActivationTime < abilityCooldown) return;

        //int randomAttack = Random.Range(0, 2);
        
        StartCoroutine(bulletCircle());

        lastAbilityActivationTime = Time.time;
    }

    
    IEnumerator bulletCircle()
    {
        
        StartCoroutine(ShadowDash());

        //find enemy script this is attached to and stop agent navmes  -> .agent.isOnNavMesh
        if(GetComponent<Enemy>().agent.isOnNavMesh)
                GetComponent<Enemy>().agent.isStopped = true;
        Debug.Log("Bullet Circle ACTIVATED");
        //shoot 36 bullets around enemy and do that 5 times each time making bullets shoot in between the last set shot, try to avoid fire points


        if (Random.Range(0, 2) == 1)
            fireAtPlayer.firingEnabled = false;
        

        for (int i = 0; i <= 5; i++)
        {
            if(i % 2 == 0)
            {
                initialSpeed = 6;
                changeSpeedWait = 0.7f;
                changeFactor = -1;
            }
            else{
                initialSpeed = 10;
                changeSpeedWait = 0.5f;
                changeFactor = -11.3f;
            }
            //lets make an array in which we store all the bullets
            GameObject[] bullets = new GameObject[36];
            for (int j = 0; j < 36; j++)
            {
                bullets[j] = Instantiate(BossBullet, transform.position, Quaternion.identity);
                bullets[j].GetComponent<EnemyBulletScript>().isGrillos = true;
            }

            //use another for to fire bullets make sure to use index to find correct angle
            //for some reason t
            // Fire bullets with evenly spaced angles
            for (int j = 0; j < 36; j++)
            {
                float angleDegrees = j * 10f; // 360 degrees / 36 bullets = 10 degrees apart
                float angleRadians = angleDegrees * Mathf.Deg2Rad;
                Vector2 forceDirection = new Vector2(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians)).normalized;
                bullets[j].GetComponent<Rigidbody2D>().AddForce(forceDirection * initialSpeed, ForceMode2D.Impulse);
            }
                yield return new WaitForSeconds(changeSpeedWait);
            for (int j = 0; j < 36; j++)
            {
                float angleDegrees = j * 10f; // 360 degrees / 36 bullets = 10 degrees apart
                float angleRadians = angleDegrees * Mathf.Deg2Rad; 
                Vector2 forceDirection = new Vector2(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians)).normalized;
                //subtract force from bullet
                bullets[j].GetComponent<Rigidbody2D>().AddForce(forceDirection * changeFactor, ForceMode2D.Impulse);
            }
                yield return new WaitForSeconds(1);

        }
        if(GetComponent<Enemy>().agent.isOnNavMesh)
                GetComponent<Enemy>().agent.isStopped = false;
        fireAtPlayer.firingEnabled = true;
        yield break;
    }
    IEnumerator ShadowDash()
    {
        Debug.Log("Shadow Dash ACTIVATED");
        //check in FireAtPlayer if hasLOS is true then we can dash towards player and shoot
        if(GetComponent<FireAtPlayer>().hasLOS)
        {
            //get distance between player and enemy
            float distance = Vector3.Distance(transform.position, playerController.transform.position);
            //if distance is less than 10
            if(distance < 6)
            {
                yield break;
            }
            else
            {
                Debug.Log("Teleporting");
                //place enemy 3/4 of the way between player and enemy
                Vector3 direction = playerController.transform.position - transform.position;
                Vector3 treefour = transform.position + direction.normalized * distance * 0.75f;
                //set poistion of enemy(gameobject) to halfway
                transform.position = treefour;
                //GetComponent<Enemy>().agent.SetDestination(halfWay);
            }
            //dash towards player
            //shoot at player
        }
        else
        {
            yield break;
        }
        
    }
}
