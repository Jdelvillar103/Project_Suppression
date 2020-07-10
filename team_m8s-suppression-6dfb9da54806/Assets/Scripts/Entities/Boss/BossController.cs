using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : Entity {

    [SerializeField]
    private int maxHealth = 1000;
    [SerializeField]
    private int currentHealth;

    [SerializeField]
    private bool facingRight;

    [SerializeField]
    private Transform targetTransform;

    [SerializeField]
    private Image healthUI;
    [SerializeField]
    private Image healthBar;

    private bool phaseTwo = false; //Checks it is phase 2 because it would at each frame start the phase 2 music over and over

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
        GameObject healthUIObject = GameObject.Find("UI/GameUI/BossHealthBG");
        GameObject healthBarObject = GameObject.Find("UI/GameUI/BossHealthBG/HealthBar");

        healthUI = healthUIObject.GetComponent<Image>();
        healthBar = healthBarObject.GetComponent<Image>();

        currentHealth = maxHealth;

        healthUI.gameObject.SetActive(true);
        
        healthBar.fillAmount = (float)currentHealth / maxHealth;
    }


    void Update()
    {
        // checks health
        CheckHealth();

        // fills health bar
        healthBar.fillAmount = (float)currentHealth / maxHealth;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        // flips direction depending on position of player
        Vector3 distance = targetTransform.position - transform.position;
        Vector3 direction = distance.normalized;
        if ((facingRight && direction.x < 0) || (!facingRight && direction.x > 0))
        {
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
            facingRight = !facingRight;
        }

        controller2D.Move(moveDistance);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*
         * Behavior controlled by state machines and
         * behavior scripts.
         */

        moveDistance.y += gravity * Time.fixedDeltaTime;
    }

    private void CheckHealth()
    {
        if (currentHealth <= 0)
        {
            moveSpeed = 0;
            Destroy(gameObject);
            FindObjectOfType<AudioManager>().Play("VictoryFanfare");
            FindObjectOfType<AudioManager>().Play("VictorySong");
            FindObjectOfType<Victory>().victoryScreen();
        }

        if ((float)currentHealth / maxHealth <= 0.5f)
        {
            Animator anim = gameObject.GetComponent<Animator>();
            anim.SetBool("stageTwo", true);
            FindObjectOfType<AudioManager>().Stop("SceneMusic");
            if(!phaseTwo)
            {
                FindObjectOfType<AudioManager>().Play("MainTheme");
                phaseTwo = true;
            }
            
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        FindObjectOfType<AudioManager>().Play("BossGrunt");
        Debug.Log("Boss receives " + damage + " damage. Health: " + currentHealth + "/" + maxHealth);
    }
}
