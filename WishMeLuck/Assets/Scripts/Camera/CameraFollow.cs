using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	// posisi target yang akan diikuti camera
	public Transform target; 
	// kecepatan kamera mengikuti target
	public float smoothing = 5f;     
	// jarak kamera ke target
	Vector3 offset;

	void Start() {
		// hitung offset
		offset = transform.position - target.position;
	}

	void FixedUpdate () {
		// menentukan posisi target kamera
		Vector3 targetCamPos = target.position + offset;
		
		// interpolasi antara posisi camera dan target camera
		transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
	}
}