using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour {

    // reference -> player's health
    public PlayerHealth playerHealth;
    // waktu sebelum restart
    public float restartDelay = 5f;

    // reference -> animator component
    Animator anim;
    // waktu untuk menghitung waktu restart
    float restartTimer;
    GUIManager guiManager;

    void Awake() {
        // mendapatkan reference
		guiManager = GameObject.FindObjectOfType(typeof(GUIManager)) as GUIManager;
        anim = GameObject.Find("HUDCanvas").GetComponent<Animator>();
    }

    void Update() {
        // darah habis
        if (playerHealth.currentHealth <= 0 && !guiManager.IsGameOver()) {
			// save player preferences
			PlayerPrefs.Save();

            // set trigger game over
            anim.SetTrigger("GameOver");

            // hitung waktu mundur sebelum restart
            restartTimer += Time.deltaTime;

            // hitung muncdur selesai
            if (restartTimer >= restartDelay) {
                // reload scene
				guiManager.ShowGameOverCanvas();
                anim.ResetTrigger("GameOver");
            }
        }
    }
}
