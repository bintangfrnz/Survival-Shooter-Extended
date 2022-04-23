using UnityEngine;
using System.Collections;

public class Eye : MonoBehaviour {

	// reference -> death particles
	public ParticleSystem deathParticles;  

	// nilai untuk menghilangkan shader eyes ketika musuh burning
	float cutoffValue = 0f;
	// trigger -> destroy object
	bool triggered = false;

	void Update () {
		// update cutoffValue
		cutoffValue = Mathf.Lerp(cutoffValue, 1f, 0.8f * Time.deltaTime);
		GetComponent<Renderer>().materials[0].SetFloat("_Cutoff", cutoffValue);

		// sebelum material hilang (dissolve value = 1), destroy object
		if (cutoffValue >= 0.8f && !triggered) {
			deathParticles.Stop();
			Destroy(gameObject, 1.5f);
			triggered = true;
		}
	}
}
