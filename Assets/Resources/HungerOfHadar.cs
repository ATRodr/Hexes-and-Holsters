using System.Collections;
using UnityEngine;

public class HungerOfHadar : MonoBehaviour
{


    bool hasCasted = false;
    // Start is called before the first frame update
    void Start()
    {
        //make sure particle effect doen't start playing until casted
        this.gameObject.GetComponent<ParticleSystem>().Stop();
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
        this.gameObject.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(8);
        Destroy(this.gameObject);
    }
}
