using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Needed for displaying the Score and Player lives on the screen
using UnityEngine.UI;

/// <summary>
/// Author: Scott Gartz
/// Class to check for collision between the ship and asteroids, as well
/// as between bullets and asteroids. Based on if there's a collision, it 
/// will be resolved, either by damaging the player, or destroying the asteroid.
///
/// </summary>
public class Collision : MonoBehaviour
{
    //Stores the player
	public GameObject ship;
	//Stores all asteroids currently in scene
	public GameObject[] asteroids;
	//Stores all bullets currently in the scene
	public GameObject[] bullets;
	//Stores computer distance between two objects
	private Vector3 objectDistance;
	//radius of the asteroid bounding circle
	private float astRadius;
	//Radius of the ship bounding circle
	private float radius;
	//Stores the score progress towards an extra life
	private int extraLifeScore;
	//Current score
	private int score;
	//Current HP of the ship
	private int shipLives;
	//False if collision between player and asteroid, true if between bullet and asteroid
	private bool collided=false;
	//False if Powerup is not active, true while active
	private bool powerUp1=false;
	//Stores the text to be displayed;
	string displayText;
	//Timer for powerUp1
	private float timer=0;

	//Canvas and a text property to display score and health
	public Canvas gui;
	private Text scoreTxt;

	// Use this for initialization
	void Start ()
	{
		shipLives = 3;
		score = 0;
		radius = 1f;
		astRadius = 2.5f;
		scoreTxt = gui.GetComponent<Text> ();

	
	}
	
	// Update is called once per frame
	void Update ()
	{
	    //Use of Unity's Tag Manager to allow easy finding of objects.
		//Asteroid prefabs are tagged with "Asteroid" and bullet prefab is tagged with "Bullet"
		//So we can search for and store all Asteroids and Bullets into arrays for collision detection
		asteroids = GameObject.FindGameObjectsWithTag ("Asteroid"); 
		bullets = GameObject.FindGameObjectsWithTag ("Bullet");

		CheckCollide ();
		CheckBulletCollide ();
		DisplayScore ();
		if(powerUp1==true){PowerUpTimer ();}
	}

	/// <summary>
	/// Checks for a collision between the player and every asteroid that is currently in the scene
	/// If a collision is found, the asteroid gets destroyed (without splitting) and the player takes 1 damage. 
	/// </summary>
	void CheckCollide ()
	{
		
		for (int i = 0; i < asteroids.Length; i++) {
			objectDistance = asteroids [i].transform.position - ship.transform.position;

			//The Asteroid radius gets multiplied by the scale so that the collisions for the smaller asteroids isn't
			//using the radius of a regular sized asteroid
			if (objectDistance.magnitude < radius + (astRadius * asteroids [i].transform.lossyScale.x)) {// 
				
				DestroyAsteroid (i, false);
				ShipDamage ();
				gameObject.GetComponent<AstSpawn> ().astNum--;	
			}
		}
	}

	/// <summary>
	/// Increases the score and the Extra life counter
	/// Increments player health by 1 for every 1500 points earned 
	///</summary>
	/// <param name="scoreInc">Score inc.</param>
	void ScoreUp (int scoreInc)
	{
		score += scoreInc;
		extraLifeScore += scoreInc;

		//Player gains one extra life for getting 5000 points
		if (extraLifeScore >= 1500) {
			extraLifeScore -= 1500; 
			shipLives += 1;
		}
	}

	/// <summary>
	/// Checks for collision between the bullet's position and a circle that covers the entire asteroid
	/// If a collision is found, DestroyAsteroid() is called, the number of asteroids on the screen goes down by 1,
	/// And the bullet is destroyed if the powerup is not active.
	/// If powerUp1 is true, it makes bullets pierce.
	/// </summary>
	void CheckBulletCollide ()
	{
		for (int x = 0; x < bullets.Length; x++) {
			for (int y = 0; y < asteroids.Length; y++) {
				objectDistance = bullets [x].transform.position - asteroids [y].transform.position;

				if (objectDistance.magnitude < (astRadius * asteroids [y].transform.lossyScale.x)) {

					DestroyAsteroid (y, true);
					gameObject.GetComponent<AstSpawn> ().astNum--;
					gameObject.GetComponent<AstSpawn> ().AstCapCounter ();

					if (powerUp1 == false) {
					Destroy (bullets [x]);}
				}
			}
		}
	}

	/// <summary>
	/// Ship takes 1 damage, and upon reaching 0 shipLives the game "ends"
	/// </summary>
	void ShipDamage ()
	{
		shipLives--;
		if (shipLives == 0) {
			EndGame ();
		}
	}

	/// <summary>
	/// Modifies the text component of the Canvas in order to display lives and score
	/// </summary>
	void DisplayScore ()
	{
		displayText = "Ship Lives: " + shipLives + "      Score:: " + score.ToString ();
		if(powerUp1==true){displayText=displayText+"        POWERUP ACTIVE";}

		scoreTxt.text = displayText;
	}

	/// <summary>
	/// If the Asteroid is a 1st level, it will get destroyed, and 2 smaller asteroids will be spawned and score will increase by 20. 5% chance for powerup
	/// If the asteroid isn't 1st level, it will get destroyed and score will increase by 50. 10% chance for powerup
	/// Unless it is hit by player, then it will only get destroyed.
	/// </summary>
	/// <param name="ast">Ast is the index of the asteroid that was collided with</param>
	/// <param name="collided">Bool for whether it was an asteroid v. player colision or an asteroid vs. bullet collision</param>"> 

	void DestroyAsteroid (int ast, bool collided)
	{
		if (asteroids [ast].transform.lossyScale.x == 1 & collided ==true) {
			gameObject.GetComponent<AstSpawn> ().SpawnSecondLevel (asteroids [ast]);
			ScoreUp (20);
			if(Random.Range(0f,1f)>=.95f)
		{
			powerUp1 = true;
		}
		} else if (collided==true){
			ScoreUp (50);
			if(Random.Range(0f,1f)>=.90f)
		{
			powerUp1 = true;
		}
		}
		GameObject dest = asteroids [ast];
		Destroy (dest);

	}

	/// <summary>
	/// Destroys the scene manager
	/// </summary>
	void EndGame ()
	{
		Destroy (gameObject);
	}
	/// <summary>
	/// Dictates when the powerup ends.
	/// </summary>
	void PowerUpTimer()
	{
		timer += 1f * Time.deltaTime;
		if(timer>=3f){powerUp1 = false; timer = 0f;}
	}

	//Was used for visual debugging of collision
	/*void OnDrawGizmos ()
	{
		foreach (var item in asteroids) {
		
	
			Gizmos.DrawSphere (item.transform.position, astRadius);
		}
	}*/
}
