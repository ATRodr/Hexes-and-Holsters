using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private bool isPaused = false;
    private UIDocument uiDocument;
    private VisualElement root;
    public float moveSpeed = 5f;
    public Rigidbody2D rb; 
    public GameObject[] sides;

    public AimSystem aimSystem;

    public GameObject dynamite;
    public GameObject explosion;
    
    public Gun gun;
    Vector2 moveDirection;

    [Header("Dash Settings")]  // This is the attribute causing the error
    [SerializeField] float dashSpeed = 15f;
    [SerializeField] float dashDuration = 0.25f;
    [SerializeField] float dashCoolDown = 1f;

    private float nextDynamiteDash;
    bool isDash;
    bool canDash;

    private void Start(){
        aimSystem = GetComponent<AimSystem>();
        uiDocument = GameObject.FindObjectOfType<UIDocument>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        canDash = true;

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
            gun.Fire();
        } 
        //Chain Lightning and cowboy abilty (TBD)
        if(Input.GetKeyDown(KeyCode.E)){
            if(aimSystem.isCowboy){
                if(Time.time > nextDynamiteDash){
                    nextDynamiteDash = Time.time + 1f;
                    StartCoroutine(dynamiteDash());
                }else{
                    Debug.Log("Dynamite Dash on cooldown");
                }
            }else{
                gun.FireLightning();
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
    IEnumerator dynamiteDash()
    {
        nextDynamiteDash = Time.time + 15f;
        Vector2 pos = transform.position;
        Quaternion rot = transform.rotation;
        Debug.Log("Dynamite Dash");
        Instantiate(dynamite, pos, rot);
        StartCoroutine(Dash(0.16f, 27f));
        yield return new WaitForSeconds(2);
        Instantiate(explosion, pos, rot);
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

    private IEnumerator Dash(float duration,float speed){
        canDash = false;
        isDash = true;
        rb.velocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);
        
        yield return new WaitForSeconds(duration);
        isDash = false;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }
}

