using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : MonoBehaviour {

    [SerializeField]
    private float dashSpeed;

    private float dashTime;

    [SerializeField]
    private float startDashTime;
    
    private PlayerController delo;
    private int direction;
    private float dashMove;
    private bool doDash;
    

	// Use this for initialization
	void Start () {
        delo = GetComponent<PlayerController>();
        dashTime = startDashTime;

	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            DashMove();
        }
		
	}
    void DashMove()
    {
        if (direction == 0)
        {
            //follow mouse direction

            if (delo.isFacingRight())
            { direction = 1;
                Debug.Log("Right");
            }
            else
            { direction = 2;
                Debug.Log("left");
            }

        }
        else
        {
            if (dashTime <= 0)
            {
                direction = 0;
                dashTime = startDashTime;
                dashMove = 0;
            }
            else
            {
                if (dashTime <= 0)
                {
                    dashTime -= Time.deltaTime;
                    delo.ResetMoveDistance();
                }
                else
                { dashTime -= Time.deltaTime; }

                if (direction == 1)
                {
                    delo.setMoveDistance(new Vector2((delo.GetMoveSpeed() * dashSpeed), 0));
                }
                else if (direction == 2)
                {
                    delo.setMoveDistance(new Vector2((delo.GetMoveSpeed() * dashSpeed)/**(-1)*/, 0));
                }
            }

        }
    }
}
