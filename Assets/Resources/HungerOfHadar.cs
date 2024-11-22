using System.Collections;
using UnityEngine;

public class HungerOfHadar : MonoBehaviour
{


    bool hasCasted = false;

    //damage per second cause its easier to understand for the setting of the damage
    public float damagePerSecond;

    //will be calculated based on the damage per second
    private float damagePerFrame; 
    // Start is called before the first frame update
    void Start()
    {
        setDamagePerSecond(2f);
        //make sure particle effect doen't start playing until casted
        this.gameObject.GetComponent<ParticleSystem>().Stop();

        //turn collider off until casted
        this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!hasCasted)
        {
            followMouse();
        }
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(castHungerOfHadar());
        }
    }
    void followMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;
        Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = objectPos;
    }
    IEnumerator castHungerOfHadar()
    {
        hasCasted = true;

        //turn off area of effect indicator
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;

        //turn on collider
        this.gameObject.GetComponent<CircleCollider2D>().enabled = true;

        //play animation
        this.gameObject.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(8);
        Destroy(this.gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("within hunger of hadar");
        if(collision.gameObject.TryGetComponent<Enemy>(out Enemy enemyComponent))
        {
            if(enemyComponent.agent.isOnNavMesh)
                enemyComponent.agent.isStopped = true;
            enemyComponent.TakeDamage(0); //change back to damagePerFrame after testing
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<Enemy>(out Enemy enemyComponent))
        {
            if(enemyComponent != null && enemyComponent.agent.isOnNavMesh)
                enemyComponent.agent.isStopped = false;
        }
    }
    public void setDamagePerSecond(float damage)
    {
        //set damage
        damagePerSecond = damage;

        //calculate damage per frame
        damagePerFrame = damagePerSecond / 50;

    }
}