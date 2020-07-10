using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class FireBall : MonoBehaviour
{

    [SerializeField]
    private float speed;
    [SerializeField]
    private int damage = 20;
    [SerializeField]
    private LayerMask whatIsEnemies;
    [SerializeField]
    private float distance;

    [SerializeField]
    private Vector3 direction;
    [SerializeField]
    private Vector3 moveDistance;

    [SerializeField]
    private float projectileLifeTime = 10;
    private float startLifeTime;

    private Controller2D controller2D;

    // Use this for initialization
    void Start()
    {
        controller2D = GetComponent<Controller2D>();
        startLifeTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startLifeTime > projectileLifeTime)
        {
            Destroy(gameObject);
        }
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
                index = controller2D.FindVerticalTagIndex("Enemy");

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
                index = controller2D.FindHorizontalTagIndex("Enemy");

                // if found, set hitInfo to RaycastHit2D element
                if (index != -1)
                {
                    hitInfo = controller2D.GetHorizontalRaycastHitAtIndex(index);
                }
            }

            // if tag found and detected, deal damage
            if (index != -1)
            {
                hitInfo.collider.GetComponent<EnemyController>().TakeDamage(damage);
            }
            Debug.Log("FireBall");
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
