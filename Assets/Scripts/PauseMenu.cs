using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public string levelSelect;
    public string mainMenu;

    private LevelManager levelManager;

    public GameObject pauseScreen;

    private PlayerController player;

	// Use this for initialization
	void Start () {
        levelManager = FindObjectOfType<LevelManager>();
        player = FindObjectOfType<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Pause"))
        {
            if (Time.timeScale == 0f)
            {
                Resume();
            }
            else
            {
                PauseGame();
            }
        }
	}

    public void PauseGame()
    {
        Time.timeScale = 0;

        pauseScreen.SetActive(true);
        player.canMove = false;
        levelManager.levelMusic.Pause();
    }
    public void Resume()
    {
        Time.timeScale = 1f;

        pauseScreen.SetActive(false);
        player.canMove = true;
        levelManager.levelMusic.Play();
    }

    public void LevelSelect()
    {
        PlayerPrefs.SetInt("PlayerLives", levelManager.currentLives);
        PlayerPrefs.SetInt("CoinCount", levelManager.coinCount);
        PlayerPrefs.SetInt("PlayerHealth", levelManager.healthCount);

        Time.timeScale = 1f;

        SceneManager.LoadScene(levelSelect);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(mainMenu);
    }
}
