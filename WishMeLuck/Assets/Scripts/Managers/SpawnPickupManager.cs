using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPickupManager : MonoBehaviour
{
    // reference -> darah player
	public PlayerHealth playerHealth;
    // waktu antar spawn orbs
    public float timeBetweenSpawn = 3f;
    float timer;
    // posisi spawn
    Vector3 spawnPosition = Vector3.zero;
    PickupManager pickupManager;
    
    void Awake()
    {
        pickupManager = GameObject.Find("PickupManager").GetComponent<PickupManager>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > timeBetweenSpawn) {
            // kemunculan sebesar 40%
			// - 50% -> health
			// - 25% -> speed
            // - 25% -> power
            float rand = Random.value;
            if (rand <= 0.4f) {
				if (rand <= 0.2f) {
                    SpawnOrbs(pickupManager.healthPickup);
				}
				else if (rand > 0.2f && rand <= 0.3f) {
                    SpawnOrbs(pickupManager.speedPickup);
				}
                else {
                    SpawnOrbs(pickupManager.powerPickup);
                }
			}
        }
    }

    private void SpawnOrbs(Pickup orb)
    {
        // jika player sudah mati, berhenti
		if (playerHealth.currentHealth <= 0f) {
			return;
		}
        timer = 0f;

        // mendapatkan random posisi
        Vector3 randomPosition = Random.insideUnitSphere * 35;
        randomPosition.y = 0;

        // mendapatkan posisi terdekat di nav mesh dari randomPosition
        // ulang jika ga valid
        UnityEngine.AI.NavMeshHit hit;
        if (!UnityEngine.AI.NavMesh.SamplePosition(randomPosition, out hit, 5, 1)) {
            SpawnOrbs(orb);
        } else {
            spawnPosition = hit.position;
            spawnPosition.y = 1;
            // spawn orb
            Pickup pickup = Instantiate(orb, spawnPosition, Quaternion.identity);
        }
    }
}
