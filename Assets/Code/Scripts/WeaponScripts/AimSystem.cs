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
    public GameObject gun; //Reference to unmirrored gun
    public GameObject orb; 
    private GameObject weapon;
    private SpriteRenderer weaponRenderer; 
    public GameObject[] bodySprites; // Array of body sprites (8/4? directions)
    public GameObject[] cowboySprites;//Cowboy Sprites
    public GameObject[] wizardSprites;//Wizard Sprites
    private Camera mainCamera;
    private Vector2 aimDirection;
    public Animator swappingAnimation;

    void Start()
    {
        swappingAnimation.SetBool("isCowboy",isCowboy);
        mainCamera = Camera.main;   
        bodySprites = cowboySprites;
        foreach (GameObject sprite in wizardSprites)
        {
            sprite.SetActive(false);
        }
        weapon = gun;
        gun.SetActive(true);
        orb.SetActive(false);
    }

    void Update()
    {
        Aim(weapon);
        if (Time.time >= swappingDelay + lastSwapTime && Input.GetKeyDown(KeyCode.LeftShift))
        {
            swapping = true; //protects from mid swap updates
            foreach (GameObject sprite in bodySprites)
            {
                sprite.SetActive(false);
            }
            bodySprites = isCowboy ? wizardSprites : cowboySprites;  //swap logic
            weapon = isCowboy ? orb : gun; //swap weapon logic
            isCowboy =!isCowboy;  //match state
            lastSwapTime = Time.time;  //get swap time
            if(isCowboy)
            {
                gun.SetActive(true);
                orb.SetActive(false);
            }else
            {
                gun.SetActive(false);
                orb.SetActive(true);
            }
            StartCoroutine(handleSwapAnimations(swappingAnimation,0.55f)); 
        }else if(!swapping){
            UpdateBodySprite();
        }
    }

    IEnumerator handleSwapAnimations(Animator anim,float duration){
        gun.GetComponent<SpriteRenderer>().enabled = false;
        swappingAnimation.SetBool("isCowboy",isCowboy);
        swappingAnimation.playbackTime = 0;
        swappingAnimation.GetComponent<SpriteRenderer>().enabled = true;
        Debug.Log("Reseting");
        yield return new WaitForSeconds(duration);
        anim.GetComponent<SpriteRenderer>().enabled = false;
        gun.GetComponent<SpriteRenderer>().enabled = true;
        swapping = !swapping;
        lastSwapTime = Time.time;  //get swap time
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

        weap.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Switch guns based on the angle (-90 to 90 shows right gun, otherwise left gun)
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
        // //weaponRenderer = weap.GetComponent(SpriteRenderer);
        // if(angle > -180 && angle < 0)
        // {
        //     weap.GetComponent<SpriteRenderer>().sortingOrder = 7;
        // }
        // else
        // {
        //     weap.GetComponent<SpriteRenderer>().sortingOrder = 1;
        // }
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
        }
    }
}