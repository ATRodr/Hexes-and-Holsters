using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Code.Scripts.SkillTreeSystem;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance { get; private set; }

    public PlayerController playerController;

    public HealthBar healthBar;

    public PlayerHealth playerHealth;

    public PlayerSkillManager playerSkillManager;

    public AimSystem aimSystem;

    public UIManager uiManager;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Debug.Log("MainManager initialized");
    }
}
