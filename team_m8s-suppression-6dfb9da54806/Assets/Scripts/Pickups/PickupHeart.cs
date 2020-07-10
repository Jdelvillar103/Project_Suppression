using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class PickupHeart : Entity {

    [SerializeField]
    private int healAmount = 2;

	// Use this for initialization
	protected override void Start () {
        base.Start();

        controller2D = GetComponent<Controller2D>();
	}

    private void Update()
    {
        controller2D.Move(moveDistance);
    }

    // Update is called once per frame
    void FixedUpdate () {

        if (controller2D.collisions.top || controller2D.collisions.bottom)
            moveDistance.y = 0;

        moveDistance.y += gravity * Time.deltaTime;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController controller = collision.GetComponent<PlayerController>();

            if (!controller.AtMaxHealth())
            {
                controller.HealDamage(healAmount);
                FindObjectOfType<AudioManager>().Play("PickUpHeart");
                Destroy(gameObject);
            }
        }
    }
}
