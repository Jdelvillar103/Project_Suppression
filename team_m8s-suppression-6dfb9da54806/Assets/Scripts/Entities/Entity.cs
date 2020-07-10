using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Controller2D))]
public class Entity : MonoBehaviour {

    [SerializeField]
    protected float moveSpeed;
    [SerializeField]
    private float maxJumpHeight;
    [SerializeField]
    private float minJumpHeight;
    [SerializeField]
    private float timeToJumpApex;

    protected float gravity;
    protected float maxJumpVelocity;
    protected float minJumpVelocity;
    protected Vector3 moveDistance;

    protected Controller2D controller2D;

	// Use this for initialization
	protected virtual void Start () {
        controller2D = GetComponent<Controller2D>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public Vector3 GetMoveDistance()
    {
        return moveDistance;
    }

    public bool GetControllerCollisionsTop()
    {
        return controller2D.collisions.bottom;
    }

    public bool GetControllerCollisionsBottom()
    {
        return controller2D.collisions.bottom;
    }

    public bool GetControllerCollisionsLeft()
    {
        return controller2D.collisions.left;
    }

    public bool GetControllerCollisionsRight()
    {
        return controller2D.collisions.right;
    }
}
