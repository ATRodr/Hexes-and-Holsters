using System.Collections;
using System.Collections.Generic;
using Code.Scripts.SkillTreeSystem;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{    
    private PlayerSkillManager playerSkillManager;
    public PlayerSkillManager PlayerSkillManager => playerSkillManager;

    private UIDocument  uiDocument;
    public UIDocument UIDocument => uiDocument;

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();        
        playerSkillManager = FindObjectOfType<PlayerSkillManager>();
    }
}
