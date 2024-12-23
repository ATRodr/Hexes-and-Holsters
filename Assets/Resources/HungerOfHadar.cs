using System.Collections;
using Unity.VisualScripting;
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
            // get audio clip from folder
            AudioClip hadarSound = Resources.Load<AudioClip>("Hadar");
            SoundManager.Instance.PlaySoundFXClip(hadarSound, transform, 0.08f);
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
        // play sound
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
        if(collision.gameObject.TryGetComponent<Enemy>(out Enemy enemyComponent))
        {
            float disFromCenter = (enemyComponent.transform.position - transform.position).magnitude;
            if(disFromCenter > 2.7)
                return;
            Debug.Log("within hunger of hadar");
            if(enemyComponent.agent.isOnNavMesh)
                enemyComponent.agent.isStopped = true;
            enemyComponent.TakeDamage(damagePerFrame); //change back to damagePerFrame after testing
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