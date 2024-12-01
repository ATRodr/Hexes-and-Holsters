using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZoneScript : MonoBehaviour
{
    private List<GameObject> objectsInTrigger = new List<GameObject>();
    public List<GameObject> ObjectsInTrigger => objectsInTrigger;
    public bool playerInRoom = false;
    public int enimiesInRoom = 0;

    void OnTriggerEnter2D(Collider2D col)  
    {
        if (!objectsInTrigger.Contains(col.gameObject))
        {
            if (col.gameObject.GetComponent<Enemy>() != null)
            {
                enimiesInRoom++;
            }
            if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                playerInRoom = true;
            }
            objectsInTrigger.Add(col.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (objectsInTrigger.Contains(col.gameObject))
        {
            if (col.gameObject.GetComponent<Enemy>() != null)
            {
                enimiesInRoom--;
            }
            if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                playerInRoom = false;
            }
            objectsInTrigger.Remove(col.gameObject);
        }
    }
    public Vector3 GetRandomPositionInCollider(Vector3 playerPosition)
    {
        // Get the bounds of the collider
        Bounds bounds = GetComponent<Collider2D>().bounds;

        // Ensure that the positions are spread to either side of the player
        float halfWidth = bounds.size.x / 2f;
        float halfHeight = bounds.size.y / 2f;

        // Split the area around the player into two halves (left and right, or top and bottom)
        Vector3 colliderCenter = bounds.center;

        // Random point to the left of the player
        Vector3 randomPositionLeft = new Vector3(
            Random.Range(colliderCenter.x - halfWidth, playerPosition.x), // Left side of the player
            Random.Range(colliderCenter.y - halfHeight, colliderCenter.y + halfHeight), // Random height within collider bounds
            colliderCenter.z); // Same Z-axis position

        // Random point to the right of the player
        Vector3 randomPositionRight = new Vector3(
            Random.Range(playerPosition.x, colliderCenter.x + halfWidth), // Right side of the player
            Random.Range(colliderCenter.y - halfHeight, colliderCenter.y + halfHeight), // Random height within collider bounds
            colliderCenter.z); // Same Z-axis position

        // You can now choose one of the two points
        return Random.Range(0f, 1f) > 0.5f ? randomPositionLeft : randomPositionRight;
    }


}
