using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToMenu : MonoBehaviour
{
    // This method will be called when the button is clicked.
    public void LoadMainMenu()
    {
        // Load the "Main Menu" scene.
        SceneManager.LoadScene("Main Menu");
    }
}
