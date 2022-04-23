using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using TMPro;

public class ScoreManager : MonoBehaviour
{
	// reference -> score text
	public TextMeshProUGUI scoreText;
	// score player
	[HideInInspector]
	public int scoreNumber;       

	void Awake() {
		// reset score
		scoreNumber = 0;
	}

	void Update() {
		// update score
		scoreText.text = scoreNumber.ToString();
	}

	public void AddScore(int num) {
		scoreNumber += num;
		scoreText.GetComponent<Animation>().Stop();
		scoreText.GetComponent<Animation>().Play();
	}

	public int GetScore() {
		return scoreNumber;
	}
}