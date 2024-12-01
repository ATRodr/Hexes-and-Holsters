using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyAnimations : MonoBehaviour
{
    //private Vector3 defaultScale = new Vector3(1f, 1f, 1f);
    //private Vector3 grillosScale = new Vector3(2f, 2f, 1f);
    //private Transform characterTransform;    
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
            if(enemy.isGrillos){
                enemyAnimations.SetBool("isGrillos", true);
                enemyAnimations.SetBool("isMagic", true); 
                //characterTransform.localScale = grillosScale;
            }
            if (enemy.isMagic && !enemy.isGrillos)
            {
                //characterTransform.localScale = defaultScale;
                if(!enemy.isMelle){
                    enemyAnimations.SetBool("isMelee", false);
                    enemyAnimations.SetBool("isMagic", enemy.isMagic);
                }
                else{
                    enemyAnimations.SetBool("isMelee", true);
                    enemyAnimations.SetBool("isMagic", enemy.isMagic);
                }
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
