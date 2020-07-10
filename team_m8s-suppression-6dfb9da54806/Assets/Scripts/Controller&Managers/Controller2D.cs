using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BoxCollider2D))]
public class Controller2D : RaycastController {

    public CollisionInfo collisions;        // stores collision information
    public RaycastHit2D[] verticalRaycastHits;
    public RaycastHit2D[] horizontalRaycastHits;
    private int verticalHitCounts;
    private int horizontalHitCounts;
    public bool drawRaycasts;

	// Use this for initialization
	public override void Start () {
        base.Start();
        drawRaycasts = true;
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void Move(Vector3 moveDistance)
    {
        UpdateRaycastOrigins();     // updates raycast origins
        collisions.Reset();         // resets collision info

        if (moveDistance.x != 0)
            horizontalCollisions(ref moveDistance);
        if (moveDistance.y != 0)
            verticalCollisions(ref moveDistance);

        transform.Translate(moveDistance);
    }

    // detects vertical collisions and readjusts move distance
    public void verticalCollisions(ref Vector3 moveDistance)
    {
        // determines the direction of object
        float directionY = Mathf.Sign(moveDistance.y);

        // determines ray length
        float rayLength = Mathf.Abs(moveDistance.y) + skinWidth;

        verticalRaycastHits = new RaycastHit2D[verticalRayCount];

        verticalHitCounts = 0;

        // draws vertical rays
        for (int i = 0; i < verticalRayCount; i++)
        {
            // if direction down, set ray origin to bottom left -- else top left
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;

            // finds origin of ray
            rayOrigin += Vector2.right * (verticalRaySpacing * i + moveDistance.x);

            // projects 2d physics raycast
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            // top/bottom rays
            if (drawRaycasts)
                Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            // if collision detected
            if (hit)
            {// sets collision tag array
                verticalRaycastHits[verticalHitCounts] = hit;
                verticalHitCounts++;

                // adjust move distance
                moveDistance.y = (hit.distance - skinWidth) * directionY;

                // change ray length
                rayLength = hit.distance;

                // sets bool values to collisions
                collisions.top = directionY == 1;
                collisions.bottom = directionY == -1;
            }
        }
    }

    // detects vertical collisions and readjusts move distance
    public void horizontalCollisions(ref Vector3 moveDistance)
    {
        // determines the direction of object
        float directionX = Mathf.Sign(moveDistance.x);

        // determines ray length
        float rayLength = Mathf.Abs(moveDistance.x) + skinWidth;

        horizontalRaycastHits = new RaycastHit2D[horizontalRayCount];

        horizontalHitCounts = 0;

        // draws vertical rays
        for (int i = 0; i < horizontalRayCount; i++)
        {
            // if direction down, set ray origin to bottom left -- else bottom right
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;

            // finds origin of ray
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);

            // projects 2d physics raycast
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            // left/right rays
            if (drawRaycasts)
                Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            // if collision detected
            if (hit)
            {
                // set horizontal collision tag array
                horizontalRaycastHits[horizontalHitCounts] = hit;
                horizontalHitCounts++;

                // adjust move distance
                moveDistance.x = (hit.distance - skinWidth) * directionX;

                // change ray length
                rayLength = hit.distance;

                // sets bool values to collisions
                collisions.left = directionX == -1;
                collisions.right = directionX == 1;
            }
        }
    }

    

    // stores all the collision info
    public struct CollisionInfo
    {
        public bool top, bottom;
        public bool left, right;

        // sets all values to false
        public void Reset()
        {
            top = bottom = false;
            left = right = false;
        }
    }

    // returns true if collision is detected
    public bool GetAnyCollision()
    {
        if (collisions.top == true || collisions.bottom == true || collisions.left == true || collisions.right == true)
        {
            return true;
        }

        return false;
    }

    // finds index with matching tag in vertical raycast hits array
    public int FindVerticalTagIndex(string tag)
    {
        for (int i = 0; i < verticalHitCounts; i++)
        {
            if (verticalRaycastHits[i].collider.tag == tag)
            {
                return i;
            }
        }

        return -1;
    }

    // finds index with matching tag in horizontal raycast hits array
    public int FindHorizontalTagIndex(string tag)
    {
        for (int i = 0; i < horizontalHitCounts; i++)
        {
            if (horizontalRaycastHits[i].collider.tag == tag)
            {
                return i;
            }
        }

        return -1;
    }

    // returns vertical raycast hits array
    public RaycastHit2D[] GetVerticalRaycastHits()
    {
        return verticalRaycastHits;
    }

    // returns horizontal raycast hits array
    public RaycastHit2D[] GetHorizontalRaycastHits()
    {
        return horizontalRaycastHits;
    }

    // returns vertical raycast hit at specific index
    public RaycastHit2D GetVerticalRaycastHitAtIndex(int index)
    {
        return verticalRaycastHits[index];
    }
    
    // returns horizontal raycast hit at specific index
    public RaycastHit2D GetHorizontalRaycastHitAtIndex(int index)
    {
        return horizontalRaycastHits[index];
    }
}
