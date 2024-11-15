using UnityEngine;
using UnityEngine.SceneManagement;

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

    // Called when the scene finishes loading
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Method to position the player at the spawn point after the scene loads
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find the spawn point by tag
        GameObject spawnPoint = GameObject.FindWithTag(spawnPointTag);
        if (spawnPoint != null)
        {
            // Move the player to the spawn point
            GameObject player = MainManager.Instance.playerController.gameObject;
            if (player != null)
            {
                player.transform.position = spawnPoint.transform.position; // Set the player's position

                // Find the camera and set the player as the target
                CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
                if (cameraFollow != null)
                {
                    cameraFollow.SetPlayer(player.transform); // Update the camera's target to the player
                }
            }
        }
    }
}
