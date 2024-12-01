using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        ResetEnemyStates();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void ResetEnemyStates()
    {
        foreach (var ID in PlayerPrefs.GetString("EnemyID", "").Split(';'))
        {
            if (!string.IsNullOrEmpty(ID))
            {
                PlayerPrefs.DeleteKey(ID);
            }
        }
        PlayerPrefs.Save();

    }
}
