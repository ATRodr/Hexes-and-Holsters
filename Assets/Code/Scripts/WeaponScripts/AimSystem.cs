using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class AimSystem : MonoBehaviour
{
    private float lastSwapTime;

    // need player controller to know if game is paused
    private PlayerController controller;
    private CooldownUIController cooldownUIController;
    private Ultimates ultimates;

    [SerializeField] private AudioClip swapSound;

    [SerializeField]
    public float swappingDelay = 0.5f;
    public bool isCowboy = true;
    public bool canShoot = true;
    public bool swapping = false;
    public bool goldenGunActive = false;
    //gun is cowboys weapon orb is wizrd weapon.
    //Weapon is singleton/parent which takes on the current weapon based on which character is currently being played
    public GameObject gun; 
    public GameObject GoldenGun;

    //orb parent is what we need to rotate
    public GameObject orbParent; 
    //orb (orb child) is what we need to change layers of animation of
    
    //animation 
    //public Animator animator;
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

    public bool isWalking = false;
    public bool isWizWalking = false;

    private Vector3 defaultScale = new Vector3(1f, 1f, 1f);
    private Vector3 wizScale = new Vector3(0.53f, 0.53f, 1f);
    private Vector3 targetScale;
    private Transform characterTransform;
    

    void Start()
    {
        controller = GetComponent<PlayerController>();
        ultimates = GetComponent<Ultimates>();
        cooldownUIController = GameObject.Find("AbilityCooldowns").GetComponent<CooldownUIController>();
        GoldenGun = GameObject.Find("GoldenGun");
        //start as cowboy and assign sprites
        swappingAnimation.SetBool("isCowboy",isCowboy);

        characterTransform = transform;


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
        targetScale = defaultScale;

        if(!isCowboy){
            characterTransform.localScale = wizScale;
        }
        else{
            characterTransform.localScale = defaultScale;
        }
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        if (controller.isPaused) return;

        //always update aim 
        Aim(weapon);
        if((Time.time >= (swappingDelay + lastSwapTime)))
        {
            canShoot = true;
        }
        //update walking animation
        AnimatorStateInfo stateInfo = swappingAnimation.GetCurrentAnimatorStateInfo(0);
        
        if((stateInfo.IsName("wizard") || stateInfo.IsName("wizard walk"))){
            swappingAnimation.GetComponent<SpriteRenderer>().enabled = false;
            weapon.SetActive(false);
            //Debug.Log("sprite disabled");
            
            //Debug.Log("Scale changed");
            weapon.SetActive(true);
            swappingAnimation.GetComponent<SpriteRenderer>().enabled = true;
            //Debug.Log("Sprite enabled");
            characterTransform.localScale = wizScale;
        }
        if((stateInfo.IsName("cowboy idle") || stateInfo.IsName("cowboy walk"))){
            swappingAnimation.GetComponent<SpriteRenderer>().enabled = false;
            weapon.SetActive(false);
            //Debug.Log("sprite disabled");
            
            characterTransform.localScale = defaultScale;
            //Debug.Log("Scale changed");
            weapon.SetActive(true);
            swappingAnimation.GetComponent<SpriteRenderer>().enabled = true;
            //Debug.Log("Sprite enabled");
        }
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float speed = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
        //only update body sprite when not swapping
        if(!swapping && isCowboy){
            
            UpdateBodySprite();
            weapon.SetActive(true);
            foreach (GameObject sprite in bodySprites)
            {
                sprite.SetActive(false);
            }
            StartCoroutine(handleIdleAnimations());
            if(speed > 0){
                isWalking = true;
                if(isCowboy){
                    StartCoroutine(handleWalkingAnimations(swappingAnimation));
                }
            }
            else{
                isWalking = false;
                swappingAnimation.SetBool("IsWalking", false);
            }
        }
        
        else if(!swapping && !isCowboy){
            
            //Debug.Log("Wizard and Not Swapping");
            UpdateBodySprite();
            weapon.SetActive(true);
            foreach (GameObject sprite in bodySprites)
            {
                sprite.SetActive(false);
            }
            StartCoroutine(handleIdleAnimations());
            if(speed > 0){
                //Debug.Log("Wizard Speed > 0");
                isWizWalking = true;
                if(!isCowboy){
                    //Debug.Log("Attempt start wizwalk coroutine");
                    StartCoroutine(handleWizWalkingAnimations(swappingAnimation));
                }
            }
            else{
                isWizWalking = false;
                swappingAnimation.SetBool("IsWizWalking", false);
            }
        }
        
        if((Time.time >= (swappingDelay + lastSwapTime)) && Input.GetKeyDown(KeyCode.LeftShift) && ultimates.canSwap)
        {
            canShoot = false;
            //protects from mid swap updates
            //Debug.Log("Swapped");
            swapping = true;
            
            isCowboy = !isCowboy;

            cooldownUIController.UpdateCooldowns();
            cooldownUIController.UpdateUltimate(fromSwap: true);
            
            //play swap sound
            SoundManager.Instance.PlaySoundFXClip(swapSound, transform, 0.2f);
            StartCoroutine(handleSwapAnimations(swappingAnimation));
            
            //Debug.Log(swapping);
            //Debug.Log(isCowboy);
        }
    }

    IEnumerator handleIdleAnimations(){
        while(!isWalking || !isWizWalking){
            swappingAnimation.SetFloat("MouseDirectionX", aimDirection.x);
            swappingAnimation.SetFloat("MouseDirectionY", aimDirection.y);
            yield return null;
        }
    }
    IEnumerator handleWalkingAnimations(Animator swappingAnimation){
        swappingAnimation.SetBool("IsWalking", true); // Trigger the walking animation

        while (isWalking && isCowboy)
        {
            //Debug.Log("We are enumerating!");
            swappingAnimation.SetFloat("MouseDirectionX", aimDirection.x);
            swappingAnimation.SetFloat("MouseDirectionY", aimDirection.y);
            
            yield return null;
        }
        
    }

    IEnumerator handleWizWalkingAnimations(Animator swappingAnimation){
        swappingAnimation.SetBool("IsWizWalking", true); // Trigger the walking animation

        while (isWizWalking)
        {
            //Debug.Log("We are enumerating!");
            swappingAnimation.SetFloat("MouseDirectionX", aimDirection.x);
            swappingAnimation.SetFloat("MouseDirectionY", aimDirection.y);
            
            yield return null;
        }

        
    }
    IEnumerator handleSwapAnimations(Animator anim){
        
        swappingAnimation.SetBool("IsSwap", true);
        weapon.SetActive(false);
        swappingAnimation.SetBool("isCowboy",isCowboy);
        
        
        //yield return new WaitForSeconds(duration);
        
        lastSwapTime = Time.time;  //get swap time
        swapping = false;
        swappingAnimation.SetBool("IsSwap", false);
        yield return null;
        
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
        /*
        foreach (GameObject sprite in bodySprites)
        {
            sprite.SetActive(false);
        }
        */

        //Debug.Log(angle);
        // Activate the correct sprite based on angle (quad directions)
        if (angle > -45f && angle <= 45f)
        {
            //bodySprites[0].SetActive(true); // Right
            weapon.GetComponent<SpriteRenderer>().sortingOrder = 0;

        }
        else if (angle > 45f && angle <= 135f)
        {
            //bodySprites[1].SetActive(true); // Up
            weapon.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
        else if ((angle > 135f && angle <= 180) || angle > -180f && angle <= -135)
        {
            //bodySprites[2].SetActive(true); // Left
            weapon.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
        else if (angle > -135f && angle <= -45f)
        {
            //bodySprites[3].SetActive(true); // Down
            weapon.GetComponent<SpriteRenderer>().sortingOrder = 7;
            //orb.GetComponent<SpriteRenderer>().sortingOrder = 7;
        }
    }
}