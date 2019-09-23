using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public string firstLevel;
    public string levelSelect;
    public int startingLives;
    public int startingHealth;

    public List<string> levelNames;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void NewGame()
    {
        SceneManager.LoadScene(firstLevel);

        levelNames.ForEach(name => PlayerPrefs.SetInt(name, 0));
        PlayerPrefs.SetInt("CoinCount", 0);
        PlayerPrefs.SetInt("PlayerLives", startingLives);
        PlayerPrefs.SetInt("PlayerHealth", startingHealth);
    }

    public void Continue()
    {
        SceneManager.LoadScene(levelSelect);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
