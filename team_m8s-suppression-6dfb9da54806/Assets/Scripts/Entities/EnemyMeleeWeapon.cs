using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeWeapon : MonoBehaviour {

    [SerializeField]
    private int damage = 1;
    [SerializeField]
    private float attackRange;
    [SerializeField]
    private float timeBtwAttack = 5f;
    private float nextAttackTime;

    [SerializeField]
    private float chargeTime;
    private float attackTime;

    [SerializeField]
    private Transform attackPos;
    [SerializeField]
    private LayerMask whatIsEnemies;

    [SerializeField]
    private bool showDmgHitbox;

    [SerializeField]
    private EnemyController enemyController;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Transform targetTransform;

    private Vector2 displacementFromTarget;
    private float distanceToTarget;

	// Use this for initialization
	void Start () {
        enemyController = GetComponentInParent<EnemyController>();
        animator = GetComponentInParent<Animator>();
        targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
        showDmgHitbox = true;

        animator.SetBool("charging", false);
        animator.SetBool("attacking", false);
	}
	
	// Update is called once per frame
	void Update () {

        displacementFromTarget = targetTransform.position - transform.position;
        distanceToTarget = displacementFromTarget.magnitude;

        // if distance from target less than 2 and not attacking
        if (distanceToTarget < 1.0f && !animator.GetBool("attacking") && !enemyController.GetSuspendedState())
        {
            // if enemy is to the right or the left
            if ((enemyController.GetFacingRight() && displacementFromTarget.x > 0) ||
                (!enemyController.GetFacingRight() && displacementFromTarget.x < 0))
            {
                // if ready for charge time, wait for charge to finish and set animator to charge and suspend enemy
                if (nextAttackTime - Time.time <= chargeTime && !animator.GetBool("charging"))
                {
                    print("CHARGING");
                    attackTime = chargeTime + Time.time;
                    enemyController.SetSuspendState(true);
                    animator.SetBool("charging", true);
                }
            }
        }

        // if ready to attack and charging
        if (Time.time > attackTime && animator.GetBool("charging"))
        {
            print("ATTACKING");
            animator.SetBool("charging", false);

            // plays sword slice
            FindObjectOfType<AudioManager>().Play("SwordSlice");

            // overlaps circle and finds collisions
            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);

            // deal damage to player
            for (int i = 0; i < enemiesToDamage.Length; i++)
            {
                enemiesToDamage[i].GetComponent<PlayerController>().TakeDamage(damage);
            }

            // set next attack time
            nextAttackTime = Time.time + timeBtwAttack;

            // start animator coroutine
            StartCoroutine(AnimatorSetAttack(1f));
        }
    }
    
    private IEnumerator AnimatorSetAttack(float animationLength)
    {
        animator.SetBool("attacking", true);

        yield return new WaitForSeconds(animationLength);

        animator.SetBool("attacking", false);

        enemyController.SetSuspendState(false);
    }

    private void OnDrawGizmos()
    {
        if (showDmgHitbox)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPos.position, attackRange);
        }
    }
}
