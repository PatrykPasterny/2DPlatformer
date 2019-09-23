using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearthPickup : MonoBehaviour {

    public int healthToGive;

    private LevelManager levelManager;

	// Use this for initialization
	void Start () {
        levelManager = FindObjectOfType<LevelManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            levelManager.GiveHealth(healthToGive);
            gameObject.SetActive(false);
        }
    }
}
