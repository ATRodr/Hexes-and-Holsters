using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimSystem : MonoBehaviour
{
    [SerializeField]
    public float delay = 0.5f;
    public float time;
    public bool isCowboy = true;
    public Transform gun; //Reference to unmirrored gun
    public Transform wand; 
    public Transform revolver;
    public GameObject[] bodySprites; // Array of body sprites (8/4? directions)
    public GameObject[] cowboySprites;//Cowboy Sprites
    public GameObject[] wizardSprites;//Wizard Sprites
    private Camera mainCamera;
    private Vector2 aimDirection;


    void Start()
    {
        mainCamera = Camera.main;
        bodySprites = wizardSprites;
        foreach (GameObject sprite in wizardSprites)
        {
            sprite.SetActive(false);
        }
    }

    void Update()
    {
        Aim();
        if (Time.time >= delay + time && Input.GetKeyDown(KeyCode.LeftShift))
        {
            time = Time.time;
            foreach (GameObject sprite in bodySprites)
            {
                sprite.SetActive(false);
            }
            bodySprites = isCowboy ? wizardSprites : cowboySprites;
            isCowboy = !isCowboy;
        }
        UpdateBodySprite();
    }

    void Aim()
    {
        // Get mouse position in world space
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
       
        // Calculate aim direction
        Vector2 playerPos = transform.position;
        aimDirection = mousePos - playerPos;

        // Get the angle in degrees
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        gun.rotation = Quaternion.Euler(0, 0, angle);
        
        Vector2 scale = transform.localScale;

        // Switch guns based on the angle (-90 to 90 shows right gun, otherwise left gun)
        if (angle > -90 && angle < 90)
        {
            gun.localScale = new Vector3(0.25f,0.25f,0.25f);
        }
        else
        {
            gun.localScale = new Vector3(0.25f,-0.25f,0.25f);
        }
        // gun.localScale = scale;
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
            gun.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
        else if (angle > 45f && angle <= 135f)
        {
            bodySprites[1].SetActive(true); // Up
            gun.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
        else if ((angle > 135f && angle <= 180) || angle > -180f && angle <= -135)
        {
            bodySprites[2].SetActive(true); // Left
            gun.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
        else if (angle > -135f && angle <= -45f)
        {
            bodySprites[3].SetActive(true); // Down
            gun.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
        // else if (angle > 157.5f || angle <= -157.5f)
        // {
        //     bodySprites[6].SetActive(true); // Left
        //     gun.GetComponent<SpriteRenderer>().sortingOrder = 1;
        // }
        // else if (angle > -157.5f && angle <= -112.5f)
        // {
        //     bodySprites[5].SetActive(true); // DownLeft
        //     gun.GetComponent<SpriteRenderer>().sortingOrder = 2;
        // }
        // else if (angle > -112.5f && angle <= -67.5f)
        // {
        //     bodySprites[7].SetActive(true); // Down
        //     gun.GetComponent<SpriteRenderer>().sortingOrder = 2;
        // }
        // else if (angle > -67.5f && angle <= -22.5f)
        // {
        //     bodySprites[4].SetActive(true); // DownRight
        //     gun.GetComponent<SpriteRenderer>().sortingOrder = 2;
        // }
    }
}
