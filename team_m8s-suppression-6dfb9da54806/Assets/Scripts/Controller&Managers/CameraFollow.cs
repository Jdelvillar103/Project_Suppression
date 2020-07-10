using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Controller2D target;     // target of the camera
    public Vector2 focusAreaSize;   // size of the focus area around target

    public float verticalOffset;
    public float lookAheadDistX;
    public float lookSmoothTimeX;

    float currentLookAheadX;
    float targetLookAheadX;
    float lookAheadDirX;
    float smoothLookVelocityX;
    float smoothVelocityY;

    public bool drawGizmos;

    FocusArea focusArea;

	// Use this for initialization
	void Start () {
        focusArea = new FocusArea(target.GetComponent<BoxCollider2D>().bounds, focusAreaSize);
        drawGizmos = true;
	}

    void LateUpdate()
    {
        focusArea.Update(target.GetComponent<BoxCollider2D>().bounds);

        // finds new focus position and follows the object
        Vector2 focusPosition = focusArea.center + Vector2.up * verticalOffset;
        transform.position = (Vector3)focusPosition + Vector3.forward * -10;
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawCube(focusArea.center, focusAreaSize);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    // focus area for the target
    struct FocusArea
    {
        public Vector2 center;      // keeps the center of the focus area
        public Vector2 deltaPosition;   // finds the change in position
        float left, right;          // left/right bounds of area
        float top, bottom;          // top/bottom bounds of area

        // constructor for focus area and sets center using bounds
        public FocusArea(Bounds targetBounds, Vector2 size)
        {
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;
            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;
            deltaPosition = Vector2.zero;
            center = new Vector2((left + right) / 2, (top + bottom) / 2);
        }

        // updates focus area position when target moves against edges
        public void Update(Bounds targetBounds)
        {
            float shiftX = 0;
            // if target is moving against the left edge
            if (targetBounds.min.x < left)
            {
                shiftX = targetBounds.min.x - left;
            }
            // if target is moving against the right edge
            else if (targetBounds.max.x > right)
            {
                shiftX = targetBounds.max.x - right;
            }
            left += shiftX;
            right += shiftX;

            float shiftY = 0;
            // if target is moving against the bottom edge
            if (targetBounds.min.y < bottom)
            {
                shiftY = targetBounds.min.y - bottom;
            }
            // if target is moving against the top edge
            else if (targetBounds.max.y > top)
            {
                shiftY = targetBounds.max.y - top;
            }
            top += shiftY;
            bottom += shiftY;

            center = new Vector2((left + right) / 2, (top + bottom) / 2);
            deltaPosition = new Vector2(shiftX, shiftY);
        }
    }
}
