using UnityEngine;
using System.Collections;

/// <summary>
/// Author: Scott Gartz
/// 
/// Spawns asteroids based on how many are on the screen up to a scaling limit.
/// Collision.cs calls the AstCapCounter() method every time an asteroid is destroyed
/// </summary>
public class AstSpawn : MonoBehaviour 
{
    //The asteroid index
	public int astNum;
	//Stores the 5 asteroid prefabs
	public GameObject[] asteroids = new GameObject[5];
	//clone used for instantiating new asteroids
	private GameObject clone;
	//The cap of asteroid's to prevent unending spawning
	private int astNumCap;
	//Player's ship
	public GameObject ship;
	//Positon of asteroid
	private Vector3 astPos;
	//int to decide if the asteroid's spawn position is negative or positive
	int dir;
	//counter to cycle through different prefabs
	int k = 0;
	//counter for AstCapCounter() to hold the number needed to be reached to scale up the astNumCap
	int astCounter;

	// Use this for initialization
	void Start () 
	{
		astNumCap = 1;
	}
	
	// Update is called once per frame
	void Update () 
	{
	    //As long as the number of asteroids that exist is below the cap, more will spawn
		if (astNum < astNumCap) 
		{
			SpawnAsteroid();                                                                                                                                                                                                
		}
	}

	/// <summary>
	/// Spawns as many asteroids as needed until the cap is reached. 
    /// Places new asteroids atleast 4f away from the player.
    /// Randomly decides if the spawn position is negative or positive
	/// </summary>
	void SpawnAsteroid()
	{
		for (int i = astNum; i < astNumCap; i++) 
		{
			astPos = new Vector3 (Random.Range (4f,20f),Random.Range(4f,20f),0);
			for (int j = 0; j < 2; j++) 
			{
				if (j==0)
				{
					dir = Random.Range (0,2);
					if(dir==0)
					{
						astPos.x = -astPos.x;
					}
				}
				else
				{
					dir = Random.Range (0,2);
					if(dir==0)
					{
						astPos.y = -astPos.y;
					}
				}
			}
			astPos = ship.transform.position + astPos;
			clone = Instantiate (asteroids [k],astPos,Quaternion.identity) as GameObject;
			clone.transform.parent = this.transform;
			astNum++;
			k++;
			if(k==5){k = 0;}
		}
	}
	/// <summary>
	/// Called when a first level asteroid is split.
	/// Spawns two new asteroids where the 1st level asteroid was. Changes direction they travel in
	/// Scales them down by .5f
	/// </summary>
	/// <param name="astParent">Ast parent.</param>
	public void SpawnSecondLevel (GameObject astParent)
	{
		Vector3 tempPos = astParent.transform.position;
		for (int a = 0; a < 2; a++) {
			tempPos = tempPos * .98f;
			clone = Instantiate (asteroids [k], tempPos, Quaternion.identity) as GameObject;
			clone.transform.localScale = clone.transform.localScale * .5f;
			clone.transform.parent = this.transform;
			astNum++;
			k++;
			if (k == 5) {
				k = 0;
			}
		}

	}

	/// <summary>
	/// Scales up the cap for the maximum amount of asteroids
	/// </summary>
	public void AstCapCounter()
	{
		astCounter++;
		if(astCounter>= (int)(astNumCap*1.5))
		{
			int prevCap = astNumCap;
			astNumCap = (int)(astNumCap * 1.2);
			if(astNumCap==prevCap)
			{
				astNumCap++;
			}
			astCounter = 0;
		}
	}

}
