using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DawnScript : MonoBehaviour
{
    public float speed = 10f; // Speed of the swoop
    public float dawnTickRate = 0.5f; // Time between hits
    private Vector3 startPoint; // Start position
    private Vector3 endPoint;   // End position
    private SpriteRenderer spriteRenderer;
    private TriggerZoneScript triggerZoneScript;
    private Vector3 controlPoint; // Player position
    private float elapsedTime = 0f;
    private float duration;
    private bool isActive = false;
    private float lastDawnHit = 0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        triggerZoneScript = GameObject.Find("TriggerZone").GetComponent<TriggerZoneScript>();
    }

    void Update()
    {
        if (isActive)
        {
            UpdatePosition();
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }

    public void ActivateDawn(Vector3 playerPosition)
    {
        Debug.Log("Dawn activated");

        // Initialize positions
        startPoint = triggerZoneScript.GetRandomPositionInCollider(playerPosition);
        endPoint = triggerZoneScript.GetRandomPositionInCollider(playerPosition);
        controlPoint = playerPosition;

        // Calculate duration based on curve length and speed
        float curveLength = Vector3.Distance(startPoint, controlPoint) + Vector3.Distance(controlPoint, endPoint);
        duration = curveLength / speed;

        elapsedTime = 0f;
        isActive = true;
    }

    private void ResetDawn()
    {
        Debug.Log("Dawn reset");

        spriteRenderer.enabled = false;
        GetComponent<Collider2D>().enabled = false;

        // Reset state
        isActive = false;
        elapsedTime = 0f;
    }

    private void UpdatePosition()
    {
        elapsedTime += Time.deltaTime;
        
        // Calculate normalized time (0 to 1)
        float t = elapsedTime / duration;

        // Calculate the position along the quadratic Bezier curve
        Vector3 newPosition = CalculateBezierPoint(t, startPoint, controlPoint, endPoint);

        spriteRenderer.enabled = true;
        GetComponent<Collider2D>().enabled = true;
        
        // Update position
        transform.position = newPosition;

        // Check if the swoop is finished
        if (t >= 1f)
        {
            ResetDawn();
        }
    }

    // Quadratic Bezier curve calculation
    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        return (1 - t) * (1 - t) * p0 + 2 * (1 - t) * t * p1 + t * t * p2;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // player should not take damage if they were hit by Dawn in the last dawnTickRate seconds
            if (Time.time - lastDawnHit < dawnTickRate)
            {
                return;
            }
            lastDawnHit = Time.time;
            
            Debug.Log("Player hit by Dawn");
            
            other.GetComponent<PlayerHealth>().TakeDamage(1f);
        }
    }
}
