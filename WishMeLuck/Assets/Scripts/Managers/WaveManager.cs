using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class WaveManager : MonoBehaviour {
	
	// reference -> darah player
	public PlayerHealth playerHealth;
	// jarak untuk memastikan musuh spawn di luar layar
	public float bufferDistance = 200f;
	// waktu antar wave
	public float timeBetweenWaves = 5f;
	// waktu saat player tidak membunuh siapapun
	public float timeSinceNoEnemiesKilled = 30.0f;
    // waktu antar spawn di suatu wave
    public float spawnTime = 0.5f;
    // gelombang pertama
	public int startingWave = 1;
    // tingkat kesulitan (loop setiap 9 wave)
	public int startingDifficulty = 1;
	// reference -> wave text
	public TextMeshProUGUI number;
	// reference -> enemies left text
	public TextMeshProUGUI numberEnemies; 
    // banyak musuh tersisa
	[HideInInspector]
	public int enemiesAlive = 0;

    // class yang ngedefine berapa entries dalam satu wave
    [System.Serializable]
    public class Wave {
		public int weight;
        public Entry[] entries;

        // class yang ngedefine musuh apa dan berapa banyak dalam satu entry
        [System.Serializable]
        public class Entry {
            // tipe musuh
            public GameObject enemy;
            // banyak musuh
            public int count;
            // banyak yang sudah di spawn
            [System.NonSerialized]
            public int spawned;
        }
    }

    public Wave[] waves;
    Vector3 spawnPosition = Vector3.zero;
	Wave currentWave;
	int waveNumber;
	[HideInInspector]
	public int actualWave;
	int spawnedThisWave = 0;
	int totalToSpawnForWave;
	bool shouldSpawn = false;
	int difficulty;
	int enemiesInLastFrame;
	float timer;
	float noKillTimer;
	float delayTime;
	GUIManager guiManager;

	void Awake()
	{
		// mendapatkan reference
		guiManager = GameObject.FindObjectOfType(typeof(GUIManager)) as GUIManager;
	}

	void Start()
	{
		// Jika mau mulai dari wave ke-n dan tingkat kesulitan lebih
		waveNumber = (startingWave > 0) ? startingWave - 1 : 0;
		difficulty = startingDifficulty;
		delayTime = 0;

		// mulai next wave
		StartCoroutine(PrepareNextWave());
	}
	
	void Update()
	{
		// jika player sudah mati, stop
		if (playerHealth.currentHealth <= 0f) {
			return;
		}
		
		// batasan ketika sedang prepare next wave
		if (!shouldSpawn) {
			return;
        }

		// update time
		timer += Time.deltaTime;

		// mulai wave baru hanya jika seluruh entry sudah dispawn dan
		// semua musuh sudah dibunuh atau player tidak membunuh siapapun selama 30 detik
		if (spawnedThisWave == totalToSpawnForWave
			&& (enemiesAlive == 0 || noKillTimer > timeSinceNoEnemiesKilled))
		{
			if ((waveNumber + ((difficulty - 1) * waves.Length)) % 3 == 0) {
				if (delayTime > 2) {
					guiManager.ShowUpgradeCanvas();
					if (delayTime > 4) {
						StartCoroutine(PrepareNextWave());
						delayTime = 0;
					}
				}
				else {
					delayTime += Time.deltaTime;
					return;
				}
			}
			StartCoroutine(PrepareNextWave());
			return;
		}

        // saat waktu melewati waktu antar spawn (timer akan direset difungsi spawn)
		if (timer >= spawnTime) {
			foreach (Wave.Entry entry in currentWave.entries) {
				if (entry.spawned < (entry.count * difficulty)) {
					SpawnEnemy(entry);
				}
			}
		}

		numberEnemies.text = enemiesAlive.ToString();

		// update no kill timer
		if (enemiesInLastFrame == enemiesAlive) {
			noKillTimer += Time.deltaTime;
		} else {
			enemiesInLastFrame = enemiesAlive;
			noKillTimer = 0;
		}
	}

	public void playReduceEnemyAliveAnim(){
		numberEnemies.GetComponent<Animation>().Play();
	}

	IEnumerator PrepareNextWave()
	{
		shouldSpawn = false;
		yield return new WaitForSeconds(timeBetweenWaves);

		if (waveNumber < waves.Length) {
			currentWave = waves[waveNumber];
		} 
		// setelah melewati wave terakhir, wave kembali ke awal dan tingkat kesulitan bertambah
		else {
			difficulty++;

			// reset jumlah musuh yang sudah di spawn
			foreach (Wave.Entry entry in waves [waves.Length - 1].entries) {
				entry.spawned = 0;
			}

			// balik ke entry pertama
			waveNumber = 0;
			currentWave = waves[waveNumber];
		}

        // banyak musuh pada suatu entri * difficulty
        totalToSpawnForWave = 0;
		foreach (Wave.Entry entry in currentWave.entries) {
			totalToSpawnForWave += (entry.count * difficulty);
		}

		spawnedThisWave = 0;
		shouldSpawn = true;

		waveNumber++;

		actualWave = (waveNumber + ((difficulty - 1) * waves.Length));
		number.text = actualWave.ToString();

		// play animator di wave number text
		number.GetComponent<Animation>().Play();
	}

	void SpawnEnemy(Wave.Entry entry) {
		// jika player sudah mati, stop
		if (playerHealth.currentHealth <= 0f) {
			return;
		}
		timer = 0f;
		
		// mendapatkan random posisi
		Vector3 randomPosition = Random.insideUnitSphere * 35;
		randomPosition.y = 0;
		
		// mendapatkan posisi terdekat di nav mesh dari randomPosition
        // akan diulang jika gak valid
		UnityEngine.AI.NavMeshHit hit;
		if (!UnityEngine.AI.NavMesh.SamplePosition(randomPosition, out hit, 5, 1)) {
			return;
		}
		spawnPosition = hit.position;
        spawnPosition.y = 1;

		// akan diulang jika posisi terlihat dalam layar
		Vector3 screenPos = Camera.main.WorldToScreenPoint(spawnPosition);
		if ((screenPos.x > -bufferDistance && screenPos.x < (Screen.width + bufferDistance)) && 
		    (screenPos.y > -bufferDistance && screenPos.y < (Screen.height + bufferDistance))) 
		{
			return;
		}

		// spawn enemy
		GameObject enemy = Instantiate(entry.enemy, spawnPosition, Quaternion.identity);

		// tingkat kesulitan bertambah, attribut musuh bertambah
		enemy.GetComponent<EnemyHealth>().startingHealth *= difficulty;
		enemy.GetComponent<EnemyHealth>().scoreValue *= difficulty;
		
		entry.spawned++;
		spawnedThisWave++;
		enemiesAlive++;
		numberEnemies.text = enemiesAlive.ToString();
	}
}
