using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class imp_2_fireBehavior : StateMachineBehaviour {

    [SerializeField]
    private Transform targetTransform;
    private Vector3 magicPos;
    [SerializeField]
    private GameObject fireBoltPrefab;
    [SerializeField]
    private GameObject manaBoltPrefab;

    [SerializeField]
    private GameObject[] enemies;

    [SerializeField]
    private float projectileCooldown;
    [SerializeField]
    private float projectileTimer;

    [SerializeField]
    private float splashCooldown;
    [SerializeField]
    private float splashTimer;

    [SerializeField]
    private float spawnCooldown;
    [SerializeField]
    private float spawnTimer;

    private int actionState;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        targetTransform = GameObject.FindGameObjectWithTag("Player").transform;

        magicPos = GameObject.FindGameObjectWithTag("BossMagicPos").transform.position;

        actionState = Random.Range(0, 2);
        Debug.Log(actionState);

        projectileTimer = 0;
        splashTimer = .5f;
        spawnTimer = 0;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            // spawns projectiles around boss
            if (actionState == 0)
            {
                // spawns circle of manabolts
                if (splashTimer <= 0)
                {
                    splashTimer = splashCooldown;

                    projectileTimer = projectileCooldown;


                    magicPos = GameObject.FindGameObjectWithTag("BossMagicPos").transform.position;

                    for (float i = 0; i < 360; i += 360 / 20)
                    {
                        GameObject manaBolt = Instantiate(manaBoltPrefab, magicPos, Quaternion.Euler(0, 0, i));

                        float dirX = Mathf.Cos(i * Mathf.Deg2Rad);
                        float dirY = Mathf.Sin(i * Mathf.Deg2Rad);

                        Vector3 direction = new Vector3(dirX, dirY, 0);

                        EnemyProjectile newEnemyProjectile = manaBolt.GetComponent<EnemyProjectile>();
                        newEnemyProjectile.SetDirection(direction);

                        SpriteRenderer sprite = manaBolt.GetComponentInChildren<SpriteRenderer>();
                        sprite.transform.localEulerAngles = new Vector3(0, 0, i);
                    }
                }
                else
                {
                    splashTimer -= Time.deltaTime;
                }

                // spawns stream of fire bolts
                if (projectileTimer <= 0)
                {
                    projectileTimer = projectileCooldown;

                    GameObject fireBolt = Instantiate(fireBoltPrefab, magicPos, Quaternion.identity);

                    Vector3 difference = targetTransform.position - magicPos; //recieves direction
                    float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg; //get degrees of rotation

                    // finds normalized vector using degrees
                    float dirX = Mathf.Cos(rotZ * Mathf.Deg2Rad);
                    float dirY = Mathf.Sin(rotZ * Mathf.Deg2Rad);

                    // makes new vector3 normalized vector
                    Vector3 direction = new Vector3(dirX, dirY, 0);

                    // grabs StunBolt component and sets direction of StunBolt
                    EnemyProjectile newEnemyProjectile = fireBolt.GetComponent<EnemyProjectile>();
                    newEnemyProjectile.SetDirection(direction);

                    SpriteRenderer sprite = fireBolt.GetComponentInChildren<SpriteRenderer>();
                    sprite.transform.localEulerAngles = new Vector3(0, 0, rotZ);
                }
                else
                {
                    projectileTimer -= Time.deltaTime;
                }
            }

            // spawns enemies from the boss
            else if (actionState == 1)
            {
                int enemyState = Random.Range(0, enemies.Length);
                if (spawnTimer <= 0)
                {
                    spawnTimer = spawnCooldown;
                    Instantiate(enemies[enemyState], magicPos, Quaternion.identity);
                }
                else
                {
                    spawnTimer -= Time.deltaTime;
                }
            }

            // to do
            else if (actionState == 2)
            {
                
            }

            

            animator.SetTrigger("idle");
        }
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
