using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

    public bool bossActive;

    public float timeBetweenDrops;
    private float timeBetweenDropStore;
    private float dropCount;

    public float waitForPlatforms;
    private float platformCount;

    public Transform leftPoint;
    public Transform rightPoint;
    public Transform dropSawSpawnPoint;

    public GameObject dropSaw;

    public GameObject boss;
    public bool bossRight;

    public GameObject leftPlatforms;
    public GameObject rightPlatforms;

    public bool takeDamage;

    private int startingDamage;
    public int startingHealth;
    private int currentHealth;

    public GameObject levelExit;

    private CameraController theCamera;

    private LevelManager levelManager;

    public bool waitingForRespawn;

	// Use this for initialization
	void Start () {
        timeBetweenDropStore = timeBetweenDrops;
        dropCount = timeBetweenDrops;
        platformCount = waitForPlatforms;
        currentHealth = startingHealth;
        startingDamage = dropSaw.GetComponentInChildren<HurtPlayer>().damageToGive;

        boss.transform.position = rightPoint.position;
        bossRight = true;
        theCamera = FindObjectOfType<CameraController>();
        levelManager = FindObjectOfType<LevelManager>();
	}
	
	// Update is called once per frame
	void Update () {

        if (levelManager.respawnCoActive)
        {
            bossActive = false;
            waitingForRespawn = true;
        }

        if (waitingForRespawn && !levelManager.respawnCoActive)
        {
            boss.SetActive(false);
            leftPlatforms.SetActive(false);
            rightPlatforms.SetActive(false);

            timeBetweenDrops = timeBetweenDropStore;

            platformCount = waitForPlatforms;
            dropCount = timeBetweenDrops;

            boss.transform.position = rightPoint.position;
            bossRight = true;
            currentHealth = startingHealth;
            dropSaw.GetComponentInChildren<HurtPlayer>().damageToGive = startingDamage;

            theCamera.followTarget = true;

            waitingForRespawn = false;
            levelManager.bossFightMusic.Stop();
            levelManager.levelMusic.Stop();
            levelManager.levelMusic.Play();
        }

		if (bossActive)
        {
            theCamera.followTarget = false;
            theCamera.transform.position = Vector3.Lerp(theCamera.transform.position, new Vector3(transform.position.x, theCamera.transform.position.y, theCamera.transform.position.z), theCamera.smoothing * Time.deltaTime);

            boss.SetActive(true);
            if(dropCount > 0f)
            {
                dropCount -= Time.deltaTime;
            }
            else
            {
                dropSawSpawnPoint.position = new Vector3(Random.Range(leftPoint.position.x, rightPoint.position.x), dropSawSpawnPoint.position.y, 0f);
                Instantiate(dropSaw, dropSawSpawnPoint.position, dropSawSpawnPoint.rotation);
                dropCount = timeBetweenDrops;
            }

            if (platformCount > 0f)
            {
                platformCount -= Time.deltaTime;
            }

            if (bossRight)
            {
                if (platformCount <= 0f)
                {
                    rightPlatforms.SetActive(true);
                }
            }
            else
            {
                if (platformCount <= 0f)
                {
                    leftPlatforms.SetActive(true);
                }
            }

            if (takeDamage)
            {
                currentHealth -= 1;

                if (currentHealth <= 0)
                {
                    dropSaw.GetComponentInChildren<HurtPlayer>().damageToGive = startingDamage;
                    levelExit.SetActive(true);
                    theCamera.followTarget = true;

                    levelManager.bossFightMusic.Stop();
                    levelManager.levelMusic.Play();

                    gameObject.SetActive(false);
                    
                }

                if (bossRight)
                {
                    boss.transform.position = leftPoint.position;
                }
                else
                {
                    boss.transform.position = rightPoint.position;
                }

                bossRight = !bossRight;
                leftPlatforms.SetActive(false);
                rightPlatforms.SetActive(false);

                platformCount = waitForPlatforms;

                timeBetweenDrops = timeBetweenDrops / 2f;
                dropSaw.GetComponentInChildren<HurtPlayer>().damageToGive += 1;

                takeDamage = false;
            }
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            bossActive = true;
        }
    }
}
