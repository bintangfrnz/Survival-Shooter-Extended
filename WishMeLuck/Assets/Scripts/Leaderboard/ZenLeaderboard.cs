using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public static class ZenLeaderboard {
    public const int ZenEntryCount = 3;
 
    public struct ZenScoreEntry {
        public string name;
        public int score;
        public int timer;
 
        public ZenScoreEntry(string name, int score, int timer) {
            this.name = name;
            this.score = score;
            this.timer = timer;
        }
    }
 
    private static List<ZenScoreEntry> zenEntries;
 
    private static List<ZenScoreEntry> zenScoreEntries {
        get {
            if (zenEntries == null) {
                zenEntries = new List<ZenScoreEntry>();
                LoadZenScores();
            }
            return zenEntries;
        }
    }
 
    private const string PlayerPrefsBaseKey = "zen_leaderboard";
 
    private static void SortZenScores() {
        zenEntries.Sort((a, b) => b.score.CompareTo(a.score));
    }
 
    private static void LoadZenScores() {
        zenEntries.Clear();
 
        for (int i = 0; i < ZenEntryCount; ++i) {
            ZenScoreEntry entry;
            entry.name = PlayerPrefs.GetString(PlayerPrefsBaseKey + "[" + i + "].name", "");
            entry.score = PlayerPrefs.GetInt(PlayerPrefsBaseKey + "[" + i + "].score", 0);
            entry.timer = PlayerPrefs.GetInt(PlayerPrefsBaseKey + "[" + i + "].timer", 0);
            zenEntries.Add(entry);
        }
 
        SortZenScores();
    }
 
    private static void SaveZenScores() {
        for (int i = 0; i < ZenEntryCount; ++i) {
            var entry = zenEntries[i];
            PlayerPrefs.SetString(PlayerPrefsBaseKey + "[" + i + "].name", entry.name);
            PlayerPrefs.SetInt(PlayerPrefsBaseKey + "[" + i + "].score", entry.score);
            PlayerPrefs.SetInt(PlayerPrefsBaseKey + "[" + i + "].timer", entry.timer);
        }
    }
 
    public static ZenScoreEntry GetEntry(int index) {
        return zenScoreEntries[index];
    }
 
    public static void Record(string name, int score, int timer) {
        zenScoreEntries.Add(new ZenScoreEntry(name, score, timer));
        SortZenScores();
        zenScoreEntries.RemoveAt(zenScoreEntries.Count - 1);
        SaveZenScores();
    }
}