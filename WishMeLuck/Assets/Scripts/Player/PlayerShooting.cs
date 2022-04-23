using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerShooting : MonoBehaviour {

    public Color[] bulletColors;
    // efek peluru
    public float bounceDuration = 10;
    public float pierceDuration = 10;
    public float floatTimeDuration = 10;
    // power 1 tembakan player di awal
    public int startingPower = 20;
    // power 1 tembakan player saat ini
    public int damagePerShot;
    // power maksimal 1 tembakan player
    public int maxPower = 50;
    // peluru yang keluar dalam sekali tembakan
    public int numberOfBullets = 1;
    // kecepatan peluru
	public float shotSpeed = 30.0f;
	// lama peluru di udara (tidak menggunakan jarak)
	public float lifeOfBullet = 0.5f;
    // waktu diantara tembakan
    public float timeBetweenBullets = 0.15f;
    public float angleBetweenBullets = 10f;

    // reference -> power slider
    public Slider powerSlider;
    // reference -> player power text
    public TextMeshProUGUI playerPowerText;

    // layer mask yang hanya dapat dikenai raycast
    public LayerMask shootableMask;
    // reference -> enchange UI
    public Image bounceImage;
    public Image pierceImage;
    public TextMeshProUGUI floatTimeText;
    // reference -> bullet prefabs
    public GameObject bullet;
    PlayerBullet cBullet;
    // reference -> bullet spawn anchor
    public Transform bulletSpawnAnchor;
    // reference -> timer text
	public GameObject pierceTimerObj;
	public GameObject bounceTimerObj;
    public GameObject floatTimerObj;

    // waktu untuk menembak
    float timer;
    // ray from the gun end forwards
    Ray shootRay;
    // info apa yang tertembak 
    RaycastHit shootHit;
    // reference -> particle system
    ParticleSystem gunParticles;
    // reference -> line renderer
    LineRenderer gunLine;
    // reference -> gun audio
    AudioSource gunAudio;
    // reference -> light component  
    Light gunLight;
    // proporsi timeBetweenBullets yang efeknya akan muncul 
    float effectsDisplayTime = 0.2f;

    // waktu efek
    float bounceTimer;
    float pierceTimer;
    float floatTimer;
    // efek peluru
    bool bouncing;
    bool piercing;
    bool floating;
    // warna peluru
    Color bulletColor;

    public float FloatTimer {
        get { return floatTimer; }
        set { floatTimer = value; }
    }

    public float BounceTimer {
        get { return bounceTimer; }
        set { bounceTimer = value; }
    }

    public float PierceTimer {
        get { return pierceTimer; }
        set { pierceTimer = value; }
    }

    void Awake() {
        // mendapatkan reference
        gunParticles = GetComponent<ParticleSystem>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponentInChildren<Light>();
    }

    private void Start()
    {
        // initialize
        bounceTimer = bounceDuration;
        pierceTimer = pierceDuration;
        floatTimer = floatTimeDuration;

        damagePerShot = startingPower;
        powerSlider.value = startingPower;
        playerPowerText.text = $"{damagePerShot} / 50";
    }

    void Update() {
        // sebelum dapat efek
        lifeOfBullet = 0.5f;

        // update player power text
        playerPowerText.text = $"{damagePerShot} / 50";

		// disable timer labels
		bounceTimerObj.SetActive(false);
		pierceTimerObj.SetActive(false);
        floatTimerObj.SetActive(false);

        bouncing = (bounceTimer < bounceDuration) ? true : false;
        piercing = (pierceTimer < pierceDuration) ? true : false;
        floating = (floatTimer < floatTimeDuration) ? true : false;

        bulletColor = bulletColors[0];
        if (floating) {
            // enable label
			floatTimerObj.SetActive(true);
			TextMeshProUGUI floatTime = floatTimerObj.GetComponent<TextMeshProUGUI> ();
			floatTime.text = Mathf.CeilToInt(floatTimeDuration - floatTimer).ToString ();

            lifeOfBullet = 1f;
            floatTimeText.color = bulletColors[0];
        }
        floatTimeText.gameObject.SetActive(floating);

        if (bouncing) {
			// enable label
			bounceTimerObj.SetActive(true);
			TextMeshProUGUI bounceTime = bounceTimerObj.GetComponent<TextMeshProUGUI>();
			bounceTime.text = Mathf.CeilToInt(bounceDuration - bounceTimer).ToString();

            bulletColor = bulletColors[1];
            bounceImage.color = bulletColors[1];
        }
        bounceImage.gameObject.SetActive(bouncing);

        if (piercing) {
			// enable label
			pierceTimerObj.SetActive(true);
			TextMeshProUGUI pierceTime = pierceTimerObj.GetComponent<TextMeshProUGUI>();
			pierceTime.text = Mathf.CeilToInt(pierceDuration - pierceTimer).ToString();

            bulletColor = bulletColors[2];
            pierceImage.color = bulletColors[2];
        }
        pierceImage.gameObject.SetActive(piercing);

        // ganti warna jika kedua efek berjalan
        if (piercing & bouncing) {
            bulletColor = bulletColors[3];
            bounceImage.color = bulletColors[3];
            pierceImage.color = bulletColors[3];
        }

		var main = gunParticles.main;
		main.startColor = bulletColor;
        // bug -> harus diinitialize manual
        gunLight.color = (piercing & bouncing) ? new Color(1, 140f / 255f, 30f / 255f, 1) : bulletColor;

        // hitung waktu munfur efek
        bounceTimer += Time.deltaTime;
        pierceTimer += Time.deltaTime;
        floatTimer += Time.deltaTime;
        timer += Time.deltaTime;

        // shoot...
        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0) {
            Shoot();
        }

        if (timer >= timeBetweenBullets * effectsDisplayTime) {
            DisableEffects();
        }
    }

    public void DisableEffects() {
        gunLight.enabled = false;
    }

    void Shoot() {
        // reset timer
        timer = 0f;

        // putar gun shoot audio
        gunAudio.pitch = (bouncing & piercing) ? Random.Range(0.9f, 1.0f) :
                         (bouncing) ? Random.Range(1.1f, 1.2f) :
                         (piercing) ? gunAudio.pitch = Random.Range(1.0f, 1.1f) :
                         Random.Range(1.2f, 1.3f);
        gunAudio.Play();

        // enable light
        gunLight.intensity = 2 + (0.25f * (numberOfBullets - 1));
        gunLight.enabled = true;

        gunParticles.Stop();
		var main = gunParticles.main;
        main.startSize = 1 + (0.1f * (numberOfBullets - 1));
        gunParticles.Play();

        // tembak arah lurus
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        for (int i = 0; i < numberOfBullets; i++) {
            // spread bullet
            float angle = i * angleBetweenBullets - ((angleBetweenBullets / 2) * (numberOfBullets - 1));
            Quaternion rot = transform.rotation * Quaternion.AngleAxis(angle, Vector3.up);
            GameObject instantiatedBullet = Instantiate(bullet, bulletSpawnAnchor.transform.position, rot) as GameObject;
            
            // detail setiap bullet
            cBullet = instantiatedBullet.GetComponent<PlayerBullet>();
            cBullet.damage = damagePerShot;
            cBullet.speed = shotSpeed;
            cBullet.life = lifeOfBullet;
            cBullet.piercing = piercing;
            cBullet.bouncing = bouncing;
            cBullet.bulletStartColor = bulletColor;
        }
    }

    public void AddShootPower(int amount) {
		damagePerShot += amount;
		
		if (damagePerShot > maxPower) {
			damagePerShot  = maxPower;
		}
        // mengubah tampilan dari speed slider
        powerSlider.value = damagePerShot;
	}

    // upgrade Weapon
    public void AddShootSpeed() {
        shotSpeed += 10.0f;
	}

    public void AddBullet() {
        numberOfBullets += 2;
	}
}
