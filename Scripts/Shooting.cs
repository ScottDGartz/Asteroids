using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Author: Scott Gartz
///
/// Spawns a bullet at the player's position with the same rotation as the player
/// </summary>
public class Shooting : MonoBehaviour
{
	//Bullet prefab
	public GameObject bullet;
	//Used for instantiating new bullets
	private GameObject clone;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		Shoot ();
	}

	/// <summary>
	/// When space is pressed, a new bullet is spawned
	/// </summary>
	void Shoot ()
	{
		if(Input.GetKeyDown (KeyCode.Space))
		{
			clone = Instantiate ( bullet, transform.position,transform.rotation) as GameObject;

		}
	}

}
