using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DawnScript : MonoBehaviour
{
    public float speed = 10f; // Speed of the swoop
    
    private Vector3 startPoint; // Start position
    private Vector3 endPoint;   // End position
    private SpriteRenderer spriteRenderer;
    private TriggerZoneScript triggerZoneScript;
    private Vector3 controlPoint; // Player position
    private float elapsedTime = 0f;
    private float duration;
    private bool isActive = false;

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
            UpdateVisibility();
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
        spriteRenderer.enabled = true;
        GetComponent<Collider2D>().enabled = true;
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

        // Update position
        transform.position = newPosition;

        // Check if the swoop is finished
        if (t >= 1f)
        {
            ResetDawn();
        }
    }

    private void UpdateVisibility()
    {
        // You could add more checks for sprite visibility here if needed
        spriteRenderer.enabled = true;
    }

    // Quadratic Bezier curve calculation
    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        return (1 - t) * (1 - t) * p0 + 2 * (1 - t) * t * p1 + t * t * p2;
    }
}
