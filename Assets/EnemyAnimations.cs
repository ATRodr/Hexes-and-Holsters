using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyAnimations : MonoBehaviour
{
    private Enemy enemy;
    public Animator enemyAnimations;
    void Start()
    {
        // Get the Enemy component from the parent GameObject
        enemy = GetComponentInParent<Enemy>();

        if (enemy != null)
        {
            // Access the 'isMagic' variable from the Enemy script
            Debug.Log("Is Magic: " + enemy.isMagic);
        }
        else
        {
            Debug.LogError("Enemy component not found in parent!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy != null)
        {
            if (enemy.isMagic)
            {
                enemyAnimations.SetBool("isMagic", enemy.isMagic);
                if(enemy.isMoving){
                    enemyAnimations.SetBool("isMoving", true);
                }
                else{
                    enemyAnimations.SetBool("isMoving", false);
                }
            }
        }
    }
}