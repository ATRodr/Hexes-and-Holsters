using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Code.Scripts.SkillTreeSystem;

public class PlayerController : MonoBehaviour
{
    private bool isPaused = false;
    private UIDocument uiDocument;
    private VisualElement root;
    public float moveSpeed = 5f;
    public Rigidbody2D rb; 
    public AimSystem aimSystem;
    public GameObject dynamite;
    public GameObject explosion;
    public PlayerHealth playerHealth;
    public HealthBar healthBar;
    public Weapon weapon;
    Vector2 moveDirection;
    private PlayerSkillManager skillManager;

    [Header("Dash Settings")]  // This is the attribute causing the error
    [SerializeField] float dashSpeed = 15f;
    [SerializeField] float dashDuration = 0.25f;
    [SerializeField] float dashCoolDown = 1f;

    public float nextWizardAbility1Time;
    public float nextWizardAbility2Time;
    public float nextCowboyAbility1Time;
    public float nextCowboyAbility2Time;
    

    bool isDash;
    bool canDash = true;

    private void Start(){
        skillManager = GetComponent<PlayerSkillManager>();
        healthBar = GameObject.FindObjectOfType<HealthBar>();
        playerHealth = GetComponent<PlayerHealth>();
        aimSystem = GetComponent<AimSystem>();
        uiDocument = GameObject.FindObjectOfType<UIDocument>();
        rb = gameObject.GetComponent<Rigidbody2D>();

        // Hide the skill tree UI
        if (uiDocument != null)
        {
            root = uiDocument.rootVisualElement;
            root.visible = false;
        }
        else
        {
            Debug.LogError("UI Document not found in the scene.");
        }
    }

    void Update()
    {
        if(isDash){
            return;
        }
        float moveX = Input.GetAxisRaw("Horizontal"); 
        float moveY = Input.GetAxisRaw("Vertical"); 

        //normal bullet
        if(Input.GetMouseButtonDown(0)){
            weapon.Fire();
        } 
        
        if(Input.GetKeyDown(KeyCode.E)){
            if(Time.time >= nextCowboyAbility1Time && aimSystem.isCowboy){
                    skillManager.castCowboyAbility1();
                    Debug.Log("Cowboy Ability 1");
            }else if(Time.time >= nextWizardAbility1Time) {
                    skillManager.castWizardAbility1();
                    Debug.Log("wiz Ability 1");
            }
        }
        //tenatively F key for testing can change to whatever
        if(Input.GetKeyDown(KeyCode.F)){
            if(Time.time >= nextCowboyAbility2Time && aimSystem.isCowboy){
                    skillManager.castCowboyAbility2();
                    Debug.Log("Cowboy Ability 2");
            }else if(Time.time >= nextWizardAbility2Time) {
                    skillManager.castWizardAbility2();
                    Debug.Log("wiz Ability 2");
            }
        }
        

        if(Input.GetKeyDown(KeyCode.Space) && canDash){
            StartCoroutine(Dash(dashDuration,dashSpeed));
        }

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleSkillTree();
        }

        moveDirection = new Vector2(moveX, moveY).normalized;
    }
    private void ToggleSkillTree()
    {
        isPaused = !isPaused;

        // Show or hide the root element
        root.visible = isPaused;
        
        // Pause or resume the game
        Time.timeScale = isPaused ? 0f : 1f;
    }

    private void FixedUpdate(){
        if(isDash){
            return;
        }
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    public IEnumerator Dash(float duration,float speed){
        canDash = false;
        isDash = true;
        rb.velocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);
        
        yield return new WaitForSeconds(duration);
        isDash = false;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }
}

