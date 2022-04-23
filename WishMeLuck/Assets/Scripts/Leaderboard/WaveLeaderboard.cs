using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WaveLeaderboard {
    public const int WaveEntryCount = 3;
 
    public struct WaveScoreEntry {
        public string name;
        public int score;
        public int wave;
 
        public WaveScoreEntry(string name, int score, int wave) {
            this.name = name;
            this.score = score;
            this.wave = wave;
        }
    }
 
    private static List<WaveScoreEntry> waveEntries;
 
    private static List<WaveScoreEntry> waveScoreEntries {
        get {
            if (waveEntries == null) {
                waveEntries = new List<WaveScoreEntry>();
                LoadWaveScores();
            }
            return waveEntries;
        }
    }
 
    private const string PlayerPrefsBaseKey = "wave_leaderboard";
 
    private static void SortWaveScores() {
        waveEntries.Sort((a, b) => b.score.CompareTo(a.score));
    }
 
    private static void LoadWaveScores() {
        waveEntries.Clear();
 
        for (int i = 0; i < WaveEntryCount; ++i) {
            WaveScoreEntry entry;
            entry.name = PlayerPrefs.GetString(PlayerPrefsBaseKey + "[" + i + "].name", "");
            entry.score = PlayerPrefs.GetInt(PlayerPrefsBaseKey + "[" + i + "].score", 0);
            entry.wave = PlayerPrefs.GetInt(PlayerPrefsBaseKey + "[" + i + "].wave", 0);
            waveEntries.Add(entry);
        }
 
        SortWaveScores();
    }
 
    private static void SaveWaveScores() {
        for (int i = 0; i < WaveEntryCount; ++i) {
            var entry = waveEntries[i];
            PlayerPrefs.SetString(PlayerPrefsBaseKey + "[" + i + "].name", entry.name);
            PlayerPrefs.SetInt(PlayerPrefsBaseKey + "[" + i + "].score", entry.score);
            PlayerPrefs.SetInt(PlayerPrefsBaseKey + "[" + i + "].wave", entry.wave);
        }
    }
 
    public static WaveScoreEntry GetEntry(int index) {
        return waveScoreEntries[index];
    }
 
    public static void Record(string name, int score, int wave) {
        waveScoreEntries.Add(new WaveScoreEntry(name, score, wave));
        SortWaveScores();
        waveScoreEntries.RemoveAt(waveScoreEntries.Count - 1);
        SaveWaveScores();
    }
}