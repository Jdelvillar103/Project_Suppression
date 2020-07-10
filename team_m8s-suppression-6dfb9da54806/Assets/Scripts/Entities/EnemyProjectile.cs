using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Controller2D))]
public class EnemyProjectile : MonoBehaviour {

    [SerializeField]
    private float speed;
    [SerializeField]
    private int damage = 1;   
    [SerializeField]
    private LayerMask whatIsEnemies;

    [SerializeField]
    private Vector3 direction;
    [SerializeField]
    private Vector3 moveDistance;

    [SerializeField]
    private float projectileLifeTime = 10;
    private float startLifeTime;

    [SerializeField]
    private bool facingRight = true;

    private SpriteRenderer sprite;
    private Controller2D controller2D;

    [SerializeField]
    private GameObject hitParticles;

	// Use this for initialization
	void Start () {
        sprite = GetComponentInChildren<SpriteRenderer>();
        controller2D = GetComponent<Controller2D>();
        startLifeTime = Time.time;
    }

    // Update is called once per frame
    void Update () {

        // destroy object if object exceeds lifetime
        if (Time.time - startLifeTime > projectileLifeTime)
            Destroy(gameObject);

        // flips object depending on direction
        if ((moveDistance.x < 0 && facingRight) || (moveDistance.x > 0 && !facingRight))
        {
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
            facingRight = !facingRight;
        }

        // rotates sprite based on scale and position
        float rotZ;
        if (facingRight)
            rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        else
            rotZ = Mathf.Atan2(direction.y, -direction.x) * Mathf.Rad2Deg;
        sprite.transform.localEulerAngles = new Vector3(0, 0, rotZ);
    }
    
    private void FixedUpdate()
    {
        // if collision detected, destroy projectile
        if (controller2D.GetAnyCollision())
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up);
            int index = -1;

            // if collision detected on top or bottom
            if (controller2D.collisions.top || controller2D.collisions.bottom)
            {
                // find vertical RaycastHit2D index with enemy tag
                index = controller2D.FindVerticalTagIndex("Player");
                
                // if found, set hitInfo to RaycastHit2D element
                if (index != -1)
                {
                    hitInfo = controller2D.GetVerticalRaycastHitAtIndex(index);
                }
            }

            // if collision detected on left or right
            else
            {
                // find horizontal RaycastHit2D with enemy tag
                index = controller2D.FindHorizontalTagIndex("Player");

                // if found, set hitInfo to RaycastHit2D element
                if (index != -1)
                {
                    hitInfo = controller2D.GetHorizontalRaycastHitAtIndex(index);
                }
            }

            // if tag found and detected, deal damage
            if (index != -1)
            {
                hitInfo.collider.GetComponent<PlayerController>().TakeDamage(damage);
            }

            Instantiate(hitParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        moveDistance = direction * speed;

        controller2D.Move(moveDistance * Time.deltaTime);
    }
    
    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

}
