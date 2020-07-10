using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(PlayerController))]
public class PlayerHealth : MonoBehaviour {

    private int maxHearts;
    private int heartChunks;

    [SerializeField]
    private Image[] emptyHearts;
    [SerializeField]
    private Image[] hearts;

    private PlayerController playerController;

	// Use this for initialization
	void Start () {
        playerController = GetComponent<PlayerController>();

        maxHearts = playerController.GetMaxHearts();
        heartChunks = playerController.GetHeartChunks();
	}
	
	// Update is called once per frame
	void Update () {
        maxHearts = playerController.GetMaxHearts();
        heartChunks = playerController.GetHeartChunks();

        for (int i = 0; i < emptyHearts.Length; i++)
        {
            if (i < maxHearts)
            {
                emptyHearts[i].enabled = true;
            }
            else
            {
                emptyHearts[i].enabled = false;
            }
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < heartChunks)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
	}
}
