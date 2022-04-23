using UnityEngine;
using System.Collections;

public class HellephantAttack : MonoBehaviour {

	// waktu diantara serangan
	public float timeBetweenAttacks = 3f;  
	// waktu diantara tembakan
	public float timeBetweenBullets = 0.1f;
	public float angleBetweenBullets = 10f;
	// banyak peluru tiap jenis tembakan
	public int bulletsPerVolley = 5;
	public int numberOfBullets = 36;
	// reference -> bullet prefabs
	public GameObject bullet;
	// 3x attack untuk 1x spesial attack
	public int attacksPerSpecialAttack = 3;
	// reference -> audio shoot
	public AudioClip shootClip;
	public AudioClip specialClip;
	   
	// reference -> player
	GameObject player;           
	// reference -> enemy's health
	EnemyHealth enemyHealth; 
	// apakah player dalam range
	bool playerInRange;   
	// reference -> animator
	Animator anim;
	
	
	float attackTimer;
	float bulletTimer;
	int attackCount;
	int bulletCount;

	HellephantMovement helleMovement;
	float floatHeight = 3f;
	float landingTime;
	bool usedSpecial = false;
	bool landed = false;
	// reference -> enemy audio
	AudioSource enemyAudio;  
	
	void Awake() {
		// mendapatkan reference
		player = GameObject.FindGameObjectWithTag("Player");
		enemyHealth = GetComponent<EnemyHealth>();
		enemyAudio = GetComponent<AudioSource>();
		anim = GetComponent<Animator>();
		helleMovement = GetComponent<HellephantMovement>();
	}

	void Start() {
		// memastikan spawn pertama kali ada di udara
		transform.position = new Vector3(transform.position.x, floatHeight, transform.position.z);
	}

	void Update() {
		attackTimer += Time.deltaTime;
		if (attackTimer > timeBetweenAttacks
			&& enemyHealth.currentHealth > 0) {
			Attack();
		}
	}
	
	void Attack() {
		bulletTimer += Time.deltaTime;

		if (attackCount < attacksPerSpecialAttack) {
			if (bulletTimer > timeBetweenBullets && bulletCount < bulletsPerVolley) {
				Vector3 relativePos = player.transform.position - transform.position;
				Quaternion rotation = Quaternion.LookRotation(relativePos);
				Quaternion rot = rotation * Quaternion.AngleAxis(Random.Range(-5.0f, 5.0f), Vector3.up) * Quaternion.AngleAxis(Random.Range(-5.0f, 5.0f), Vector3.right);
				Instantiate(bullet, transform.position + new Vector3(0, 0.5f, 0), rot);

				// mainkan sound shooting
				enemyAudio.clip = shootClip;
				enemyAudio.Play();

				// reset timer
				bulletTimer = 0f;
				bulletCount++;

				// peluru dalam sekali serangan sudah keluar semua
				if (bulletCount == (bulletsPerVolley)) {
					bulletCount = 0;
					attackTimer = 0;
					attackCount++;
				}
			}
		}
		else {
			SpecialAttack();
		}
	}

	void SpecialAttack() {
		// mendarat turun
		if (!landed) {
			anim.SetBool("Landing", true);
			helleMovement.shouldMove = false;
			landingTime += Time.deltaTime * 5f;
			transform.position = new Vector3(transform.position.x, Mathf.Lerp(floatHeight, 0, landingTime), transform.position.z);
		}
		// terbang naik
		else {
			anim.SetBool("Landing", false);
			helleMovement.shouldMove = true;
			landingTime += Time.deltaTime * 2f;
			transform.position = new Vector3(transform.position.x, Mathf.Lerp(0, floatHeight, landingTime), transform.position.z);
		}

		// menembakkan peluru ke semua arah saat di tanah
		if (transform.position.y == 0) {
			landed = true;
			if (!usedSpecial) {
				for (int i = 0; i < numberOfBullets; i++) {
					// spread bullet
					float angle = i * angleBetweenBullets - ((angleBetweenBullets / 2) * (numberOfBullets - 1));
					Quaternion rot = transform.rotation * Quaternion.AngleAxis(angle, Vector3.up);
					Instantiate(bullet, transform.position + new Vector3(0, 0.5f, 0), rot);
				}

				// mainkan sound special
				enemyAudio.clip = specialClip;
				enemyAudio.Play();

				usedSpecial = true;
				// reset attack timer
				attackTimer = 0;
				landingTime = 0;
			}
		}

		if (transform.position.y == floatHeight) {
			attackCount = 0;
			landed = false;
			usedSpecial = false;
			landingTime = 0;
		}
	}
}