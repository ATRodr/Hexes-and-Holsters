using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathManager : MonoBehaviour
{
    public static PlayerDeathManager instance;
    private void Awake()
    {
        if (PlayerDeathManager.instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GameOver()
    {
        DeathUIManager _ui = GetComponent<DeathUIManager>();
        if (_ui != null)
        {
            _ui.ToggleDeathPanel();
        }
    }
}
