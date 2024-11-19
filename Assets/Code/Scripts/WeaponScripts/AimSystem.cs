using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class AimSystem : MonoBehaviour
{
    private float lastSwapTime;

    // need player controller to know if game is paused
    private PlayerController controller;

    [SerializeField]
    public float swappingDelay = 0.5f;
    public bool isCowboy = true;
    public bool swapping;
    public bool goldenGunActive = false;
    //gun is cowboys weapon orb is wizrd weapon.
    //Weapon is singleton/parent which takes on the current weapon based on which character is currently being played
    public GameObject gun; 
    public GameObject GoldenGun;

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
        controller = GetComponent<PlayerController>();
        GoldenGun = GameObject.Find("GoldenGun");
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
        if (controller.isPaused) return;

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
            //switch body sprites and flip state 
            bodySprites = isCowboy ? wizardSprites : cowboySprites;
            isCowboy =!isCowboy;  

            //run swap animation
            StartCoroutine(handleSwapAnimations(swappingAnimation,0.55f)); 
        }
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
        GoldenGun.transform.rotation = Quaternion.Euler(0, 0, angle);
        weap.transform.rotation = Quaternion.Euler(0, 0, angle);


        if(weap == gun || weap == GoldenGun)
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
        // GameObject renderThis = gun;

        if(isCowboy)
        {
            if(goldenGunActive)
            {
                weapon = GoldenGun;
                gun.SetActive(false);
                orbParent.SetActive(false);
               // renderThis = GoldenGun;
            }
            else
            {
                weapon = gun;
                orbParent.SetActive(false);
                GoldenGun.SetActive(false);
               // renderThis = gun;
            }
        }else
        {
            weapon = orbParent;
            gun.SetActive(false);
            GoldenGun.SetActive(false);
        }
         weapon.SetActive(true);
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
           // weapon.GetComponent<SpriteRenderer>().sortingOrder = 0;

        }
        else if (angle > 45f && angle <= 135f)
        {
            bodySprites[1].SetActive(true); // Up
           // weapon.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
        else if ((angle > 135f && angle <= 180) || angle > -180f && angle <= -135)
        {
            bodySprites[2].SetActive(true); // Left
           // weapon.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
        else if (angle > -135f && angle <= -45f)
        {
            bodySprites[3].SetActive(true); // Down
           // weapon.GetComponent<SpriteRenderer>().sortingOrder = 7;
            //orb.GetComponent<SpriteRenderer>().sortingOrder = 7;
        }
    }
}