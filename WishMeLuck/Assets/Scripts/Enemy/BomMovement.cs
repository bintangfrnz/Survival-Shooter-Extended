using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomMovement : MonoBehaviour
{
    // layer yang hanya dapat dihit raycast
	public LayerMask shootableMask;
	// speed santai
	public float calmSpeed = 4f;
	// speed rage
	public float rageSpeed = 6f;

	// reference -> posisi player
	Transform player;
	// reference -> darah player
	PlayerHealth playerHealth;
	// reference -> darah musuh
	EnemyHealth enemyHealth;

	// reference -> nav mesh agent
	UnityEngine.AI.NavMeshAgent nav;
	// reference -> renderer
	SkinnedMeshRenderer myRenderer;

	// warna mata
	public Color rageEyesColor = new Color(1, 1, 1, 1); // putih
	public Color calmEyesColor = new Color(0, 0, 0, 1); // hitam

	// ray dari gun end forwards
	Ray shootRay;          
	// info apa yang tertembak
	RaycastHit shootHit;
	// vektor posisi
	Vector3 position;

	// cek player terlihat atau tidak
	bool hasValidTarget = false; // mungkin juga environment
	bool foundPlayer = false;
	bool coroutineStart = false;

	void Awake() {
		// mendapatkan reference
		player = GameObject.FindGameObjectWithTag("Player").transform;
		playerHealth = player.GetComponent<PlayerHealth>();
		enemyHealth = GetComponent<EnemyHealth>();
		nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
		myRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

		SetRandomNavTarget();
	}
	
	
	void Update() {
		// saat musuh masih hidup
		if (enemyHealth.currentHealth > 0) {

			// jarak musuh ke player
			Vector3 distanceFromTarget = position - transform.position;

			foundPlayer = false;
			if (playerHealth.currentHealth > 0) {

				// menembakkan ray lurus ke arah player
				Vector3 direction = (player.position + new Vector3(0, 1, 0)) - (transform.position + new Vector3(0, 1, 0));
				shootRay.origin = transform.position + new Vector3(0, 1, 0);
				shootRay.direction = direction;

				if (Physics.Raycast(shootRay, out shootHit, 25, shootableMask)) {
					// jika tidak terhalang objek
					if (shootHit.transform.tag == "Player") {
						position = player.position;
						hasValidTarget = true;
						foundPlayer = true;
						nav.speed = rageSpeed;
						ChangeEyeColor(rageEyesColor);

						if (!coroutineStart) {
							coroutineStart = true;
							StartCoroutine(Blinking());
						}				
					}
					// jika terhalang objek lain
					else {
						ChangeEyeColor(calmEyesColor);
					}
				}
			}

			// player tidak berada dalam satu garis lurus
			if (!foundPlayer) {
				if (distanceFromTarget.magnitude < 1 || !hasValidTarget) {
					SetRandomNavTarget();
				}
				nav.speed = calmSpeed;
				ChangeEyeColor(calmEyesColor);
			}

			if (hasValidTarget) {
				try {
					nav.SetDestination(position);
				} catch {
					Debug.Log("Bug Position");
					SetRandomNavTarget();
				} 
			}
		}
		// musuh mati
		else {
			// disable nav mesh agent
			nav.enabled = false;
			// ubah mata menjadi hitam
			ChangeEyeColor(calmEyesColor);
		}
	}

	IEnumerator Blinking()
	{
		Light bomLight = GetComponent<Light>();
		while (foundPlayer && !enemyHealth.IsDeath()) {
			bomLight.enabled = !bomLight.enabled;
			yield return new WaitForSeconds(0.15f);
		}
		coroutineStart = false;
		yield break;
	}

	// berjalan keliling bebas ke arah random
	private void SetRandomNavTarget() {
		// generate posisi random dalam unit sphere
		Vector3 randomPosition = Random.insideUnitSphere * 30;
		randomPosition.y = 0;
		randomPosition += transform.position;

		UnityEngine.AI.NavMeshHit hit;
		hasValidTarget = UnityEngine.AI.NavMesh.SamplePosition(randomPosition, out hit, 5, 1);

		// posisi random yang dituju
		Vector3 finalPosition = hit.position;
		position = finalPosition;
    }

	private void ChangeEyeColor(Color color) {
		myRenderer.materials[1].SetColor("_RimColor",
			Color.Lerp(myRenderer.materials[1].GetColor("_RimColor"), color, 2 * Time.deltaTime));
	}
}