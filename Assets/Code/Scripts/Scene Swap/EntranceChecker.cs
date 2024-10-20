using UnityEngine;
using UnityEngine.SceneManagement; // Don't forget me!

public class BuildingEntrance : MonoBehaviour
{
#pragma warning disable 0649 // Private variables
    [SerializeField] private string interiorSceneName; // Name of the interior scene to load
    [SerializeField] private string spawnPointTag; // Tag for the spawn point
#pragma warning restore 0649

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object colliding is the player
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player)
        {
            // Load the interior scene
            if (SceneManager.GetActiveScene().name != interiorSceneName)
            {
                SceneManager.LoadScene(interiorSceneName);
                SceneSwapManager.instance.spawnPointTag = spawnPointTag; // Set spawn point tag in SceneSwapManager
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
        // Change this to your town scene name
        string townSceneName = "Level1_Town_Exterior"; // Replace with the actual name of your town scene

        // Set the appropriate spawn point tag based on the building the player is in
        if (interiorSceneName == "Level1_Saloon_Interior") // Replace with the actual name of your building scenes
        {
            SceneSwapManager.instance.spawnPointTag = "ExpitPoint_1";
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

        SceneManager.LoadScene(townSceneName); // Load the town scene
    }

}
