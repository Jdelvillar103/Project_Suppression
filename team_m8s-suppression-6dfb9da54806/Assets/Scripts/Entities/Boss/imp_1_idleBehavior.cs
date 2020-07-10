using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class imp_1_idleBehavior : StateMachineBehaviour {

    [SerializeField]
    private float timer;
    [SerializeField]
    private float minTime;
    [SerializeField]
    private float maxTime;

    [SerializeField]
    private GameObject teleportParticles;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        timer = Random.Range(minTime, maxTime);
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	    if (timer <= 0)
        {
            animator.SetTrigger("charge");
            FindObjectOfType<AudioManager>().Play("BossCharge");
        }
        else
        {
            timer -= Time.deltaTime;
        }
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Transform playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animator.transform.position = playerPos.position;

        Instantiate(teleportParticles, animator.transform.position, Quaternion.identity);
        FindObjectOfType<AudioManager>().Play("BossTeleport");
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
