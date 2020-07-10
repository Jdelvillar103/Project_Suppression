using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour {

    [SerializeField]
    private GameObject respawnPos;
    [SerializeField]
    private int damage = 2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController controller = collision.GetComponent<PlayerController>();

            controller.ResetMoveDistance();

            controller.TakeDamage(damage);

            controller.transform.position = respawnPos.transform.position;
        }
        else if (collision.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }
    }
}
