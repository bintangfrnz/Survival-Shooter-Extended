using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{

    // reference -> wave rank
    public GameObject waveRank1;
    public GameObject waveRank2;
    public GameObject waveRank3;

    // referemce -> zen rank
    public GameObject zenRank1;
    public GameObject zenRank2;
    public GameObject zenRank3;

    // canvas
    Canvas leaderboardCanvas;
    Canvas mainMenuCanvas;

    private void Update()
    {
        ShowRank();
    }

    private void ShowRank()
    {
        // Zen rank
        for (int i = 0; i < ZenLeaderboard.ZenEntryCount; ++i) {
            var entry = ZenLeaderboard.GetEntry(i);
            if (i == 0) {
                zenRank1.transform.Find("PlayerName").GetComponent<TextMeshProUGUI>().text = entry.name;
                zenRank1.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = entry.score.ToString();
                zenRank1.transform.Find("Time").GetComponent<TextMeshProUGUI>().text = FormatTimer(entry.timer);
            } else if (i == 1) {
                zenRank2.transform.Find("PlayerName").GetComponent<TextMeshProUGUI>().text = entry.name;
                zenRank2.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = entry.score.ToString();
                zenRank2.transform.Find("Time").GetComponent<TextMeshProUGUI>().text = FormatTimer(entry.timer);
            } else {
                zenRank3.transform.Find("PlayerName").GetComponent<TextMeshProUGUI>().text = entry.name;
                zenRank3.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = entry.score.ToString();
                zenRank3.transform.Find("Time").GetComponent<TextMeshProUGUI>().text = FormatTimer(entry.timer);
            }

        }

        // Wave rank
        for (int i = 0; i < WaveLeaderboard.WaveEntryCount; ++i) {
            var entry = WaveLeaderboard.GetEntry(i);
            if (i == 0) {
                waveRank1.transform.Find("PlayerName").GetComponent<TextMeshProUGUI>().text = entry.name;
                waveRank1.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = entry.score.ToString();
                waveRank1.transform.Find("Wave").GetComponent<TextMeshProUGUI>().text = entry.wave.ToString();
            } else if (i == 1) {
                waveRank2.transform.Find("PlayerName").GetComponent<TextMeshProUGUI>().text = entry.name;
                waveRank2.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = entry.score.ToString();
                waveRank2.transform.Find("Wave").GetComponent<TextMeshProUGUI>().text = entry.wave.ToString();
            } else {
                waveRank3.transform.Find("PlayerName").GetComponent<TextMeshProUGUI>().text = entry.name;
                waveRank3.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = entry.score.ToString();
                waveRank3.transform.Find("Wave").GetComponent<TextMeshProUGUI>().text = entry.wave.ToString();
            }

        }
    }

    private string FormatTimer(int timer) {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        string formattedTime = string.Format("{0:0}:{1:00}", minutes, seconds);
    
        return formattedTime;
    }
}
