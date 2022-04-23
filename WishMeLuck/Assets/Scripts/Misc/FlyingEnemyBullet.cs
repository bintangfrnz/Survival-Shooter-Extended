using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlyingEnemyBullet : MonoBehaviour
{
	// reference -> partikel sistem
	public ParticleSystem normalTrailParticles;
	public ParticleSystem ImpactParticles;
	// damage peluru (get from enemy attack)
	public int damage;
	// kecepatan peluru (get from enemy attack)
	public float speed;
	// lama peluru di udara (get from enemy attack)
	public float life;
	// warna awal peluru (get from enemy attack)
	public Color bulletColor;
	// reference -> audio hitting
	public AudioClip hitSound;

	// vektor kecepatan
	Vector3 velocity;
    Vector3 force;
	// vektor posisi
	Vector3 newPos;
	Vector3 oldPos;
	// vektor arah
	Vector3 direction;
	// kena sesuatu atau tidak
	bool hasHit = false;
	// objek apa yang tertembak
	RaycastHit lastHit;
	// reference -> bullet shoot
	AudioSource bulletAudio;  
	float timer;

	void Awake() {
		bulletAudio = GetComponent<AudioSource> ();
	}

	void Start() {
		newPos = transform.position;
		oldPos = newPos;

		// set particle colors
		var main = normalTrailParticles.main;
		main.startColor = bulletColor;

		main = ImpactParticles.main;
		main.startColor = bulletColor;

		normalTrailParticles.gameObject.SetActive(true);
	}

	void Update() {
		// jika sudah kena sesuatu, stop
		if (hasHit) {
			return;
		}
		timer += Time.deltaTime;

		// jika peluru tidak kena apa-apa, stop
		if (timer >= life) {
			Dissipate();
		}

        velocity = transform.forward;
		// velocity.y = 0;
		velocity = velocity.normalized * speed;

		// bergerak
		newPos += velocity * Time.deltaTime;
	
		// cek jika hit sesuatu
		direction = newPos - oldPos;
		float distance = direction.magnitude;

		if (distance > 0) {
            RaycastHit[] hits = Physics.RaycastAll(oldPos, direction, distance);

		    // find the first valid hit
		    for (int i = 0; i < hits.Length; i++) {
		        RaycastHit hit = hits[i];

				if (ShouldIgnoreHit(hit)) {
					continue;
				}

				// kena environment atau player
				OnHit(hit);
				lastHit = hit;

				if (hasHit) {
					newPos = hit.point;
					break;
				}
		    }
		}

		oldPos = transform.position;
		transform.position = newPos;
	}

	bool ShouldIgnoreHit (RaycastHit hit) {
		if (lastHit.point == hit.point || lastHit.collider == hit.collider || hit.collider.tag == "Enemy")
			return true;
		return false;
	}

	void OnHit(RaycastHit hit) {
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

		// kena environment
        if (hit.transform.tag == "Environment") {
			newPos = hit.point;
			ImpactParticles.transform.position = hit.point;
			ImpactParticles.transform.rotation = rotation;
			ImpactParticles.Play();

			hasHit = true;

			// memainkan audio hit
			bulletAudio.clip = hitSound;
			bulletAudio.volume = 0.5f;
			bulletAudio.pitch = Random.Range(0.6f, 0.8f);
			bulletAudio.Play();
			DelayedDestroy();
        }

		// kena player
        if (hit.transform.tag == "Player") {
			ImpactParticles.transform.position = hit.point;
			ImpactParticles.transform.rotation = rotation;
			ImpactParticles.Play();

			// mendapatkan reference -> player's health
			PlayerHealth playerHealth = hit.collider.GetComponent<PlayerHealth>();
			
			if (playerHealth != null) {
				if (playerHealth.currentHealth > 0)
				// ... the enemy should take damage.
				playerHealth.TakeDamage(damage);
			}
    		hasHit = true;
			DelayedDestroy();

			// memainkan audio hit
			bulletAudio.clip = hitSound;
			bulletAudio.volume = 0.5f;
			bulletAudio.pitch = Random.Range(0.6f, 0.8f);
			bulletAudio.Play();
        }
	}

	// fading out bullet & destroy
	void Dissipate() {
		var normalTrailParticlesEmissions = normalTrailParticles.emission.enabled;
		normalTrailParticlesEmissions = false;
		normalTrailParticles.transform.parent = null;

		var main = normalTrailParticles.main;

		Destroy(normalTrailParticles.gameObject, main.duration);
		Destroy(gameObject);
	}

	void DelayedDestroy() {
		normalTrailParticles.gameObject.SetActive(false);
		Destroy(gameObject, hitSound.length);
	}
}