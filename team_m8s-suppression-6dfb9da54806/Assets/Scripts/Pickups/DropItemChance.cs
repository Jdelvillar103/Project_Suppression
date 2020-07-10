using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  This script is used for item drop chances. Attached to objects or enemies. 
 */
public class DropItemChance : MonoBehaviour {

    [SerializeField]
    private GameObject[] gameObjects;   // holds GameObjects to spawn
    [SerializeField]
    private int[] chances;    // represents chances to spawn (25 -> 25%)
    [SerializeField]
    private int[] amounts;      // amount to spawn

    public void SpawnGameObjects()
    {
        int randNum;    // holds random integer
        for (int i = 0; i < chances.Length; i++)
        {
            randNum = Random.Range(0, 101);     // generates random number from 0 to 100 (inclusive)

            // if randNum in range of chances then spawn GameObject
            if (randNum <= chances[i])
            {
                for (int j = 0; j < amounts[i]; j++)
                {
                    Instantiate(gameObjects[i], transform.position, Quaternion.identity);
                }
            }
        }
    }

}
