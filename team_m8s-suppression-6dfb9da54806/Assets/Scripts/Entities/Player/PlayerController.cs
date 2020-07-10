using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //

public class PlayerController : Entity {

    [SerializeField]
    private int maxHearts = 3;
    [SerializeField]
    private int heartChunks;
    private int maxHeartChunks;
    private int jumpctr = 0;
    private bool facingRight;

    [SerializeField]
    private float invincibilityTime;
    private bool invincible;    // if true, player is invincible and flickers

    private Vector2 input;

    [SerializeField]
    private Transform bossSkip;

	// Use this for initialization
    protected override void Start () {
        base.Start();

        maxHeartChunks = maxHearts * 2;
        heartChunks = maxHeartChunks;

        invincible = false;
	}
	
	// Update is called once per frame
	private void Update () {
        // updates maxHeart chunks
        maxHeartChunks = maxHearts * 2;
        
        //Debug.Log(Input.mousePosition.x);

        // if heartChunks exceeds maxHeart chunks, readjust
        if (heartChunks > maxHeartChunks)
        {
            heartChunks = maxHeartChunks;
        }

        // retrieves input from player
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        // if player presses space and grounded, jump
        if (Input.GetKeyDown(KeyCode.Space) && jumpctr < 1)
        {
            jumpctr++;
            FindObjectOfType<AudioManager>().Play("JumpSoundEffect");
            moveDistance.y = maxJumpVelocity;
        }
        if (controller2D.collisions.bottom)
        {
            jumpctr = 0;
        }
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (moveDistance.y > minJumpVelocity)
            {
                moveDistance.y = minJumpVelocity;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.F1))
        {
            transform.position = bossSkip.position;
        }

        controller2D.Move(moveDistance * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (controller2D.collisions.top || controller2D.collisions.bottom)
        {
            moveDistance.y = 0;
        }

        moveDistance.x = moveSpeed * input.x;
        moveDistance.y += gravity * Time.deltaTime;
        isFacingRight();

    }

    // player takes damage in number of heart chunks
    public void TakeDamage(int damage)
    {
        if (!invincible)
        {
            heartChunks -= damage;

            Debug.Log("Player received " + damage + " damage! Health remaining: " + heartChunks);

            FindObjectOfType<AudioManager>().Play("PlayerHit");
            Debug.Log("Player received " + damage + " damage! Health remaining: " + heartChunks);

            if (heartChunks <= 0)
            {
                Debug.Log("Player struck down!");
                FindObjectOfType<AudioManager>().Play("Oof");
                FindObjectOfType<GameOver>().GameOverScreen();
                FindObjectOfType<AudioManager>().Play("GameOver");
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            StartCoroutine(SetInvincibilityTimer());
        }
    }

    public void HealDamage(int heal)
    {
        heartChunks += heal;

        if (heartChunks > maxHeartChunks)
            heartChunks = maxHeartChunks;
    }

    public bool AtMaxHealth()
    {
        if (heartChunks == maxHeartChunks)
            return true;

        return false;
    }

    // sets invincibility timer
    private IEnumerator SetInvincibilityTimer()
    {
       Debug.Log("Player invincible!");
        invincible = true;

        int blinkCount = 20;
        float blinkRate = 0.1f;

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        for (int i = 0; i < blinkCount; i++)
        {
            if (i % 2 == 0)
                sprite.enabled = true;
            else
                sprite.enabled = false;
            yield return new WaitForSeconds(blinkRate);
        }

        sprite.enabled = true;
        Debug.Log("Player vulnerable!");
        invincible = false;
    }

    public void MovePlayerImmediate(float x)
    {
        Vector2 moveVector = new Vector2(x, 0);

        controller2D.Move(moveVector);
    }

    public void ResetMoveDistance()
    {
        moveDistance.x = 0;
        moveDistance.y = 0;
    }

    public bool isFacingRight()
    {
        if ((input.x < 0 && facingRight) || (input.x > 0 && !facingRight))
        {            
            facingRight = !facingRight;
        }
        return facingRight;
    }

    public void setMoveDistance(Vector2 newSpeed)
    {
        moveDistance = newSpeed;
    }
    public int GetMaxHearts()
    {
        return maxHearts;
    }

    public int GetHeartChunks()
    {
        return heartChunks;
    }

    public int GetMaxHeartChunks()
    {
        return maxHeartChunks;
    }

    public Vector2 GetInput()
    {
        return input;
    }
}
