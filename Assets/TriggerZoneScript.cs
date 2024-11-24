using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZoneScript : MonoBehaviour
{
    private List<GameObject> objectsInTrigger = new List<GameObject>();
    public List<GameObject> ObjectsInTrigger => objectsInTrigger;

    void OnTriggerStay2D(Collider2D other)  
    {
        if (!objectsInTrigger.Contains(other.gameObject))
        {
            objectsInTrigger.Add(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (objectsInTrigger.Contains(col.gameObject))
        {
            objectsInTrigger.Remove(col.gameObject);
        }
    }
}
