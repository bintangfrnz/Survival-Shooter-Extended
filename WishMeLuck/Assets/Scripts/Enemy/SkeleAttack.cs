using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeleAttack : MonoBehaviour
{
    // reference -> warna peluru
    public Color bulletColor;
    // tipe serangan
    public enum ShootType {Consecutive, Spread};
    public ShootType shootType = ShootType.Consecutive;
    // waktu diantara serangan
	public float timeBetweenAttacks = 3f;  
    // waktu diantara tembakan
	public float timeBetweenBullets = 0.33f;
	public float angleBetweenBullets = 10f;
    // banyak peluru tiap jenis tembakan
	public int numberOfConsecutiveBullets = 5;
	public int numberOfSpreadBullets = 5;
    // power 1 tembakan
    public int damagePerShot = 10;
    // kecepatan peluru
	public float shotSpeed = 30.0f;
	// lama peluru di udara (tidak menggunakan jarak)
	public float lifeOfBullet = 0.5f;
    // reference -> bullet prefabs
	public GameObject bullet;
    GroundEnemyBullet cBullet;
    // reference -> audio shoot
	public AudioClip consecutiveShootClip;
	public AudioClip spreadShootClip;

    // reference -> player
	GameObject player;
    // reference -> darah player
	PlayerHealth playerHealth; 
	// reference -> enemy's health
	EnemyHealth enemyHealth;
    // reference -> enemy's movement
	SkeleMovement skeleMovement; 
	// apakah player dalam range
	bool playerInRange;   
	// reference -> animator
	Animator anim;

    float attackTimer = 0f;
	float bulletTimer = 0f;
	int bulletCount = 0;
	// reference -> enemy audio
	AudioSource enemyAudio; 

    private void Awake()
    {
        // mendapatkan reference
		player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.transform.GetComponent<PlayerHealth>();
		enemyHealth = GetComponent<EnemyHealth>();
        skeleMovement = GetComponent<SkeleMovement>();
		anim = GetComponent<Animator>();
		enemyAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
		attackTimer += Time.deltaTime;

        if (playerHealth.currentHealth > 0 && skeleMovement.IsFoundPlayer()) {
            if (attackTimer > timeBetweenAttacks && enemyHealth.currentHealth > 0) {
                Attack();
            }
        }
    }

    private void Attack()
    {
        switch (shootType) {
            case ShootType.Consecutive:
                bulletTimer += Time.deltaTime;

                if (bulletTimer > timeBetweenBullets && bulletCount < numberOfConsecutiveBullets) {
                    // consecutive bullet
                    Quaternion rot = transform.rotation * Quaternion.AngleAxis(0, Vector3.up);
                    GameObject instantiatedBullet = Instantiate(bullet, transform.position + new Vector3(0, 0.5f, 0), rot) as GameObject;

                    // detail setiap bullet
                    cBullet = instantiatedBullet.GetComponent<GroundEnemyBullet>();
                    cBullet.damage = damagePerShot;
                    cBullet.speed = shotSpeed;
                    cBullet.life = lifeOfBullet;
                    cBullet.bulletColor = bulletColor;

                    // play consecutiveShot sound
                    enemyAudio.clip = consecutiveShootClip;
                    enemyAudio.Play();
    
                    // reset timer
                    bulletTimer = 0f;
                    bulletCount++;

                    if (bulletCount == numberOfConsecutiveBullets) {
                        bulletCount = 0;
                        attackTimer = 0;
                    }
                }
                break;

            case ShootType.Spread:
                for (int i = 0; i < numberOfSpreadBullets; i++) {
					// spread bullet
					float angle = i * angleBetweenBullets - ((angleBetweenBullets / 2) * (numberOfSpreadBullets - 1));
					Quaternion rot = transform.rotation * Quaternion.AngleAxis(angle, Vector3.up);
					GameObject instantiatedBullet = Instantiate(bullet, transform.position + new Vector3(0, 0.5f, 0), rot) as GameObject;

                    // detail setiap bullet
                    cBullet = instantiatedBullet.GetComponent<GroundEnemyBullet>();
                    cBullet.damage = damagePerShot;
                    cBullet.speed = shotSpeed;
                    cBullet.life = lifeOfBullet;
                    cBullet.bulletColor = bulletColor;
				}

				// play spreadShot sound
				enemyAudio.clip = spreadShootClip;
				enemyAudio.Play();

                attackTimer = 0;
                break;
        }
    }
}
