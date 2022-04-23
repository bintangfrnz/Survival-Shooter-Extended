using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    // banyak darah pemain di awal
    public int startingHealth = 80;
    // banyak darah pemain saat ini
    public int currentHealth;
    // banyak darah maksimal pemain
    public int maxHealth = 100;
    // waktu player tidak bisa diserang setelah mendapat damage      
	public float invulnerabilityTime = 0.5f; 
      
    // reference -> health slider
    public Slider healthSlider;
    // reference -> player health text
    public TextMeshProUGUI playerHealthText;
    // waktu setelah diserang hingga dapat diserang kembali
    public Image healthBarColor;
    // reference -> flash screen damage image
    public Image damageImage;
    // audio clip ketika pemain mati
    public AudioClip deathClip;
    // kecepatan damage image fading
    public float flashSpeed = 5f;

    // warna flash screen damage imag
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    // warna darah
	public Color minHealthColor = new Color(1.0f, 0.326f, 0.326f, 1.0f); // merah
	public Color maxHealthColor = new Color(0.26f, 0.47f, 0.965f, 1.0f); // biru

    // reference -> animator component
    Animator anim;
    // reference -> audio source component
    AudioSource playerAudio;
    // reference -> player movement
    PlayerMovement playerMovement;
    // reference -> player shooting script
    PlayerShooting playerShooting;
    bool isDead;                                                
    bool damaged; 
    // akumulasi damage untuk current time frame
    float timer;                                   


    private void Awake()
    {
        // mendapatkan reference components
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooting = GetComponentInChildren<PlayerShooting>();
    }

    private void Start()
    {
        // initialize
        currentHealth = startingHealth;
        healthSlider.value = startingHealth;
        playerHealthText.text = $"{currentHealth} / 100";
        healthBarColor.color = maxHealthColor;
    }

    void Update()
    {
        // jika terkena damage
        damageImage.color = (damaged)
            // merubah warna gambar menjadi value dari flashColour
            ? flashColour
            // fade out damage image
            : Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);

        // add time since Update was last called
		timer += Time.deltaTime;

        // update player health text
        playerHealthText.text = $"{currentHealth} / 100";

        // set damage flag to false sehingga flash screen akan hilang
        damaged = false;
    }

    // fungsi untuk menerima damage
    public void TakeDamage(int amount)
    {
        // belum bisa diserang
        if (timer < invulnerabilityTime) {
			return;
		}

        // set damage flag to true sehingga flash screen akan muncul
        damaged = true;

        // mengurangi darah player
        currentHealth -= amount;

        // mencegah darah < 0
        if (currentHealth < 0) {
			currentHealth = 0;
		}

        // mengubah tampilan dari health slider
        healthSlider.value = currentHealth;
        // ubah warna health bar menjadi merah jika < 30%
        healthBarColor.color = maxHealthColor;
		if (currentHealth <= maxHealth * 0.3)
            healthBarColor.color = minHealthColor;

        // akumulasi damage
		timer = 0;

        // memainkan suara saat terkena damage
        playerAudio.Play();

        // memanggil method Death() jika darah < 0 dan belum mati
        if (currentHealth <= 0 && !isDead)
        {
            // mati...
            Death();
        }
    }

    void Death()
    {
        isDead = true;

        playerShooting.DisableEffects();

        // mentrigger animasi Die
        anim.SetTrigger("Die");

        // memainkan suara ketika mati
        playerAudio.clip = deathClip;
        playerAudio.Play();

        // mematikan script player
        playerMovement.enabled = false;
        playerShooting.enabled = false;
    }


    // menambah darah dari health orb
    public void AddHealth(int amount) {
		currentHealth += amount;
		
		if (currentHealth > maxHealth) {
			currentHealth = maxHealth;
		}
		// set health bar menjadi darah saat ini
		healthSlider.value = currentHealth;
        // ubah warna health bar menjadi merah jika < 30%
        healthBarColor.color = maxHealthColor;
		if (currentHealth <= maxHealth * 0.3)
            healthBarColor.color = minHealthColor;
	}
}
