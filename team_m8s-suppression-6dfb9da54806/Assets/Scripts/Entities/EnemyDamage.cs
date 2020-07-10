using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour {
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public float attackRangeX;
    public float attackRangeY;
    
    public int damage;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (timeBtwAttack <= 0)//allowed to attack//stops spam
        {            
             //Debug.Log("POKE_DID DAMAGE");
             Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(attackRangeX,attackRangeY), 0,whatIsEnemies);
             for (int i = 0; i < enemiesToDamage.Length; i++)
             {
                enemiesToDamage[i].GetComponent<PlayerController>().TakeDamage(damage);
                
             }           
            timeBtwAttack = startTimeBtwAttack;
        }
        else
        {
            timeBtwAttack -= startTimeBtwAttack;
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos.position, new Vector3(attackRangeX,attackRangeY,1));
    }
}
