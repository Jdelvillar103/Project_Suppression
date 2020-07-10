using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {

    public enum SpawnState
    {
        SPAWNING,
        WAITING,
        COUNTING
    };

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    private int nextWave = 0;

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f;
    public float waveCountdown = 0f;

    public SpawnState state = SpawnState.COUNTING;

    public bool playerInZone;

    // Use this for initialization
    void Start()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.Log("ERROR: No spawn points referenced!");
        }

        waveCountdown = timeBetweenWaves;

        playerInZone = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (playerInZone)
        {
            // if time to spawn
            if (waveCountdown <= 0)
            {
                if (state != SpawnState.SPAWNING && nextWave < waves.Length)
                {
                    // start spawning wave
                    StartCoroutine(SpawnWave(waves[nextWave]));
                }
            }
            else
            {
                waveCountdown -= Time.deltaTime;
            }
        }
        
    }

    IEnumerator SpawnWave (Wave _wave)
    {
        Debug.Log("Spawning Wave: " + _wave.name);
        state = SpawnState.SPAWNING;
        
        // spawn a bunch of things
        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;
        nextWave++;

        yield break;
    }

    void SpawnEnemy(Transform _enemy)
    {
        // spawn enemy
        Debug.Log("Spawning enemy: " + _enemy.name);

        Transform _sp;

        if (_enemy.name == "Boss")
            _sp = spawnPoints[2];
        else
            _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Instantiate(_enemy, _sp.position, _sp.rotation);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FindObjectOfType<AudioManager>().Play("Warning");
            playerInZone = true;
        }
            
    }

}
