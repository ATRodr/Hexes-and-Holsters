using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimSystem : MonoBehaviour
{
    private float lastSwapTime;

    [SerializeField]
    public float swappingDelay = 0.5f;
    public bool isCowboy = true;
    public bool swapping;

    //gun is cowboys weapon orb is wizrd weapon.
    //Weapon is singleton/parent which takes on the current weapon based on which character is currently being played
    public GameObject gun; 

    //orb parent is what we need to rotate
    public GameObject orbParent; 
    //orb (orb child) is what we need to change layers of animation of
    
    public GameObject orb;
    private GameObject weapon;
    
    // Array of body sprites (4 directions)
    //Cowboy Sprites and Wizard Sprites
    public GameObject[] bodySprites; 
    public GameObject[] cowboySprites;
    public GameObject[] wizardSprites;


    private Camera mainCamera;
    private Vector2 aimDirection;

    //animation used when swapping between characters
    public Animator swappingAnimation;

    void Start()
    {
        //start as cowboy and assign sprites
        swappingAnimation.SetBool("isCowboy",isCowboy);
        bodySprites = cowboySprites;
        
        mainCamera = Camera.main;   

        //ensure wizard sprites are not active
        foreach (GameObject sprite in wizardSprites)
        {
            sprite.SetActive(false);
        }

        //starting weapon is gun
        weapon = gun;
        gun.SetActive(true);
        orbParent.SetActive(false);
    }

    void Update()
    {
        //always update aim 
        Aim(weapon);
        //only update body sprite when not swapping
        if(!swapping)
            UpdateBodySprite();
        if (Time.time >= swappingDelay + lastSwapTime && Input.GetKeyDown(KeyCode.LeftShift))
        {
            //protects from mid swap updates
            swapping = true;

            //set current set of sprits to invisable so we can switch to other sprites
            foreach (GameObject sprite in bodySprites)
            {
                sprite.SetActive(false);
            }
            //call swap then change state 
            swap();
            isCowboy =!isCowboy;  

            //run swap animation
            StartCoroutine(handleSwapAnimations(swappingAnimation,0.55f)); 
        }
    }
    void swap()
    {
        bodySprites = isCowboy ? wizardSprites : cowboySprites;
        weapon.SetActive(false);
        weapon = isCowboy ? orbParent : gun; 
        weapon.SetActive(true);
    }

    IEnumerator handleSwapAnimations(Animator anim,float duration){
        weapon.SetActive(false);
        swappingAnimation.SetBool("isCowboy",isCowboy);
        swappingAnimation.playbackTime = 0;
        swappingAnimation.GetComponent<SpriteRenderer>().enabled = true;
        
        yield return new WaitForSeconds(duration);
        anim.GetComponent<SpriteRenderer>().enabled = false;
        weapon.SetActive(true);
        lastSwapTime = Time.time;  //get swap time
        swapping = !swapping;
    }
    void Aim(GameObject weap)
    {
        // Get mouse position in world space
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
       
        // Calculate aim direction
        Vector2 playerPos = transform.position;
        aimDirection = mousePos - playerPos;

        // Get the angle in degrees
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        gun.transform.rotation = Quaternion.Euler(0, 0, angle);
        weap.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Switch guns based on the angle (-90 to 90 shows right gun, otherwise left gun)
        if(weap == gun)
        {
            
        }
        if(weap = gun)
        {
            if (angle > -90 && angle < 90)
            {
              weap.transform.localScale = new Vector3(0.25f,0.25f,0.25f);
            }
            else
            {
               weap.transform.localScale = new Vector3(0.25f,-0.25f,0.25f);
            }   
        }
    }
    void UpdateBodySprite()
    {
        // Calculate angle for player body sprite rotation
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        // Deactivate all body sprites
        foreach (GameObject sprite in bodySprites)
        {
            sprite.SetActive(false);
        }

        //Debug.Log(angle);
        // Activate the correct sprite based on angle (quad directions)
        if (angle > -45f && angle <= 45f)
        {
            bodySprites[0].SetActive(true); // Right
            gun.GetComponent<SpriteRenderer>().sortingOrder = 0;

        }
        else if (angle > 45f && angle <= 135f)
        {
            bodySprites[1].SetActive(true); // Up
            gun.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
        else if ((angle > 135f && angle <= 180) || angle > -180f && angle <= -135)
        {
            bodySprites[2].SetActive(true); // Left
            gun.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
        else if (angle > -135f && angle <= -45f)
        {
            bodySprites[3].SetActive(true); // Down
            gun.GetComponent<SpriteRenderer>().sortingOrder = 7;
            //orb.GetComponent<SpriteRenderer>().sortingOrder = 7;
        }
    }
}