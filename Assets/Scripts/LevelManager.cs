using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public float waitToRespawn;
    public PlayerController thePlayer;

    public GameObject deathExplosion;

    public int coinCount;
    public Text coinText;
    public AudioSource coinSound;

    public Image heart1;
    public Image heart2;
    public Image heart3;

    public Sprite heartFull;
    public Sprite heartHalf;
    public Sprite heartEmpty;

    public int maxHealth;
    public int healthCount;

    public bool respawning;

    public List<ResetOnRespawn> objectsToReset;

    public bool invincible;

    public int startingLives;
    public int currentLives;
    public Text livesText;

    public GameObject gameOverScreen;

    public int bonusLiveTreshold;

    public AudioSource levelMusic;
    public AudioSource gameOverMusic;
    public AudioSource levelEndMusic;
    public AudioSource bossFightMusic;

    public bool respawnCoActive;

    // Use this for initialization
    void Start () {
        thePlayer = FindObjectOfType<PlayerController>();

        objectsToReset.AddRange(FindObjectsOfType<ResetOnRespawn>());

        if (PlayerPrefs.HasKey("CoinCount"))
        {
            coinCount = PlayerPrefs.GetInt("CoinCount");
        }
        else
        {
            coinCount = 0;
        }

        coinText.text = "COINS: " + coinCount;

        if (PlayerPrefs.HasKey("PlayerLives"))
        {
            currentLives = PlayerPrefs.GetInt("PlayerLives");
        }
        else
        {
            currentLives = startingLives;
        }

        livesText.text = "LIVES: " + currentLives;

        if (PlayerPrefs.HasKey("PlayerHealth"))
        {
            healthCount = PlayerPrefs.GetInt("PlayerHealth");
            UpdateHeartMeter();
        }
        else
        {
            healthCount = maxHealth;
        }

        gameOverScreen.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (healthCount <= 0 && !respawning)
        {
            Respawn();
            respawning = true;
        }
    }

    public void Respawn()
    {
        currentLives -= 1;
        livesText.text = "LIVES: " + currentLives;
        if (currentLives > 0)
            StartCoroutine("RespawnCo");
        else
        {
            thePlayer.gameObject.SetActive(false);
            gameOverScreen.SetActive(true);

            levelMusic.volume = levelMusic.volume / 8f;
            gameOverMusic.Play();
        }
    }

    public IEnumerator RespawnCo()
    {
        respawnCoActive = true;

        thePlayer.gameObject.SetActive(false);

        Instantiate(deathExplosion, thePlayer.transform.position, thePlayer.transform.rotation);

        yield return new WaitForSeconds(waitToRespawn);

        respawnCoActive = false;
        
        coinCount = 0;

        healthCount = maxHealth;
        UpdateHeartMeter();
        respawning = false;

        thePlayer.transform.position = thePlayer.respawnPosition;
        thePlayer.gameObject.SetActive(true);
        objectsToReset.ForEach( obj => { obj.gameObject.SetActive(true);
                                         obj.ResetObject();});
        coinText.text = "COINS: " + coinCount;
    }

    public void AddCoins(int coinsToAdd)
    {
        coinCount += coinsToAdd;
        if (coinCount >= bonusLiveTreshold)
        {
            currentLives += 1;
            coinCount -= bonusLiveTreshold;
            livesText.text = "LIVES: " + currentLives;
        }

        
        coinText.text = "COINS: " + coinCount;
        coinSound.Play();
    }

    public void HurtPlayer(int damageToTake)
    {
        if (!invincible)
        {
            healthCount -= damageToTake;
            UpdateHeartMeter();
            if (healthCount > 0)
                thePlayer.Knockback();

            thePlayer.hurtSound.Play();
        }
    }

    public void GiveHealth(int healthToGive)
    {
        healthCount += healthToGive;

        if (healthCount > maxHealth)
            healthCount = maxHealth;

        coinSound.Play();
        UpdateHeartMeter();
    }

    public void UpdateHeartMeter()
    {
        switch (healthCount)
        {
            case 6:
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartFull;
                return;
            case 5:
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartHalf;
                return;
            case 4:
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartEmpty;
                return;
            case 3:
                heart1.sprite = heartFull;
                heart2.sprite = heartHalf;
                heart3.sprite = heartEmpty;
                return;
            case 2:
                heart1.sprite = heartFull;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                return;
            case 1:
                heart1.sprite = heartHalf;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                return;
            case 0:
                heart1.sprite = heartEmpty;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                return;
            default:
                heart1.sprite = heartEmpty;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                return;
        }
    }

    public void AddLives(int livesToAdd)
    {
        currentLives += livesToAdd;
        livesText.text = "LIVES: " + currentLives;
        coinSound.Play();
    }
}
