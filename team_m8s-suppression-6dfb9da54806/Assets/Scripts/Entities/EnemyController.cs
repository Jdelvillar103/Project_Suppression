using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DropItemChance))]
public class EnemyController : Entity {

    [SerializeField]
    private int maxHealth = 20;
    [SerializeField]
    private int health;
    private float dazedTime = 0.5f;
    [SerializeField]
    private float startDazedTime;

    private Vector2 displacementFromTarget;
    private float distanceToTarget;

    [SerializeField]
    private Transform targetTransform;

    [SerializeField]
    private bool facingRight;

    private DropItemChance itemDrops;

    [SerializeField]
    private bool suspended = false;

    [SerializeField]
    private GameObject deathParticles;

    [SerializeField]
    private Image healthBar;
    [SerializeField]
    private bool showHealthBar;
    private float healthBarTimer;

    // Use this for initialization
    protected override void Start () {
        base.Start();

        suspended = false;

        targetTransform = GameObject.FindGameObjectWithTag("Player").transform;

        itemDrops = GetComponent<DropItemChance>();

        health = maxHealth;
        startDazedTime = 0;
        
        healthBar.fillAmount = (float)health / maxHealth;
    }
	
	// Update is called once per frame
	private void Update () {
        CheckHealth();
        
        healthBar.fillAmount = (float)health / maxHealth;

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        if (Time.time - startDazedTime <= dazedTime || suspended)
        {
            moveDistance.x = 0;
        }

        Animator animator = GetComponent<Animator>();
        animator.SetFloat("horizontalSpeed", Mathf.Abs(moveDistance.x));

        controller2D.Move(moveDistance * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        // checks health of enemy
        CheckHealth();
        
        // if there is a collision on top or bottom
        if (controller2D.collisions.top || controller2D.collisions.bottom)
        {
            moveDistance.y = 0;
        }

        displacementFromTarget = targetTransform.position - transform.position;
        distanceToTarget = displacementFromTarget.magnitude;

        if (distanceToTarget > 2.0f && distanceToTarget < 20)
        {
            if (Mathf.Abs(displacementFromTarget.x) > 2.0f)
            {
                moveDistance.x = moveSpeed * Mathf.Sign(displacementFromTarget.x);

                float sign = Mathf.Sign(moveDistance.x);
                if ((sign < 0 && facingRight) || (sign > 0 && !facingRight))
                {
                    Vector3 newScale = transform.localScale;
                    newScale.x *= -1;
                    transform.localScale = newScale;
                    facingRight = !facingRight;
                }
            }
        }
        else if (distanceToTarget >= 10)
        {
            moveDistance.x = 0;
        }

        moveDistance.y += gravity * Time.deltaTime;
    }

    private void CheckHealth()
    {
        if (health <= 0)
        {
            itemDrops.SpawnGameObjects();
            moveSpeed = 0;
            Debug.Log("Enemy struck down!");
            if (gameObject.name.Contains("ImpKnight"))
                FindObjectOfType<AudioManager>().Play("EnemyDeathSound");
            if (gameObject.name.Contains("Droid_Mk1"))
                FindObjectOfType<AudioManager>().Play("DroidDeath");
            if (gameObject.name.Contains("WillOWisp"))
                FindObjectOfType<AudioManager>().Play("WispDeath");
            Instantiate(deathParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        Debug.Log("Enemy receives " + damage + " damage. Health remaining: " + health);
        if(health > 0 && gameObject.name.Contains("Droid_Mk1"))
            FindObjectOfType<AudioManager>().Play("DroidDamage");
        if (health > 0 && gameObject.name.Contains("ImpKnight"))
            FindObjectOfType<AudioManager>().Play("EnemyHit");
        if (health > 0 && gameObject.name.Contains("WillOWisp"))
            FindObjectOfType<AudioManager>().Play("WispDamage");

        CheckHealth();

        startDazedTime = Time.time;
    }

    public void SetSuspendState(bool state)
    {
        suspended = state;
    }

    public bool GetSuspendedState()
    {
        return suspended;
    }

    public bool GetFacingRight()
    {
        return facingRight;
    }
}
