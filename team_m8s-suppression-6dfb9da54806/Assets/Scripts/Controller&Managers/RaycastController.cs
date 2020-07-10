using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour {

    public LayerMask collisionMask;

    public const float skinWidth = 0.015f;         // width of the skin for raycasting
    public int horizontalRayCount = 4;      // number of horizontal raycasts
    public int verticalRayCount = 4;        // number of vertical raycasts

    [HideInInspector]       // hides variable below from inspector
    public float horizontalRaySpacing;             // horizontal spacing of raycasts
    [HideInInspector]
    public float verticalRaySpacing;               // vertical spacing of raycasts

    [HideInInspector]
    public BoxCollider2D collider;                 // box collider
    public RaycastOrigins raycastOrigins;          // struct vectors of raycastOrigins


    // Use this for initialization (virtual allows extension and called before start)
    public virtual void Awake () {
        collider = GetComponent<BoxCollider2D>();       // grabs BoxCollider2D component
        CalculateRaySpacing();                          // recalculates raycast spacing
    }

    // Update is called once per frame
    public virtual void Start () {
        CalculateRaySpacing();
	}

    // struct to hold raycast origins
    public struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    // updates the raycast origins
    public void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;    // finds the bounds of the collider
        bounds.Expand(skinWidth * -2);      // expands the bounds using skin width

        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);       // finds top left bounds
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);      // finds top right bounds
        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);    // finds bottom left bounds
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);   // finds bottom right bounds
    }

    // calculuates the ray spacing
    public void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;    // finds the bounds of the collider
        bounds.Expand(skinWidth * -2);      // expands the bounds using skin width

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);  // clamps the horizontal raycount
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);      // clamps the vertical raycount

        // finds the spacing between each ray
        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }
}
