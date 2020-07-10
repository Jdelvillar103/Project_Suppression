using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkills : MonoBehaviour {



    public enum HitBox
    {
        BOX,
        SPHERE
    };

    [System.Serializable]
    public class Skill

    {
        public string name;

        public KeyCode keycode;

        public int damage;
        public float attackRange;
        public float cooldownTime;
        public float cooldownCountdown;

        public Transform attackOrigin;
        public LayerMask enemies;

        public Text cooldownTimer;
        public Image cooldownSlider;

        public bool showDmgHitBox;
        public HitBox hitboxType;
    }

    [SerializeField]
    private Skill[] skills;

    [SerializeField]
    private LayerMask dashCollisionMask;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < skills.Length; i++)
        {
            // used for cooldown timers
            if (skills[i].cooldownCountdown < 0)
                skills[i].cooldownTimer.enabled = false;
            else
            {
                // enables countdown timer
                skills[i].cooldownTimer.enabled = true;
                // fills cooldown slider
                skills[i].cooldownSlider.fillAmount = skills[i].cooldownCountdown / skills[i].cooldownTime;
                
                // if less than 3, display 1 decimal place
                if (skills[i].cooldownCountdown < 3)
                    skills[i].cooldownTimer.text = skills[i].cooldownCountdown.ToString("F1");
                else
                    skills[i].cooldownTimer.text = ((int)skills[i].cooldownCountdown).ToString();
            }

            if (skills[i].cooldownCountdown < 0)
            {
                if (Input.GetKeyDown(skills[i].keycode) && Time.timeScale != 0f)
                {
                    Debug.Log("Player used " + skills[i].name);

                    if (i == 0)
                        Skill1Toggle(skills[i]);
                    else if (i == 1)
                        Skill2Toggle(skills[i]);





                    skills[i].cooldownCountdown = skills[i].cooldownTime;
                }

            }
            else
            {
                skills[i].cooldownCountdown -= Time.deltaTime;
            }
        }

        /*
        if (cooldownCountdown < 0)
        {
            cooldownTimer.enabled = false;
        }
        else
        {
            cooldownTimer.enabled = true;
            cooldownSlider.fillAmount = cooldownCountdown / cooldownTime;
            if (cooldownCountdown < 3)
                cooldownTimer.text = cooldownCountdown.ToString("F1");
            else
                cooldownTimer.text = ((int)cooldownCountdown).ToString();
        }

        if (cooldownCountdown < 0)
        {
            if (Input.GetKeyDown(KeyCode.F) && Time.timeScale != 0f)
            {
                PlayerAnimator animator = GetComponent<PlayerAnimator>();
                animator.SetSpinning();

                FindObjectOfType<AudioManager>().Play("SwordSpin");

                print("Player spin slashes!");

                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackOrigin.position, attackRange, enemies);

                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<EnemyController>().TakeDamage(damage);
                }

                cooldownCountdown = cooldownTime;
            }
        }
        else
        {
            cooldownCountdown -= Time.deltaTime;
        }
        */
    }

    private void Skill1Toggle(Skill skill)
    {
        PlayerController controller = GetComponent<PlayerController>();
        PlayerAnimator animator = GetComponent<PlayerAnimator>();
        animator.SetDashing();

        FindObjectOfType<AudioManager>().Play("SwordDash");

        print("Player dash slashes!");

        // raycasts to check for obstacles
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.right, skill.attackRange, dashCollisionMask);
        float distance = Mathf.Abs(hit.point.x - transform.position.x);

        float attackRange;
        if (hit.collider != null)
            attackRange = distance;
        else
            attackRange = skill.attackRange;

        // overlaps box over enemies to be damaged
        Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(skill.attackOrigin.position, new Vector2(attackRange, 2), 0, skill.enemies);

        // damage enemies
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            try
            {
                enemiesToDamage[i].GetComponent<EnemyController>().TakeDamage(skill.damage);
            }
            catch (System.Exception e)
            {
                enemiesToDamage[i].GetComponent<BossController>().TakeDamage(skill.damage);
            }
        }

        // if collision detected in path
        if (hit.collider != null)
        {
            if (animator.GetFacingRight())
                controller.MovePlayerImmediate(distance - 0.5f);
            else
                controller.MovePlayerImmediate(-distance + 0.5f);
        }
        else
        {
            if (animator.GetFacingRight())
                controller.MovePlayerImmediate(skill.attackRange);
            else
                controller.MovePlayerImmediate(-skill.attackRange);
        }


    }

    private void Skill2Toggle(Skill skill)
    {
        PlayerAnimator animator = GetComponent<PlayerAnimator>();
        animator.SetSpinning();

        FindObjectOfType<AudioManager>().Play("SwordSpin");

        print("Player spin slashes!");

        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(skill.attackOrigin.position, skill.attackRange, skill.enemies);

        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            try
            {
                enemiesToDamage[i].GetComponent<EnemyController>().TakeDamage(skill.damage);
            }
            catch (System.Exception e)
            {
                enemiesToDamage[i].GetComponent<BossController>().TakeDamage(skill.damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i].showDmgHitBox)
            {
                Gizmos.color = Color.yellow;
                if (skills[i].hitboxType == HitBox.BOX)
                    Gizmos.DrawWireCube(skills[i].attackOrigin.position, new Vector3(skills[i].attackRange, 2, 1));
                else if (skills[i].hitboxType == HitBox.SPHERE)
                    Gizmos.DrawWireSphere(skills[i].attackOrigin.position, skills[i].attackRange);
            }

        }
        /*
         * Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackOrigin.position, attackRange);
           */
    }
}
