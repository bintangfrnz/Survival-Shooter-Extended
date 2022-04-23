using UnityEngine;

public class Healthbar : MonoBehaviour {
	// reference -> musuh
	public GameObject enemy;

	void Update() {
		if (enemy == null) {
			return;
		}
		// memastikan health bar bergerak mengikuti posisi musuh
		Vector3 screenPos = Camera.main.WorldToScreenPoint(enemy.transform.position);
		gameObject.transform.position = screenPos + new Vector3(0, 60, 0);
	}
}
