using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyManager : MonoBehaviour
{
    // reference -> darah player
    public PlayerHealth playerHealth;
    // reference -> enemy
    public GameObject enemy;
    // waktu spawning musuh
    public float spawnTime = 3f;
    public Transform[] spawnPoints;

    void Start()
    {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    private void Spawn ()
    {
        if (playerHealth.currentHealth <= 0f) {
            return;
        }

        int spawnPointIndex = Random.Range (0, spawnPoints.Length);
        Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }
}
