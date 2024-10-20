using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb; 
    public GameObject[] sides;
    
    public Gun gun;
    Vector2 moveDirection;

    [Header("Dash Settings")]  // This is the attribute causing the error
    [SerializeField] float dashSpeed = 15f;
    [SerializeField] float dashDuration = 1f;
    [SerializeField] float dashCoolDown = 1f;
    bool isDash;
    bool canDash;

    private void Start(){
        rb = gameObject.GetComponent<Rigidbody2D>();
        canDash = true;
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
            gun.FireLightning();
        }
        

        if(Input.GetKeyDown(KeyCode.Space) && canDash){
            StartCoroutine(Dash());
        }

        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    private void FixedUpdate(){
        if(isDash){
            return;
        }
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    private IEnumerator Dash(){
        canDash = false;
        isDash = true;
        rb.velocity = new Vector2(moveDirection.x * dashSpeed, moveDirection.y * dashSpeed);
        
        yield return new WaitForSeconds(dashDuration);
        isDash = false;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }
}

