using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathUIManager : MonoBehaviour
{
    [SerializeField] GameObject deathPanel;
    
    public void ToggleDeathPanel()
    {
        // Set it to its opposite
        deathPanel.SetActive(!deathPanel.activeSelf);
    }
}
