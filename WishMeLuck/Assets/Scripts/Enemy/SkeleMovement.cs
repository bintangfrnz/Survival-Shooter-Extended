using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeleMovement : MonoBehaviour
{
    // layer yang hanya dapat dihit raycast
	public LayerMask shootableMask;

    // reference -> posisi player
	Transform player;
	// reference -> darah player
	PlayerHealth playerHealth;
	// reference -> darah musuh
	EnemyHealth enemyHealth;

    // reference -> enemy's rigidbody
    Rigidbody enemyRigidbody;
	// reference -> renderer
	SkinnedMeshRenderer myRenderer;

    // warna mata
	public Color rageEyesColor = new Color(0, 0, 1, 1); // biru
	public Color calmEyesColor = new Color(0, 0, 0, 1); // hitam

    // ray dari gun end forwards
	Ray shootRay;          
	// info apa yang tertembak
	RaycastHit shootHit;
	// vektor posisi
	Vector3 position;

    // cek player terlihat atau tidak
	bool foundPlayer = false;

    // Start is called before the first frame update
    void Awake()
    {
        // mendapatkan reference
		player = GameObject.FindGameObjectWithTag("Player").transform;
		playerHealth = player.GetComponent<PlayerHealth>();
        enemyRigidbody = GetComponent<Rigidbody>();
		enemyHealth = GetComponent<EnemyHealth>();
		myRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // saat musuh masih hidup
		if (enemyHealth.currentHealth > 0) {
            foundPlayer = false;
			if (playerHealth.currentHealth > 0) {

                // menembakkan ray lurus ke arah player
				Vector3 direction = (player.position + new Vector3(0, 1, 0)) - (transform.position + new Vector3(0, 1, 0));
				shootRay.origin = transform.position + new Vector3(0, 1, 0);
				shootRay.direction = direction;

                if (Physics.Raycast(shootRay, out shootHit, 25, shootableMask)) {
					// jika tidak terhalang objek
					if (shootHit.transform.tag == "Player") {
						foundPlayer = true;
						ChangeEyeColor(rageEyesColor);

                        Quaternion targetRotation = Quaternion.LookRotation(player.position - transform.position);
                        // rotasi menghadap player dengan smooth
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);					
					}
					// jika terhalang objek lain
					else {
						ChangeEyeColor(calmEyesColor);
					}
				}
            }

			if (!foundPlayer) {
				ChangeEyeColor(calmEyesColor);
			}

        } else {
			ChangeEyeColor(calmEyesColor);
        }
    }

    private void ChangeEyeColor(Color color) {
		myRenderer.materials[1].SetColor("_RimColor",
			Color.Lerp(myRenderer.materials[1].GetColor("_RimColor"), color, 2 * Time.deltaTime));
	}

    public bool IsFoundPlayer() {
        return foundPlayer;
    }
}
