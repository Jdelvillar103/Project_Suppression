using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedWeapon : MonoBehaviour {

    [SerializeField]
    private EnemyController enemyController;
    [SerializeField]
    private Transform targetTransform;
    [SerializeField]
    private GameObject enemyProjectile;
    [SerializeField]
    private Transform point;

    [SerializeField]
    private float timeBtwShots;
    private float nextShotTime;
    [SerializeField]
    private float shotDelayTime;

    [SerializeField]
    private float chargeTime = 2;

    private Vector2 displacementFromTarget;
    private float distanceToTarget;

	// Use this for initialization
	void Start () {
        targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
        enemyController = GetComponentInParent<EnemyController>();
    }
	
	// Update is called once per frame
	void Update () {

        displacementFromTarget = targetTransform.position - transform.position; // finds displacement of target
        distanceToTarget = displacementFromTarget.magnitude;    // finds distance from target

        if (distanceToTarget < 10f)
        {
            if (nextShotTime - Time.time <= chargeTime)
            {
                Animator animator = GetComponentInParent<Animator>();
                animator.SetBool("inRange", true);
            }

            if (Time.time > nextShotTime)
            {
                FindObjectOfType<AudioManager>().Play("LaserGunShot");
                GameObject newStunBolt = Instantiate(enemyProjectile, point.position, Quaternion.identity);
                newStunBolt.transform.localScale *= Mathf.Sign(transform.localScale.x);

                Vector3 difference = targetTransform.position - transform.position; //recieves direction
                float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg; //get degrees of rotation

                // finds normalized vector using degrees
                float dirX = Mathf.Cos(rotZ * Mathf.Deg2Rad);
                float dirY = Mathf.Sin(rotZ * Mathf.Deg2Rad);

                // makes new vector3 normalized vector
                Vector3 direction = new Vector3(dirX, dirY, 0);

                // grabs StunBolt component and sets direction of StunBolt
                EnemyProjectile newEnemyProjectile = newStunBolt.GetComponent<EnemyProjectile>();
                newEnemyProjectile.SetDirection(direction);

                SpriteRenderer sprite = newStunBolt.GetComponentInChildren<SpriteRenderer>();
                sprite.transform.localEulerAngles = new Vector3(0, 0, rotZ);

                nextShotTime = timeBtwShots + Time.time;
                
                StartCoroutine(AnimatorSetFire(.5f));
            }
        }

	}

    private IEnumerator AnimatorSetFire(float animationLength)
    {
        Animator animator = GetComponentInParent<Animator>();

        animator.SetBool("inRange", false);

        animator.SetBool("firing", true);

        yield return new WaitForSeconds(animationLength);

        animator.SetBool("firing", false);
    }
}
