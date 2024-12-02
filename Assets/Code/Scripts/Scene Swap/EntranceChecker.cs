using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildingEntrance : MonoBehaviour
{
    [SerializeField] private string interiorSceneName; // Name of the interior scene to load
    [SerializeField] private string spawnPointTag; // Tag for the spawn point

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object colliding is the player
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player)
        {
            // Hardcode going back to main menu at end of game
            if (SceneManager.GetActiveScene().name == "Level3_Exterior")
            {
                SceneManager.LoadScene("Main Menu");
            }

            // Load the interior scene
            if (SceneManager.GetActiveScene().name != interiorSceneName)
            {
                Debug.Log($"Transitioning to scene: {interiorSceneName} with spawn point tag: {spawnPointTag}");
                SceneSwapManager.instance.spawnPointTag = spawnPointTag; // Set spawn point tag in SceneSwapManager
                SceneManager.LoadScene(interiorSceneName);
            }
            else
            {
                // If already in the interior, load the town scene
                LoadTownScene();
            }
        }
    }

    private void LoadTownScene()
    {
        string townSceneName = "Level1_Town_Exterior";

        // Set the appropriate spawn point tag based on the building
        if (interiorSceneName == "Level1_Saloon_Interior")
        {
            SceneSwapManager.instance.spawnPointTag = "ExitPoint_1";
        }
        else if (interiorSceneName == "Level1_Bank_Interior")
        {
            SceneSwapManager.instance.spawnPointTag = "ExitPoint_2";
        }
        else if (interiorSceneName == "Level1_PostOffice_Interior")
        {
            SceneSwapManager.instance.spawnPointTag = "ExitPoint_3";
        }
        else if (interiorSceneName == "Level1_GeneralStore_Interior")
        {
            SceneSwapManager.instance.spawnPointTag = "ExitPoint_4";
        }

        Debug.Log($"Transitioning to town scene: {townSceneName} with spawn point tag: {SceneSwapManager.instance.spawnPointTag}");
        SceneManager.LoadScene(townSceneName); // Load the town scene
    }
}
