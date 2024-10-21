using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    public Sprite fullInvHeart;
    public Sprite halfInvHeart;
    
    public PlayerHealth playerHealth;
    Image heartImage;

    private void Awake(){
        playerHealth = FindObjectOfType<PlayerHealth>();
        if(playerHealth == null){
            Debug.LogError("Player Health not found in the scene.");
        }
        heartImage = GetComponent<Image>();
    }

    public void SetHeartImage(HeartStatus status){
        if (playerHealth.isInvincible)
        {
            switch (status)
            {
                case HeartStatus.Empty:
                    heartImage.sprite = emptyHeart;
                    break;
                case HeartStatus.Half:
                    heartImage.sprite = halfInvHeart;
                    break;
                case HeartStatus.Full:
                    heartImage.sprite = fullInvHeart;
                    break;
            }
        }
        else
        {
            switch(status)
            {
                case HeartStatus.Empty:
                    heartImage.sprite = emptyHeart;
                    break;
                case HeartStatus.Half:
                    heartImage.sprite = halfHeart;
                    break;
                case HeartStatus.Full:
                    heartImage.sprite = fullHeart;
                    break;
            }
        }
    }
}

public enum HeartStatus{
    Empty = 0,
    Half = 1,
    Full = 2
}