using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZoneScript : MonoBehaviour
{
    private List<GameObject> objectsInTrigger = new List<GameObject>();
    public List<GameObject> ObjectsInTrigger => objectsInTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!objectsInTrigger.Contains(collision.gameObject))
        {
            objectsInTrigger.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (objectsInTrigger.Contains(collision.gameObject))
        {
            objectsInTrigger.Remove(collision.gameObject);
        }
    }
}
