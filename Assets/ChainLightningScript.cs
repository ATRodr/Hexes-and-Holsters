using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChainLightningScript : MonoBehaviour
{
    private CircleCollider2D coll;

    public LayerMask enemyLayer;
    public float damage;

    public GameObject ChainLightningEffect;
    public GameObject beenStuck;

    public int amountToChain;

    private GameObject StartObject;
    public GameObject EndObject;

    private Animator ani;

    public ParticleSystem parti;

    private int singleSpawn;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        if(amountToChain <= 0)
        {
            Debug.Log("no more chains");
            Destroy(gameObject);
            return;
        }
        coll = GetComponent<CircleCollider2D>();
        ani = GetComponent<Animator>();

        StartObject = gameObject;
        parti = GetComponent<ParticleSystem>();
        singleSpawn =  1;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Hit: {collision.gameObject.name}, Layer: {collision.gameObject.layer}");
        if (enemyLayer == (enemyLayer | (1 << collision.gameObject.layer)) && !collision.GetComponentInChildren<EnemyStruck>())
        {
            Debug.Log("Hit Enemy");
            if(singleSpawn != 0){
                EndObject = collision.gameObject;
                amountToChain -= 1; 

                Instantiate(ChainLightningEffect, collision.transform.position, Quaternion.identity);
                Instantiate(beenStuck, collision.transform.position, Quaternion.identity);

                collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);


                ani.StopPlayback();

                coll.enabled = false;

                singleSpawn -= 1;

                parti.Play();
                var emitParams = new ParticleSystem.EmitParams();

                emitParams.position = StartObject.transform.position;   
                parti.Emit(emitParams, 1);
            
                emitParams.position = EndObject.transform.position;
                parti.Emit(emitParams, 1); 

                Destroy(gameObject, 1f);
            }
        }
    }
}
