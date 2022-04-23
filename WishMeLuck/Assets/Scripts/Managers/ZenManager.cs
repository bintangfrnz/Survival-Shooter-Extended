using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ZenManager : MonoBehaviour
{
	public PlayerHealth playerHealth;
	// jarak untuk memastikan musuh spawn di luar layar
	public float bufferDistance = 200f;
    // waktu tiap upgrade weapon
    public float timeBetweenUpgrade = 60f;
    // waktu antar spawn orbs
    public float timeBetweenSpawn = 3f;
    // reference -> timer text
	public TextMeshProUGUI gameTime;
	// reference -> enemies killed text
	public TextMeshProUGUI enemyKilled; 
    // banyak musuh terbunuh
    [HideInInspector]
	public int enemiesKilled = 0;

    
    float timer = 0f;
    [HideInInspector]
    public float gameTimer = 0f;
    float upgradeTimer = 0f;
    // posisi spawn
    Vector3 spawnPosition = Vector3.zero;
    // manager
    EnemyManager enemyManager;
    GUIManager guiManager;

    void Start()
    {
        guiManager = GameObject.FindObjectOfType(typeof(GUIManager)) as GUIManager;;
        enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // jika player sudah mati, stop
		if (playerHealth.currentHealth <= 0f) {
			return;
		}

        timer += Time.deltaTime;
        upgradeTimer += Time.deltaTime;
        gameTimer += Time.deltaTime;

        gameTime.text = SetTimer();
        enemyKilled.text = enemiesKilled.ToString();

        // upgrade weapon
        if (upgradeTimer > timeBetweenUpgrade) {
            guiManager.ShowUpgradeCanvas();
            upgradeTimer = 0;
        }
            

        if (timer > timeBetweenSpawn) {
            // kemunculan sebesar 60% enemies + 3% boss
			// - 50% -> Zom (50:50)
			// - 25% -> Skele (50:50)
            // - 25% -> Bomb (50:50)
            float rand = Random.value;
            if (rand <= 0.6f) {
				if (rand <= 0.3f) {
                    if (rand <= 0.15f)
                        SpawnEnemy(enemyManager.zomBear);
                    else
                        SpawnEnemy(enemyManager.zomBunny);
				}
				else if (rand > 0.3f && rand <= 0.45f) {
                    if (rand <= 0.375f)
                        SpawnEnemy(enemyManager.skeleBear);
                    else
                        SpawnEnemy(enemyManager.skeleBunny);
				}
                else {
                    if (rand <= 0.525)
                        SpawnEnemy(enemyManager.bomBear);
                    else
                        SpawnEnemy(enemyManager.bomBunny);
                }
			} else if (rand > 0.6f && rand <= 0.63f) {
                SpawnEnemy(enemyManager.boss);
            }
        }
    }

    public void playAddEnemyKilledAnim(){
		enemyKilled.GetComponent<Animation>().Play();
	}

    private void SpawnEnemy(GameObject en)
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
            SpawnEnemy(en);
        } else {
            spawnPosition = hit.position;
            spawnPosition.y = 1;

            // akan diulang jika posisi terlihat dalam layar
            Vector3 screenPos = Camera.main.WorldToScreenPoint(spawnPosition);
            if ((screenPos.x > -bufferDistance && screenPos.x < (Screen.width + bufferDistance)) && 
                (screenPos.y > -bufferDistance && screenPos.y < (Screen.height + bufferDistance))) {
                SpawnEnemy(en);
            } else {
                // spawn orb
                GameObject enemy = Instantiate(en, spawnPosition, Quaternion.identity);
            }
        }
    }

    public string SetTimer() {
        int minutes = Mathf.FloorToInt(gameTimer / 60f);
        int seconds = Mathf.FloorToInt(gameTimer - minutes * 60);
        string formattedTime = string.Format("{0:0}:{1:00}", minutes, seconds);
    
        return formattedTime;
    }
}
