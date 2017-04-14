using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public float turnDelay = .1f;

	//accessible outside of class
	//variable belongs to the class itself instead of an instance of the class
	//can access public functions of GameManager from any script in the game
	public static GameManager instance = null;

	public BoardManager boardScript;
	public int playerFoodPoints = 100;
	[HideInInspector] public bool playersTurn = true;

	private int level = 3;
	private List<Enemy> enemies;
	private bool enemiesMoving;

	void Awake ()
	{
		//Dont end up with two instances of GameManager
		if (instance == null) {
			instance = this;
		} else if(instance != null) {
			Destroy (gameObject);
		}

		//Dont destroy game objects in between scenes
		DontDestroyOnLoad (gameObject);
		enemies = new List<Enemy> ();
		boardScript = GetComponent<BoardManager> ();
		InitGame ();
	}

	void InitGame ()
	{
		enemies.Clear ();
		boardScript.SetupScene (level);
	}

	public void GameOver()
	{
		enabled = false;
	}

	void Update ()
	{
		if (playersTurn || enemiesMoving)
			return;

		StartCoroutine (MoveEnemies ());
	}

	public void AddEnemyToList(Enemy script)
	{
		enemies.Add (script);
	}

	IEnumerator MoveEnemies()
	{
		enemiesMoving = true;
		yield return new WaitForSeconds (turnDelay);
		if (enemies.Count == 0) {
			yield return new WaitForSeconds (turnDelay);
		}

		for (int i = 0; i < enemies.Count; i++) {
			enemies [i].MoveEnemy ();
			yield return new WaitForSeconds (turnDelay);
		}
		playersTurn = true;
		enemiesMoving = false;
	}
}
