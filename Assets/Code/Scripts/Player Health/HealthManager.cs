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

    private void Start()
    {
        heartImage = GetComponent<Image>();
    }

    public void SetHeartImage(HeartStatus status, bool isInvincible = false)
    {
        if (heartImage == null)
        {
            heartImage = GetComponent<Image>();
        }
        if (isInvincible)
        {
            Debug.Log("Invincible");
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
            switch (status)
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