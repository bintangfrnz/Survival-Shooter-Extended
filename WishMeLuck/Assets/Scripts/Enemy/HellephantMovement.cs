using UnityEngine;
using System.Collections;

public class HellephantMovement : MonoBehaviour {
	
	public float moveSpeed = 3f;
	public float rotateSpeed = 2f;
	[HideInInspector]
	public bool shouldMove = true;

	// reference -> posisi player
	Transform player;       
	// reference -> darah player
	PlayerHealth playerHealth;     
	// reference -> darah musuh
	EnemyHealth enemyHealth;      
	// reference -> renderer
	SkinnedMeshRenderer myRenderer;
	Vector3 position;
	float currentSpeed;

	void Awake() {
		// mendapatkan references
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		playerHealth = player.GetComponent<PlayerHealth> ();
		enemyHealth = GetComponent <EnemyHealth> ();
		myRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
	}
	
	void Update() {
		// jika boss masih hidup
		if (enemyHealth.currentHealth > 0) {
			if (playerHealth.currentHealth > 0) {
				Rotate();

				if (shouldMove) {
					Move();
				}
			}
		}
		// jika boss mati
		else {
			myRenderer.materials[1].SetColor("_RimColor", Color.Lerp(myRenderer.materials[1].GetColor("_RimColor"), new Color(0, 0, 0, 1), 2 * Time.deltaTime));
		}
	}

	void Rotate() {
		Quaternion rot = Quaternion.LookRotation(player.position - transform.position);
		float rotationX = rot.eulerAngles.y;
		Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
		transform.localRotation = Quaternion.Slerp(transform.localRotation, xQuaternion, Time.deltaTime * rotateSpeed);
	}
	
	void Move() {
		transform.GetComponent<Rigidbody>().MovePosition(transform.GetComponent<Rigidbody>().position + transform.TransformDirection(0, 0, moveSpeed) * Time.deltaTime);
	}
}