using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimSystem : MonoBehaviour
{
    public Transform gun; //Reference to unmirrored gun
    public Transform wand; 
    public Transform revolver;
    public GameObject[] bodySprites; // Array of body sprites (8 directions)
    public GameObject[] cowboySprites;//Cowboy Sprites
    public GameObject[] wizardSprites;//Wizard Spritess
    private Camera mainCamera;
    private Vector2 aimDirection;


    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        Aim();
        if(Input.GetKeyDown(KeyCode.LeftShift)){

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

        // Activate the correct sprite based on angle (8 directions)
        if (angle > -22.5f && angle <= 22.5f)
        {
            bodySprites[3].SetActive(true); // Right
        }
        else if (angle > 22.5f && angle <= 67.5f)
        {
            bodySprites[2].SetActive(true); // UpRight
        }
        else if (angle > 67.5f && angle <= 112.5f)
        {
            bodySprites[1].SetActive(true); // Up
        }
        else if (angle > 112.5f && angle <= 157.5f)
        {
            bodySprites[0].SetActive(true); // UpLeft
        }
        else if (angle > 157.5f || angle <= -157.5f)
        {
            bodySprites[6].SetActive(true); // Left
        }
        else if (angle > -157.5f && angle <= -112.5f)
        {
            bodySprites[5].SetActive(true); // DownLeft
        }
        else if (angle > -112.5f && angle <= -67.5f)
        {
            bodySprites[7].SetActive(true); // Down
        }
        else if (angle > -67.5f && angle <= -22.5f)
        {
            bodySprites[4].SetActive(true); // DownRight
        }
    }
}
