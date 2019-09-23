using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour {

    public string levelToLoad;

    public string levelToUnlock;

    private PlayerController player;
    private LevelManager levelManager;
    private CameraController theCamera;

    public float waitToMove;
    public float waitToLoad;

    private bool movePlayer;

    public Sprite flagOpen;
    private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerController>();
        theCamera = FindObjectOfType<CameraController>();
        levelManager = FindObjectOfType<LevelManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (movePlayer)
        {
            player.myRigidbody.velocity = new Vector3(player.moveSpeed, player.myRigidbody.velocity.y, 0f);
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            spriteRenderer.sprite = flagOpen;
            StartCoroutine("LevelEndCo");
    }

    public IEnumerator LevelEndCo()
    {
        player.canMove = false;
        theCamera.followTarget = false;
        levelManager.invincible = true;

        levelManager.levelMusic.Stop();
        levelManager.levelEndMusic.Play();

        player.myRigidbody.velocity = Vector3.zero;

        PlayerPrefs.SetInt("CoinCount", levelManager.coinCount);
        PlayerPrefs.SetInt("PlayerLives", levelManager.currentLives);
        PlayerPrefs.SetInt("PlayerHealth", levelManager.healthCount);

        PlayerPrefs.SetInt(levelToUnlock, 1);

        yield return new WaitForSeconds(waitToMove);

        movePlayer = true;

        yield return new WaitForSeconds(waitToLoad);

        UnityEngine.SceneManagement.SceneManager.LoadScene(levelToLoad);

        levelManager.levelEndMusic.Stop();
    }
}
