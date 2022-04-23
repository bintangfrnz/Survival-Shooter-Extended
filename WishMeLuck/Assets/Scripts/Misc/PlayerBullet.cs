using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerBullet : MonoBehaviour
{
	// reference -> partikel sistem
	public ParticleSystem normalTrailParticles;
	public ParticleSystem bounceTrailParticles;
	public ParticleSystem pierceTrailParticles;
	public ParticleSystem ImpactParticles;
	// kecepatan peluru (get from player shooting)
	public float speed;
	// lama peluru di udara (get from player shooting)
	public float life;
	// damage peluru (get from player shooting)
	public int damage;
	// efek peluru (get from player shooting)
	public bool piercing;
	public bool bouncing;
	// warna awal peluru (get from player shooting)
	public Color bulletStartColor;
	// reference -> audio bouncing
	public AudioClip bounceSound;
	// reference -> audio hitting
	public AudioClip hitSound;

	// vektor kecepatan
	Vector3 velocity;
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

		// set particle colors.
		var main = normalTrailParticles.main;
		main.startColor = bulletStartColor;

		main = bounceTrailParticles.main;
		main.startColor = bulletStartColor;

		main = pierceTrailParticles.main;
		main.startColor = bulletStartColor;

		main = ImpactParticles.main;
		main.startColor = bulletStartColor;

		normalTrailParticles.gameObject.SetActive(true);
		if (bouncing) {
			bounceTrailParticles.gameObject.SetActive(true);
			normalTrailParticles.gameObject.SetActive(false);
		}
		if (piercing) {
			pierceTrailParticles.gameObject.SetActive(true);
			normalTrailParticles.gameObject.SetActive(false);
		}
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
		velocity.y = 0;
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

				// kena environment atau musuh
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
		if (lastHit.point == hit.point || lastHit.collider == hit.collider)
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

			if (bouncing) {
				Vector3 reflect = Vector3.Reflect(direction, hit.normal);
				transform.forward = reflect;

				// memainkan audio bouncing
				bulletAudio.clip = bounceSound;
				bulletAudio.pitch = Random.Range(0.8f, 1.2f);
				bulletAudio.Play();
			} else {
				hasHit = true;

				// memainkan audio hit
				bulletAudio.clip = hitSound;
				bulletAudio.volume = 0.5f;
				bulletAudio.pitch = Random.Range(1.2f, 1.3f);
				bulletAudio.Play();
				DelayedDestroy();
			}
        }

		// kena enemy
        if (hit.transform.tag == "Enemy") {
			ImpactParticles.transform.position = hit.point;
			ImpactParticles.transform.rotation = rotation;
			ImpactParticles.Play();

			// mendapatkan reference -> enemy's health
			EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
			
			if (enemyHealth != null) {
				enemyHealth.TakeDamage(damage, hit.point);
			}

			// jika ada efek piercing maka menembus musuh (belum didestroy)
			if (!piercing) {
            	hasHit = true;
				DelayedDestroy();
			}

			// memainkan audio hit
			bulletAudio.clip = hitSound;
			bulletAudio.volume = 0.5f;
			bulletAudio.pitch = Random.Range(1.2f, 1.3f);
			bulletAudio.Play();
        }
	}

	// fading out bullet & destroy
	void Dissipate() {
		var normalTrailParticlesEmission = normalTrailParticles.emission.enabled;
		normalTrailParticlesEmission = false;
		normalTrailParticles.transform.parent = null;

		var normalTrailParticlesMain = normalTrailParticles.main;
		Destroy(normalTrailParticles.gameObject, normalTrailParticlesMain.duration);

		if (bouncing) {
			var bounceParticlesEmission = bounceTrailParticles.emission.enabled;
			bounceParticlesEmission = false;
			bounceTrailParticles.transform.parent = null;

			var bounceTrailParticlesMain = bounceTrailParticles.main;
			Destroy(bounceTrailParticles.gameObject, bounceTrailParticlesMain.duration);
		}
		if (piercing) {
			var pierceTrailParticlesEmission = pierceTrailParticles.emission.enabled;
			pierceTrailParticlesEmission = false;
			pierceTrailParticles.transform.parent = null;

			var pierceTrailParticlesMain = pierceTrailParticles.main;
			Destroy(pierceTrailParticles.gameObject, pierceTrailParticlesMain.duration);
		}

		Destroy(gameObject);
	}

	void DelayedDestroy() {
		normalTrailParticles.gameObject.SetActive(false);
		if (bouncing)
			bounceTrailParticles.gameObject.SetActive(false);
		if (piercing)
			pierceTrailParticles.gameObject.SetActive(false);
		Destroy(gameObject, hitSound.length);
	}
}