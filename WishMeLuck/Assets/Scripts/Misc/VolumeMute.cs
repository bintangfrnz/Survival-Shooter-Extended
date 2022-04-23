using UnityEngine;
using System.Collections;

public class VolumeMute : MonoBehaviour {

	// control volume
    public float volume = 1.0f;

	void Start () {
		AudioListener.volume = volume;
	}
}
