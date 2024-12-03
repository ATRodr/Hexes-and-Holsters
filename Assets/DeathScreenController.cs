using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreenController : MonoBehaviour
{
    public GameObject deathScreenCanvas;

    void Start()
    {
        Debug.Log("PLayers is deadddddd!");
        SceneManager.LoadScene("Main Menu");
        //PlayerHealth.OnPlayerDeath += ShowDeathScreen;
    }

    void OnDestroy()
    {
        //PlayerHealth.OnPlayerDeath -= ShowDeathScreen;
    }

    void ShowDeathScreen()
    {
        Debug.Log("Make cnavas show");
        //deathScreenCanvas.SetActive(true);
    }

    public void OnMainMenuButtonClicked()
    {
        ResetGameState();
        SceneManager.LoadScene("Main Menu");
    }

    void ResetGameState()
    {
        // Reset any necessary game state here
        // PlayerPrefs.DeleteAll();
        // Add any additional reset logic if needed
    }
}
