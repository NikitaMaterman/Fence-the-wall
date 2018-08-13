using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WorldManager : MonoBehaviour {
    public static WorldManager Instance = null;

	public bool LevelCompleted;
	public Text Gold;
	public Text Hearts;
	public Text GameOverText;
	public int RemainingHearts;
	public Inventory Inventory;
	public GameObject Gate;

	public GameObject TowerPanel;
	public Button[] TowerPanelButtons;
	public GameObject LevelCompletedPanel;
	public GameObject NextLevelButtonObject;
	public GameObject QuitGameButtonObject;

	private int nextLevel = 0;
	private bool loaded;

	void Awake() {
        SceneManager.sceneLoaded += DelayedOnSceneLoaded;
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

	void DelayedOnSceneLoaded(Scene scene, LoadSceneMode mode) {
		loaded = false;
		LevelCompleted = false;
		StartCoroutine("OnSceneLoaded");
	}

	IEnumerator OnSceneLoaded() {
		yield return new WaitForEndOfFrame();

		Inventory = ScriptableObject.CreateInstance<Inventory>();
		RemainingHearts = 3;

		var goldObject = GameObject.Find("Gold");
		Gold = goldObject.GetComponent<Text>();

		var heartsObject = GameObject.Find("Hearts");
		Hearts = heartsObject.GetComponent<Text>();

		var gameOverObject = GameObject.Find("GameOver");
		GameOverText = gameOverObject.GetComponent<Text>();
		GameOverText.enabled = false;
		
		TowerPanel = GameObject.Find("TowerPanel");
		TowerPanelButtons = TowerPanel.GetComponentsInChildren<Button>();
		TowerPanel.SetActive(false);

		LevelCompletedPanel = GameObject.Find("LevelCompletedPanel");
		NextLevelButtonObject = GameObject.Find("NextLevel");

		QuitGameButtonObject = GameObject.Find("QuitGame");
		QuitGameButtonObject.SetActive(false);

		// Bind switch scene code if there's a next scene
		if (SceneManager.sceneCountInBuildSettings > nextLevel + 1) {
			NextLevelButtonObject.GetComponentInChildren<Button>().onClick.AddListener(LoadNextLevel);
		} else {
			NextLevelButtonObject.SetActive(false);
			QuitGameButtonObject.GetComponentInChildren<Button>().onClick.AddListener(QuitGame);
			QuitGameButtonObject.SetActive(true);
		}

		LevelCompletedPanel.SetActive(false);

		Gate = GameObject.FindGameObjectWithTag("Gate");

		loaded = true;
	}

	void LoadNextLevel() {
		nextLevel++;
		SceneManager.LoadScene(nextLevel);
	}

	void QuitGame() {
		Application.Quit();
	}

    void Start() {

    }

	void Update() {
		if (Input.GetKey("escape")) {
			QuitGame();
		}

		if (!loaded) {
			return;
		}

		Gold.text = String.Format("Gold: {0}", Inventory.Gold);

		var hearts = "";
		for (int i = 0; i < RemainingHearts; i++) {
			hearts += " ♥";
		}
		Hearts.text = hearts;

		if (RemainingHearts <= 0) {
			GameOverText.enabled = true;
		}

		var waves = Gate.GetComponents<Wave>();
		var enemies = GameObject.FindGameObjectsWithTag("Enemy");
		if (waves.Length == 0 && enemies.Length == 0) {
			// So other classes can find out whether it's completed
			LevelCompleted = true;

			// Enable level completed UI
			LevelCompletedPanel.SetActive(true);
		}
	}
}
