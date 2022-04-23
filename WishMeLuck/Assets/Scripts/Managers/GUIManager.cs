using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GUIManager : MonoBehaviour
{
    // reference -> minimap
    public GameObject minimap;
    // reference -> public managers
    public GameObject gameOverManager;
    public GameObject spawnEnemyManager;
    // reference -> canvases
    public GameObject waveModeCanvas;
    public GameObject zenModeCanvas;
    public GameObject waveResetConfirm;
    public GameObject zenResetConfirm;
    public GameObject hudCanvas;
    public GameObject settingCanvas;
    public GameObject upgradeCanvas;
    public GameObject gameOverCanvas;
    public GameObject mainMenuCanvas;
    public GameObject leaderboardCanvas;
    public GameObject pausedCanvas;
    // reference -> input field
    public TMP_InputField playerNameInput;
    // reference -> dropdown mode
    public TMPro.TMP_Dropdown gameModeInput;

    // game mode
    string gameMode = "-";
    string playerName = "-";

    // game state
    bool inGame = false;
    bool onUpgrade = false;
    bool onPause = false;
    bool gameOver = false;
    bool enterFirstGame = false;
    bool isMinimapOn = false;

    // reference -> managers
    ZenManager zenManager;
    WaveManager waveManager;
    ScoreManager scoreManager;
    LeaderboardManager leaderboardManager;

    private void Awake()
    {
        zenManager = GameObject.FindObjectOfType(typeof(ZenManager)) as ZenManager;
        waveManager = GameObject.FindObjectOfType(typeof(WaveManager)) as WaveManager;
        scoreManager = GameObject.FindObjectOfType(typeof(ScoreManager)) as ScoreManager;
        leaderboardManager = GameObject.FindObjectOfType(typeof(LeaderboardManager)) as LeaderboardManager;
    }

    private void Start()
    {
        Time.timeScale = 0;
    }

    private void Update()
    {
        // tekan esc untuk pause game
        if (Input.GetKeyDown(KeyCode.Escape)) {
            // hanya berlaku ketika game sedang dimulai dan tidak di menu upgrade dan tidak saat game over
            if (inGame && !onUpgrade && !gameOver)
                ResumeGame();
        }

        // tekan m untuk membuka minimap
		if (Input.GetButtonDown("MiniMap")) {
			// hanya berlaku ketika game sedang dimulai & tidak di menu upgrade & tidak di menu pause & tidak saat game over
			if (inGame && !onUpgrade && !onPause && !gameOver) {
                minimap.SetActive(!minimap.activeInHierarchy);
                isMinimapOn = !isMinimapOn;
            }
		}
    }

    // WEAPON UPGRADE
    public void ShowUpgradeCanvas() {
        Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
        onUpgrade = true;

        if (isMinimapOn)
            minimap.SetActive(!minimap.activeInHierarchy);

        hudCanvas.SetActive(!hudCanvas.activeInHierarchy);
        upgradeCanvas.SetActive(!upgradeCanvas.activeInHierarchy);
    }

    public void UpgradeBullet()
    {
        Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
        onUpgrade = false;
        GameObject.Find("Player").GetComponentInChildren<PlayerShooting>().AddBullet();

        if (isMinimapOn)
            minimap.SetActive(!minimap.activeInHierarchy);

        upgradeCanvas.SetActive(!upgradeCanvas.activeInHierarchy);
        hudCanvas.SetActive(!hudCanvas.activeInHierarchy);
    }

    public void UpgradeSpeed()
    {
        Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
        onUpgrade = false;
        GameObject.Find("Player").GetComponentInChildren<PlayerShooting>().AddShootSpeed();

        if (isMinimapOn)
            minimap.SetActive(!minimap.activeInHierarchy);
        
        upgradeCanvas.SetActive(!upgradeCanvas.activeInHierarchy);
        hudCanvas.SetActive(!hudCanvas.activeInHierarchy);
    }

    // LEADERBOARD
    public void MainMenuToLeaderboard()
    {
        mainMenuCanvas.SetActive(!mainMenuCanvas.activeInHierarchy);
        leaderboardCanvas.SetActive(!leaderboardCanvas.activeInHierarchy);
    }

    public void LeaderboardToMainMenu()
    {
        leaderboardCanvas.SetActive(!leaderboardCanvas.activeInHierarchy);
        mainMenuCanvas.SetActive(!mainMenuCanvas.activeInHierarchy);
    }

    public void ShowResetWaveConfirm()
    {
        leaderboardCanvas.SetActive(!leaderboardCanvas.activeInHierarchy);
        waveResetConfirm.SetActive(!waveResetConfirm.activeInHierarchy);
    }
    public void ResetWaveAndExit()
    {
        PlayerPrefs.DeleteKey("wave_leaderboard[0].name");
        PlayerPrefs.DeleteKey("wave_leaderboard[0].score");
        PlayerPrefs.DeleteKey("wave_leaderboard[0].wave");
        PlayerPrefs.DeleteKey("wave_leaderboard[1].name");
        PlayerPrefs.DeleteKey("wave_leaderboard[1].score");
        PlayerPrefs.DeleteKey("wave_leaderboard[1].wave");
        PlayerPrefs.DeleteKey("wave_leaderboard[2].name");
        PlayerPrefs.DeleteKey("wave_leaderboard[2].score");
        PlayerPrefs.DeleteKey("wave_leaderboard[2].wave");
        PlayerPrefs.Save();
        ExitGame();
    }

    public void ShowResetZenConfirm()
    {
        leaderboardCanvas.SetActive(!leaderboardCanvas.activeInHierarchy);
        zenResetConfirm.SetActive(!zenResetConfirm.activeInHierarchy);
    }
    public void ResetZenAndExit()
    {
        PlayerPrefs.DeleteKey("zen_leaderboard[0].name");
        PlayerPrefs.DeleteKey("zen_leaderboard[0].score");
        PlayerPrefs.DeleteKey("zen_leaderboard[0].timer");
        PlayerPrefs.DeleteKey("zen_leaderboard[1].name");
        PlayerPrefs.DeleteKey("zen_leaderboard[1].score");
        PlayerPrefs.DeleteKey("zen_leaderboard[1].timer");
        PlayerPrefs.DeleteKey("zen_leaderboard[2].name");
        PlayerPrefs.DeleteKey("zen_leaderboard[2].score");
        PlayerPrefs.DeleteKey("zen_leaderboard[2].timer");
        PlayerPrefs.Save();
        ExitGame();
    }

    // SETTING
    public void MainMenuToSetting()
    {
        mainMenuCanvas.SetActive(!mainMenuCanvas.activeInHierarchy);
        settingCanvas.SetActive(!settingCanvas.activeInHierarchy);
    }

    public void SettingToMainMenu()
    {
        settingCanvas.SetActive(!settingCanvas.activeInHierarchy);
        mainMenuCanvas.SetActive(!mainMenuCanvas.activeInHierarchy);
    }

    // PAUSE PANEL
    public void ResumeGame()
    {
        Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
        onPause = !onPause;

        if (isMinimapOn)
            minimap.SetActive(!minimap.activeInHierarchy);

        pausedCanvas.SetActive(!pausedCanvas.activeInHierarchy);
        hudCanvas.SetActive(!hudCanvas.activeInHierarchy);
    }

    public void MainMenu()
    {
        mainMenuCanvas.SetActive(!mainMenuCanvas.activeInHierarchy);
        pausedCanvas.SetActive(!pausedCanvas.activeInHierarchy);
        SceneManager.LoadScene("Level 01");

        inGame = false;
        enterFirstGame = false;
    }
    // GAME OVER
    public void ShowGameOverCanvas()
    {
        hudCanvas.SetActive(!hudCanvas.activeInHierarchy);
        gameOverCanvas.SetActive(!gameOverCanvas.activeInHierarchy);
        gameOver = true;

        minimap.SetActive(false);
        isMinimapOn = false;

        if (gameMode == "Zen mode") {
            GameObject GOZenPanel = gameOverCanvas.transform.Find("GOZenPanel").gameObject;
            GOZenPanel.SetActive(true);

            GOZenPanel.transform.Find("GOPlayer/GOPlayerName").GetComponent<TextMeshProUGUI>().text = playerName;
            GOZenPanel.transform.Find("GOLastScore/GOLastScoreNumber").GetComponent<TextMeshProUGUI>().text = scoreManager.scoreNumber.ToString();
            GOZenPanel.transform.Find("GOTimeSurvive/GOTimeSurviveNumber").GetComponent<TextMeshProUGUI>().text = zenManager.SetTimer();

            ZenLeaderboard.Record(playerName, scoreManager.scoreNumber, (int)zenManager.gameTimer);
        }
        else {
            GameObject GOWavePanel = gameOverCanvas.transform.Find("GOWavePanel").gameObject;
            GOWavePanel.SetActive(true);

            GOWavePanel.transform.Find("GOPlayer/GOPlayerName").GetComponent<TextMeshProUGUI>().text = playerName;
            GOWavePanel.transform.Find("GOLastScore/GOLastScoreNumber").GetComponent<TextMeshProUGUI>().text = scoreManager.scoreNumber.ToString();
            GOWavePanel.transform.Find("GOLastWave/GOLastWaveNumber").GetComponent<TextMeshProUGUI>().text = waveManager.actualWave.ToString();

            WaveLeaderboard.Record(playerName, scoreManager.scoreNumber, waveManager.actualWave);
        }   
    }

    public void PlayAgain()
    {
        inGame = false;
        SceneManager.LoadScene("Level 01");
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    // MAIN MENU
    public void StartGame()
    {
        if (!enterFirstGame) {
            playerName = playerNameInput.text;
            gameMode = (gameModeInput.value == 0) ? "Zen mode" : "Wave mode";
            enterFirstGame = true;
        }
        
        Debug.Log("Nama: " + playerName);
        Debug.Log("Mode: " + gameMode);

        Time.timeScale = 1;
        inGame = true;
        gameOver = false;

        // Zen Mode
        if (gameMode == "Zen mode")
        {
            zenManager.enabled = true;
            zenModeCanvas.SetActive(true);

            var managers = spawnEnemyManager.GetComponents<SpawnEnemyManager>();
            foreach (SpawnEnemyManager manager in managers)
                manager.enabled = true;
        }
        // Wave Mode
        else {
            waveManager.enabled = true;
            waveModeCanvas.SetActive(true);
        }

        mainMenuCanvas.SetActive(!mainMenuCanvas.activeInHierarchy);
        hudCanvas.SetActive(!hudCanvas.activeInHierarchy);
        gameOverManager.SetActive(true);
    }

    public void ExitGame()
    {
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
}
