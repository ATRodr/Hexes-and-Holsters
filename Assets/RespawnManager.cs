using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class RespawnManager : MonoBehaviour
{
    public void ReloadCurrentScene()
    {
        // Respawn enemies by clearing marked as kill
        // TP back to 0,0 ?
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Main Menu");

    }
}
