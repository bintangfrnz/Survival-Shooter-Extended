using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomAttack : MonoBehaviour
{
    // waktu antara serangan
	public float timeBetweenAttacks = 0.1f;  
	// attack power
	public int attackDamage = 10;               
	   
	// reference -> player
	GameObject player;       
	PlayerHealth playerHealth;

	// reference -> darah musuh
	EnemyHealth enemyHealth;

	// status jarak player dalam range serangan (trigger collider)
	bool playerInRange;   
	// waktu untuk menghitung serangan selanjutnya
	float timer;                               
	
	void Awake() {
		// mendapatkan reference
		player = GameObject.FindGameObjectWithTag("Player");
		playerHealth = player.GetComponent<PlayerHealth>();
		enemyHealth = GetComponent<EnemyHealth>();
	}

	void OnTriggerEnter(Collider other) {
		// collision -> player dalam range serangan
		if (other.gameObject == player) {
			playerInRange = true;
			// reaction time
			timer = 0.2f;
		}
	}
	
	void Update() {
		// add timer
		timer += Time.deltaTime;
		
		// ... serang
		if (timer >= timeBetweenAttacks
			&& playerInRange
			&& enemyHealth.currentHealth > 0
			&& playerHealth.currentHealth > 0) {
			Attack();
		}
	}
	
	
	void Attack() {
		// reset timer
		timer = 0f;
		
		// mengurangi darah player
		playerHealth.TakeDamage(attackDamage);

        // meledak (bomb)
        enemyHealth.currentHealth = 0;
        enemyHealth.Death();
	}
}
