using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // Player's Transform
    public float cameraSpeed = 5f;  // Speed of camera movement
    public float followRatio = 0.25f;  // Ratio between player and cursor (0.25 for 1/4 point)

    private Vector3 cameraOffset;

    void Start()
    {
        // Calculate the initial offset between the camera and player (optional, if you want a fixed offset)
        player = GameObject.Find("REALPlayerPrefab").transform;
        cameraOffset = transform.position - player.position;
    }

    void Update()
    {
        // Get the player's position
        Vector3 playerPosition = player.position;

        // Get the aim position from the cursor
        Vector3 aimPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aimPosition.z = 0f;  // Ensure it's on the same Z-plane for 2D

        // Calculate the weighted point between player and cursor (1/4 way towards the cursor)
        Vector3 targetPoint = Vector3.Lerp(playerPosition, aimPosition, followRatio);

        // Apply any offset you want to keep the camera a little bit closer to the player
        Vector3 targetPosition = targetPoint + cameraOffset;

        // Smoothly move the camera towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, cameraSpeed * Time.deltaTime);
    }

    public void SetPlayer(Transform newPlayer)
    {
        player = newPlayer;
        if (player != null)
        {
            // Set a fixed offset from the player
            cameraOffset = new Vector3(0, 0, -10f); // Adjust this value as needed
            transform.position = player.position + cameraOffset; // Set the camera position directly
        }
    }
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.localPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null; // Wait for the next frame
        }

        transform.localPosition = originalPosition; // Reset position
    }
}