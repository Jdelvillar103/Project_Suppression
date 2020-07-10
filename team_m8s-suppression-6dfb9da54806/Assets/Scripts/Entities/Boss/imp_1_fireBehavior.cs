using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class imp_1_fireBehavior : StateMachineBehaviour {

    [SerializeField]
    private Transform targetTransform;
    private Vector3 magicPos;
    [SerializeField]
    private GameObject manaBoltPrefab;

    [SerializeField]
    private float projectileCooldown;
    [SerializeField]
    private float projectileTimer;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        targetTransform = GameObject.FindGameObjectWithTag("Player").transform;

        magicPos = GameObject.FindGameObjectWithTag("BossMagicPos").transform.position;
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            if (projectileTimer <= 0)
            {
                projectileTimer = projectileCooldown;

                magicPos = GameObject.FindGameObjectWithTag("BossMagicPos").transform.position;

                GameObject manaBolt = Instantiate(manaBoltPrefab, magicPos, Quaternion.identity);

                Vector3 difference = targetTransform.position - magicPos; //recieves direction
                float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg; //get degrees of rotation

                // finds normalized vector using degrees
                float dirX = Mathf.Cos(rotZ * Mathf.Deg2Rad);
                float dirY = Mathf.Sin(rotZ * Mathf.Deg2Rad);

                // makes new vector3 normalized vector
                Vector3 direction = new Vector3(dirX, dirY, 0);

                // grabs StunBolt component and sets direction of StunBolt
                EnemyProjectile newEnemyProjectile = manaBolt.GetComponent<EnemyProjectile>();
                newEnemyProjectile.SetDirection(direction);

                SpriteRenderer sprite = manaBolt.GetComponentInChildren<SpriteRenderer>();
                sprite.transform.localEulerAngles = new Vector3(0, 0, rotZ);
            }
            else
            {
                projectileTimer -= Time.deltaTime;
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
