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

    [Header("Dash Settings")]
    [SerializeField] float dashSpeed = 15f;
    [SerializeField] float dashDuration = 1f;
    [SerializeField] float dashCoolDown = 1f;
    bool isDash;
    bool canDash;

    private void Start(){
        rb = gameObject.GetComponent<Rigidbody2D>();
        canDash = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(isDash){
            return;
        }
        float moveX = Input.GetAxisRaw("Horizontal"); 
        float moveY = Input.GetAxisRaw("Vertical"); 

        if(Input.GetMouseButtonDown(0)){
            gun.Fire();
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
