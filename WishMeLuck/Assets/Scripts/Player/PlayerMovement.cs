using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    // kecepatan player bergerak di awal
    public float startingSpeed = 5f;
    // kecepatan player bergerak saat ini
    public float currentSpeed;
    // kecepatan maksimal player bergerak
    public float maxSpeed = 10f;
    // reference -> speed slider
    public Slider speedSlider;
    // reference -> player speed text
    public TextMeshProUGUI playerSpeedText;
    // vektor yang menyimpan arah gerak pemain
    Vector3 movement;
    // reference -> animator component
    Animator anim;
    // reference -> player's rigidbody
    Rigidbody playerRigidbody;
    // layer mask
    int floorMask;
    // panjang ray dari kamera ke scene
    float camRayLength = 100f;

    private void Awake()
    {
        // mendapatkan nilai mask dari layer yang bernama Floor
        floorMask = LayerMask.GetMask("Floor");

        // set up reference
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // initialize
        currentSpeed = startingSpeed;
        speedSlider.value = startingSpeed;
        playerSpeedText.text = $"{currentSpeed} / 15";
    }

    private void FixedUpdate()
    {
        // menyimpan input axes
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // move player dalam scene
        Move(h, v);
        // turn player ke mouse cursor
        Turning();
        // animate the player
        Animating(h, v);
    }

    void Update ()
    {
        // update player speed text
        playerSpeedText.text = $"{currentSpeed} / 10";
    }

    void Move(float h, float v)
    {
        // set nilai x dan y
        movement.Set(h, 0f, v);

        // normalisasi nilai vector dan make it proportional to the speed per second
        movement = movement.normalized * currentSpeed * Time.deltaTime;

        // move player ke posisi saat ini
        playerRigidbody.MovePosition(transform.position + movement);
    }

    void Turning()
    {
        // buat Ray dari posisi mouse di layar
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // buat raycast untuk floorHit
        RaycastHit floorHit;

        // lakukan raycast
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            // mendapatkan vector dari posisi player dan posisi floorHit
            Vector3 playerToMouse = floorHit.point - transform.position;
            // memastikan vector berada pada floor plane
            playerToMouse.y = 0f;

            // mendapatkan look rotation baru ke hit position
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            // rotasi player
            playerRigidbody.MoveRotation(newRotation);
        }
    }

    void Animating(float h, float v)
    {
        // cek apakah input axes bukan 0
        bool walking = h != 0f || v != 0f;
        // ubah status is walking
        anim.SetBool("IsWalking", walking);
    }

    public void AddMovementSpeed(float amount) {
		currentSpeed += amount;
		
		if (currentSpeed > maxSpeed) {
			currentSpeed = maxSpeed;
		}
        // mengubah tampilan dari speed slider
        speedSlider.value = currentSpeed;
	}
}