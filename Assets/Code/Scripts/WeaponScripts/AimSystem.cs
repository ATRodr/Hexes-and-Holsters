using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimSystem : MonoBehaviour
{
    public Transform gunRight;  // Reference to unmirrored gun
    public Transform gunLeft;   // Reference to mirrored gun
    public GameObject[] bodySprites; // Array of body sprites (8 directions)

    private Camera mainCamera;
    private Vector2 aimDirection;
    private bool isGunLeft; // Determines if the mirrored gun is active

    void Start()
    {
        mainCamera = Camera.main;
        // Ensure both guns are set to inactive at the start, or set the default one
        gunRight.gameObject.SetActive(true);
        gunLeft.gameObject.SetActive(false);
    }

    void Update()
    {
        Aim();
        UpdateBodySprite();
    }

    void Aim()
    {
        // Get mouse position in world space
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; // Set Z position to 0 for 2D

        // Calculate aim direction
        Vector3 playerPos = transform.position;
        aimDirection = mousePos - playerPos;

        // Get the angle in degrees
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        // Rotate the active gun based on the angle
        if (!isGunLeft)
        {
            gunRight.position = playerPos; // Keep gun position at player
            gunRight.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            gunLeft.position = playerPos; // Keep gun position at player
            gunLeft.rotation = Quaternion.Euler(0, 0, angle);
        }

        // Switch guns based on the angle (-90 to 90 shows right gun, otherwise left gun)
        if (angle > -90 && angle < 90)
        {
            if (isGunLeft)
            {
                SwitchToGunRight();
            }
        }
        else
        {
            if (!isGunLeft)
            {
                SwitchToGunLeft();
            }
        }
    }

    void SwitchToGunRight()
    {
        gunRight.gameObject.SetActive(true);
        gunLeft.gameObject.SetActive(false);
        isGunLeft = false;
    }

    void SwitchToGunLeft()
    {
        gunRight.gameObject.SetActive(false);
        gunLeft.gameObject.SetActive(true);
        isGunLeft = true;
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
