using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyAnimations : MonoBehaviour
{
    //private Vector3 defaultScale = new Vector3(1f, 1f, 1f);
    private Vector3 cowboyEnemyScale = new Vector3(2f, 2f, 1f);
    private Transform characterTransform;
    private Enemy enemy;
    public Animator enemyAnimations;
    void Start()
    {
        characterTransform = transform;
        // Get the Enemy component from the parent GameObject
        enemy = GetComponentInParent<Enemy>();

        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy != null)
        {
            enemyAnimations.SetBool("isGrillos", enemy.isGrillos);
            enemyAnimations.SetBool("isMagic", enemy.isMagic); 
            enemyAnimations.SetBool("isMelee", enemy.isMelle);

            if(enemy.isGrillos){
                enemyAnimations.SetBool("isGrillos", true);
                enemyAnimations.SetBool("isMagic", true); 
                //characterTransform.localScale = grillosScale;
            }
            if (enemy.isMagic && !enemy.isGrillos)
            {
                
                if(!enemy.isMelle){
                    characterTransform.localScale = new Vector3(1.6f, 1.6f, 1f);
                    enemyAnimations.SetBool("isMelee", false);
                    enemyAnimations.SetBool("isMagic", enemy.isMagic);
                }
                else{
                    characterTransform.localScale = new Vector3(1.9f, 1.9f, 1f);
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
            else if(!enemy.isMagic && !enemy.isGrillos){
                
                if(!enemy.isMelle){
                    characterTransform.localScale = new Vector3(3.3f, 3.3f, 1f);
                    enemyAnimations.SetBool("isMelee", false);
                    enemyAnimations.SetBool("isMagic", enemy.isMagic);
                }
                else{
                    characterTransform.localScale = new Vector3(2.5f, 2.5f, 1f);
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
