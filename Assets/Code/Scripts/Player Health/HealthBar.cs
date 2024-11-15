using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public GameObject heartPrefab;
    public PlayerHealth playerHealth;
    List<HealthManager> hearts = new List<HealthManager>();

    private void OnEnable(){
        PlayerHealth.OnPlayerDamaged += () => DrawHearts();
    }

    private void OnDisable(){
        PlayerHealth.OnPlayerDamaged -= () => DrawHearts();
    }

    private IEnumerator Start(){
        MainManager.Instance.healthBar = this;

        while (MainManager.Instance == null || MainManager.Instance.playerHealth == null)
        {
            yield return null;
        }

        playerHealth = MainManager.Instance.playerHealth;
        DrawHearts();
    }

    public void DrawHearts(bool isInvincible = false){
        ClearHearts();

        float maxHealthRemainder = playerHealth.maxHealth % 2;
        int heartsToMake = (int)((playerHealth.maxHealth / 2) + maxHealthRemainder);

        for(int i = 0; i < heartsToMake; i++){
            CreateEmptyHeart();
        }

        for(int i = 0; i < hearts.Count; i++){
            int heartStatusRemainder = (int)Mathf.Clamp(playerHealth.health - (i * 2), 0, 2);
            hearts[i].SetHeartImage((HeartStatus)heartStatusRemainder, isInvincible);
        }
    }

    public void CreateEmptyHeart(){
        GameObject newHeart = Instantiate(heartPrefab);
        newHeart.transform.SetParent(transform);

        HealthManager heartComponent = newHeart.GetComponent<HealthManager>();
        heartComponent.SetHeartImage(HeartStatus.Empty);
        hearts.Add(heartComponent);
    }

    public void ClearHearts(){
        foreach(Transform t in transform){
            Destroy(t.gameObject);
        }

        hearts = new List<HealthManager>();
    }
}
