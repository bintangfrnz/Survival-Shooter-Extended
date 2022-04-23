using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
	// darah musuh di awal game
	public int startingHealth = 100;  
	// darah musuh saat ini
	[HideInInspector]
	public int currentHealth; 
	// score yang diperoleh jika membunuh musuh ini
	public int scoreValue = 10; 
	// reference -> enemy death clip
	public AudioClip deathClip;    
	// reference -> enemy burn clip
	public AudioClip burnClip;  
	// reference -> burning particle system
	public ParticleSystem deathParticles;  
	// enemy health bar
	public Slider healthBarSlider;
	// reference -> enemy eyes
	public GameObject eye;
	// apakah enemy bomber
	public bool isBomber = false;
	// enemy health slider
	Slider sliderInstance;

	// cek apakah enemy sudah mati
	bool isDead;
	// cek apakah enemy burning
	bool isBurning = false;

	// rim color -> warna shader mata musuh
	Color rimColor;
    // rim power untuk menghasilkan efek yang lebih baik
    float rimPower;
    // nilai untuk menghilangkan shader
    float cutoffValue = 0f;

	// reference -> animator
	Animator anim;
	// reference -> enemy audio source
	AudioSource enemyAudio;
	// komponen lain di enemy
	CapsuleCollider capsuleCollider;   
	SkinnedMeshRenderer myRenderer;
	// reference -> enemy health bar canvas
	GameObject enemyHealthbarCanvas;
	// managers
	ZenManager zenManager;
	WaveManager waveManager;
	ScoreManager scoreManager;
	PickupManager pickupManager;

	void Awake() {
		// mendapatkan reference
		anim = GetComponent<Animator>();
		enemyAudio = GetComponent<AudioSource>();
		capsuleCollider = GetComponent<CapsuleCollider>();
		myRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

		// enemy health bar canvas
		enemyHealthbarCanvas = GameObject.Find("EnemyHealthbarsCanvas");

		// manager
		zenManager = GameObject.Find("ZenManager").GetComponent<ZenManager>();
		waveManager = GameObject.Find("WaveManager").GetComponent<WaveManager>();
		scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
		pickupManager = GameObject.Find("PickupManager").GetComponent<PickupManager>();
	}

	void Start() {
		// initialize health
		currentHealth = startingHealth;

		// inisiasi enemy health slider (tidak terlihat)
		sliderInstance = Instantiate(healthBarSlider, gameObject.transform.position, Quaternion.identity) as Slider;
		sliderInstance.gameObject.transform.SetParent(enemyHealthbarCanvas.transform, false);
		sliderInstance.GetComponent<Healthbar>().enemy = gameObject;
		sliderInstance.gameObject.SetActive(false);

		// get rim color & rim color dari shader material
		rimColor = myRenderer.materials[0].GetColor("_RimColor");
        rimPower = myRenderer.materials[0].GetFloat("_RimPower");
    }

	void Update() {
		// menghilangkan material perlahan jika terbakar
		if (isBurning) {
			cutoffValue = Mathf.Lerp(cutoffValue, 1f, 1.3f * Time.deltaTime);
			myRenderer.materials[0].SetFloat("_Cutoff", cutoffValue);
			myRenderer.materials[1].SetFloat("_Cutoff", 1);
		}
	}

	public void TakeDamage(int amount, Vector3 hitPoint) {
		// memberikan efek menyala pada tubuh musuh saat terkena tembakan
        StopCoroutine("Ishit"); // efek sebelumnya hilang saat terkena serangan baru
		StartCoroutine("Ishit");

		// jika sudah mati sudah tidak menerima damage
		if (isDead)
			return;

		// memberikan efek knock back
		GetComponent<Rigidbody>().AddForceAtPosition(transform.forward * -250, hitPoint);
		
		// mengurangi darah
		currentHealth -= amount;

		// update enemy health bar
		sliderInstance.gameObject.SetActive(true);
		int sliderValue = (int) Mathf.Round(((float)currentHealth / (float)startingHealth) * 100);
		sliderInstance.value = sliderValue;
		
		// darah < 0
		if (currentHealth <= 0) {
			Death();
		}
	}

	IEnumerator Ishit() {

		// ubah rim color & rim power
		Color newColor = new Color(10, 0, 0, 0);
        float newPower = 0.5f;
		myRenderer.materials[0].SetColor("_RimColor", newColor);
        myRenderer.materials[0].SetFloat("_RimPower", newPower);

        float time = 0.5f; // lama efek glow up badan
		float elapsedTime = 0;

		// memberikan efek fading pada badan yang glow up
		while (elapsedTime < time) {
			newColor = Color.Lerp(newColor, rimColor, elapsedTime / time);
			myRenderer.materials[0].SetColor("_RimColor", newColor);
            newPower = Mathf.Lerp(newPower, rimPower, elapsedTime / time);
            myRenderer.materials[0].SetFloat("_RimPower", newPower);
            elapsedTime += Time.deltaTime;
			yield return null;
		}

		// kembali ke keadaans semula
        myRenderer.materials[0].SetColor("_RimColor", rimColor);
        myRenderer.materials[0].SetFloat("_RimPower", rimPower);
    }

	public void Death() {
		// ubah status musuh
		isDead = true;

		// mentrigger animasi dead
		if (!isBomber)
			anim.SetTrigger("Dead");
		
		// mengganti audio menjadi death clip & play
		enemyAudio.clip = deathClip;
		enemyAudio.Play();

		// disable nav mesh agent
		if (GetComponent<UnityEngine.AI.NavMeshAgent>()) {
			GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
		}
		
		// set rigid body kinematic untuk proses translasi-y (sinking)
		GetComponent<Rigidbody>().isKinematic = true;

		// menambah score
		scoreManager.AddScore(scoreValue);

		// mengurangi jumlah enemies
		waveManager.enemiesAlive--;
		waveManager.playReduceEnemyAliveAnim ();
		// menambah jumlah killed
		zenManager.enemiesKilled++;
		zenManager.playAddEnemyKilledAnim ();


		// turn on collider sehingga peluru menembus
		capsuleCollider.isTrigger = true;

		// sinking
		StartCoroutine(StartSinking());
		// remove enemy health bar
		Destroy(sliderInstance.gameObject);
	}

	IEnumerator StartSinking() {
		if (!isBomber)
			yield return new WaitForSeconds(2);
		else {
			GetComponent<Light>().enabled = false;
			yield return null;
		}

		isBurning = true;

		// play death particles
		deathParticles.Play();

		enemyAudio.clip = burnClip;
		enemyAudio.Play();

		// memunculkan 2 mata (memberi efek mata terlepas)
		for (int i = 0; i < 2; i++) {
			GameObject instantiatedEye = Instantiate(eye, transform.position + new Vector3(0, 0.3f, 0), transform.rotation) as GameObject;
			instantiatedEye.GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3 (Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f)));
		}

		// spawn enchance pickup (by chance)
		SpawnPickup();

		// remove enemy object
		Destroy(gameObject, 3f);
	}

	void SpawnPickup() {
		// posisi pada tempat mati musuh
		Vector3 spawnPosition = transform.position + new Vector3(0, 0.3f, 0);

		// kemunculan sebesar 25%
		// - 40% -> floating
		// - 35% -> bouncing
		// - 25% -> piercing
		float random_val = Random.value;
		if (random_val <= 0.25f) {
			if (random_val <= 0.1f) {
				// floating
				Instantiate (pickupManager.floatTimePickup, spawnPosition, transform.rotation);
			} else if (random_val > 0.1f && random_val <= 0.1875f) {
				// bouncing
				Instantiate (pickupManager.bouncePickup, spawnPosition, transform.rotation);
			} else {
				// piercing
				Instantiate (pickupManager.piercePickup, spawnPosition, transform.rotation);
			}
		}
	}

	public bool IsDeath() {
		return isDead;
	}
}
