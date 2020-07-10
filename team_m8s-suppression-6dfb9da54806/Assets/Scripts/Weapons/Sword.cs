using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {

    [SerializeField]
    private int damage = 10;
    [SerializeField]
    private float attackRange;
    [SerializeField]
    private float timeBtwAttack = 1f;
    private float timeLastAttack;

    [SerializeField]
    private Transform attackPos;
    [SerializeField]
    private LayerMask whatIsEnemies;

    [SerializeField]
    private bool showDmgHitbox;
    
	// Use this for initialization
	void Start () {
        showDmgHitbox = true;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0) && Time.timeScale != 0f)
        {
            if (Time.time - timeLastAttack >= timeBtwAttack) //allowed to attack//stops spam
            {
                PlayerAnimator animator = GetComponent<PlayerAnimator>();
                animator.SetAttacking();

                //Doesn't play sound during pause menu
                if (Time.timeScale != 0f)
                    FindObjectOfType<AudioManager>().Play("SwordSlice");
                print("Player swings his sword!");
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position,attackRange,whatIsEnemies);

                for(int i = 0; i < enemiesToDamage.Length ;i++)
                {
                    try
                    {
                        enemiesToDamage[i].GetComponent<EnemyController>().TakeDamage(damage);
                    }
                    catch (Exception e)
                    {
                        enemiesToDamage[i].GetComponent<BossController>().TakeDamage(damage);
                    }
                }

                timeLastAttack = Time.time;
            }
        }
	}
    void OnDrawGizmosSelected()
    {
        if (showDmgHitbox)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPos.position, attackRange);
        }
    }


    
}
