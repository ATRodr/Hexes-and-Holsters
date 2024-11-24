using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Code.Scripts.SkillTreeSystem;

public class PlayerController : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement root;
    public bool isPaused = false;
    public float moveSpeed = 5f;
    public Rigidbody2D rb; 
    public AimSystem aimSystem;

    //Dynamite dash stuff
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

    public float lastWizardAbility1Time = 0;
    public float lastWizardAbility2Time = 0;
    public float lastCowboyAbility1Time = 0;
    public float lastCowboyAbility2Time = 0;
    

    bool isDash;
    bool canDash = true;

    private void Start()
    {
        //for some reason it's not 1 to begin with so this needs to be here
        weapon.setDamageMultiplier(1);
        //Get the dynamite and explosion prefabs from resources
        dynamite = Resources.Load<GameObject>("Dynamite");
        explosion = Resources.Load<GameObject>("Explosion");
        if(dynamite == null)
            Debug.LogError("Dynamite not found");
        if(explosion == null)
            Debug.LogError("Explosion not found");
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
        Debug.Log($"SkillManager instance in PlayerController: {skillManager.GetInstanceID()}");

    }

    void Update()
    {
        if (isDash) return;

        float moveX = Input.GetAxisRaw("Horizontal"); 
        float moveY = Input.GetAxisRaw("Vertical"); 

        //normal bullet
        if(Input.GetMouseButtonDown(0) && aimSystem.canShoot){
            weapon.Fire();
        } 
        
        if(!isPaused && Input.GetKeyDown(KeyCode.E))
        {
            if (aimSystem.isCowboy)
            {
                skillManager.castCowboyAbility(1, ref lastCowboyAbility1Time);
                Debug.Log("Cowboy Ability 1");
            }
            else
            {
                skillManager.castWizardAbility(1, ref lastWizardAbility1Time);
                Debug.Log("wiz Ability 1");
            }
        }
        
        if (!isPaused && Input.GetKeyDown(KeyCode.F))
        {
            if (aimSystem.isCowboy)
            {
                skillManager.castCowboyAbility(2, ref lastCowboyAbility2Time);
                Debug.Log("Cowboy Ability 2");
            }
            else
            {
                skillManager.castWizardAbility(2, ref lastWizardAbility2Time);
                Debug.Log("wiz Ability 2");
            }
        }

        if( !isPaused && Input.GetKeyDown(KeyCode.Space) && canDash){
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
        aimSystem.canShoot = isPaused;
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
        aimSystem.canShoot = false;
        canDash = false;
        isDash = true;
        rb.velocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);
        
        yield return new WaitForSeconds(duration);
        isDash = false;
        aimSystem.canShoot = true;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }
}

