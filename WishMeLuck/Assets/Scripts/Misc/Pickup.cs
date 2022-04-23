using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Pickup : MonoBehaviour {

	// tipe pickup
	public enum PickupType {Float, Bounce, Pierce, Power, Speed, Health};
	public PickupType pickupType = PickupType.Float;
	// efek rotasi
	public float rotateSpeed = 90f;
	// reference -> label
	public TextMeshProUGUI label;
	// reference -> quad renderer
	private Renderer[] quadRenderers;
	// reference -> player
	private GameObject player;  
	GameObject canvas;
	Light pickupLight;
	bool destroyed = false;
	// lama pickup object ada setelah muncul
	float timeExists = 10f;
	float timer;
	bool coroutineStart = false;

	void Awake() {
		// set up reference
		player = GameObject.FindGameObjectWithTag("Player");
		quadRenderers = GetComponentsInChildren<Renderer>();
		canvas = GameObject.Find("PickupLabelCanvas");
		pickupLight = GetComponentInChildren<Light>();
	}

	void Start () {
		label.gameObject.transform.SetParent(canvas.transform, false);
		label.color = pickupLight.color;
		label.transform.localScale = Vector3.one;
		label.transform.rotation = Quaternion.identity;
	}

	void Update() {
		if (destroyed) {
			return;
		}
		// menampilkan efek rotasi box
		transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);

		Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
		label.transform.position = screenPos + new Vector3(0, 40, 0);

		if (timer > timeExists - 3) {
			// menjalankan coroutine hanya sekali
			if (!coroutineStart) {
				coroutineStart = true;
				StartCoroutine(Blinking(3));
			}
			if (timer > timeExists)
				destroyObject();
		}

		timer += Time.deltaTime;
	}

	IEnumerator Blinking(float timeBlinking)
	{
		float t = 0;
		while (t < timeBlinking && GetComponent<Collider>().enabled) {
			pickupLight.enabled = !pickupLight.enabled;
			quadRenderers[0].enabled = !quadRenderers[0].enabled;
			quadRenderers[1].enabled = !quadRenderers[1].enabled;
			quadRenderers[2].enabled = !quadRenderers[2].enabled;
			quadRenderers[3].enabled = !quadRenderers[3].enabled;
			quadRenderers[4].enabled = !quadRenderers[4].enabled;
			t += Time.deltaTime;
			yield return new WaitForSeconds(0.15f);
		}
	}

	void OnTriggerEnter (Collider other) {
		if (destroyed) {
			return;
		}

		if (other.gameObject != player) {
			return;
		}

		switch (pickupType) {
			case PickupType.Float:
				Debug.Log($"Floating Enabled");
				other.GetComponentInChildren<PlayerShooting>().FloatTimer = 0;
				break;
				
			case PickupType.Bounce:
				Debug.Log($"Bouncing Enabled");
				other.GetComponentInChildren<PlayerShooting>().BounceTimer = 0;
				break;
				
			case PickupType.Pierce:
				Debug.Log($"Piercing Enabled");
				other.GetComponentInChildren<PlayerShooting>().PierceTimer = 0;
				break;

			case PickupType.Power:
				Debug.Log($"Power bertambah {2}");
				other.GetComponentInChildren<PlayerShooting>().AddShootPower(2);
				break;

			case PickupType.Speed:
				Debug.Log($"Speed bertambah {0.25f}");
				other.GetComponentInChildren<PlayerMovement>().AddMovementSpeed(0.25f);
				break;
				
			case PickupType.Health:
				Debug.Log($"Darah bertambah {15}");
				other.GetComponentInChildren<PlayerHealth>().AddHealth(15);
				break;
		}

		GetComponent<AudioSource>().Play();
		destroyObject();
	}

	private void destroyObject()
	{
		destroyed = true;

		// disable quad renderer
		foreach (Renderer quadRenderer in quadRenderers) {
			quadRenderer.enabled = false;
		}
		// disable collider
		GetComponent<Collider>().enabled = false;

		// disable light & label
		pickupLight.enabled = false;

		Destroy(label);
		Destroy(gameObject, 1);
	}
}
