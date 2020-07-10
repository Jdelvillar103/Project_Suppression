using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(PlayerController))]
[RequireComponent (typeof(Animator))]
public class PlayerAnimator : MonoBehaviour {

    [SerializeField]
    private bool facingRight = true;

    private float moveSpeed;
    private Vector2 input;
    private Vector3 moveDistance;

    Animator animator;
    PlayerController playerController;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();

        moveSpeed = playerController.GetMoveSpeed();
        input = playerController.GetInput();
        moveDistance = playerController.GetMoveDistance();
	}
	
	// Update is called once per frame
	void Update () {
        moveSpeed = playerController.GetMoveSpeed();
        input = playerController.GetInput();
        moveDistance = playerController.GetMoveDistance();

        if (moveDistance.x > 0 || moveDistance.x < 0)
        {
            // set animator movespeed to movespeed
            animator.SetFloat("moveSpeed", moveSpeed);
        }
        else
        {
            // set animator movespeed to 0
            animator.SetFloat("moveSpeed", 0);
        }

        if ((input.x < 0 && facingRight) || (input.x > 0 && !facingRight))
        {
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
            facingRight = !facingRight;
        }

        if (playerController.GetControllerCollisionsBottom() && !Input.GetKeyDown(KeyCode.Space))
        {
            // set animator vertical speed
            animator.SetBool("inAir", false);
        }
        else if (!playerController.GetControllerCollisionsBottom())
        {
            // set animator vertical speed to zero
            animator.SetBool("inAir", true);
        }

        animator.SetFloat("verticalSpeed", moveDistance.y);


        animator.SetBool("attacking", false);
        animator.SetBool("dashing", false);
        animator.SetBool("spinning", false);
	}

    public void SetAttacking()
    {
        animator.SetBool("attacking", true);
    }

    public void SetDashing()
    {
        animator.SetBool("dashing", true);
    }

    public void SetSpinning()
    {
        animator.SetBool("spinning", true);
    }

    public bool GetFacingRight()
    {
        return facingRight;
    }

}
