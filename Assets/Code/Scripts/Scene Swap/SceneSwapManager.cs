using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneSwapManager : MonoBehaviour
{
    public static SceneSwapManager instance;

    public string spawnPointTag; // The tag of the spawn point to identify where the player should spawn

    private void Awake()
    {
        // Ensures that there is only one instance of the SceneSwapManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Subscribe to sceneLoaded event
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Unsubscribe from sceneLoaded event
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Method to position the player at the spawn point after the scene loads
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(PositionPlayerAfterSceneLoad(scene));
    }

    private IEnumerator PositionPlayerAfterSceneLoad(Scene scene)
    {
        // Wait for one frame to ensure all objects in the scene are initialized
        yield return null;

        if (string.IsNullOrEmpty(spawnPointTag))
        {
            Debug.LogWarning($"Scene '{scene.name}' loaded, but spawnPointTag is not set. Cannot position player.");
            yield break;
        }

        // Find the spawn point by tag
        GameObject spawnPoint = GameObject.FindWithTag(spawnPointTag);
        if (spawnPoint == null)
        {
            Debug.LogError($"Scene '{scene.name}' loaded, but no GameObject with tag '{spawnPointTag}' found.");
            yield break;
        }

        // Find the player
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player GameObject not found in the scene. Ensure the player is tagged correctly.");
            yield break;
        }

        // Move the player to the spawn point
        player.transform.position = spawnPoint.transform.position;
        Debug.Log($"Player positioned at spawn point '{spawnPoint.name}' in scene '{scene.name}'.");

        // Update the camera's target to the player
        CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
        if (cameraFollow != null)
        {
            cameraFollow.SetPlayer(player.transform);
            Debug.Log("Camera target updated to the player.");
        }
        else
        {
            Debug.LogWarning("CameraFollow component not found on the main camera.");
        }
    }
}
