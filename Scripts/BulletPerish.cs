using UnityEngine;
using System.Collections;

/// <summary>
/// Author: Scott Gartz
///
/// Handles the timing of bullets so that they despawn after a certain amount of time. 
/// Also handles movement and wrapping of bullets
/// </summary>
public class BulletPerish : MonoBehaviour {
    //Timer for bullets
	private float perishTimer;
	//Speed of bullets
	private float speed;
	//Direction the bullet is traveling
	private Vector3 dir;
	//Velocity of the bullets
	private Vector3 velo;
	//Position of the bullet
	private Vector3 bulletPos;
	//Holds the rotation of the bullet
	private Quaternion rot;
	//Holds Main Camera for wrapping purposes
	private Camera mainCam;


	// Use this for initialization
	void Start () 
	{
		bulletPos = transform.position;
		dir = new Vector3 (0, 1, 0);
		speed = 50f;
		rot = transform.rotation;
		velo = rot * dir * speed;
		mainCam = Camera.main;
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		Move ();
		Perish ();

	}
	/// <summary>
	/// Destroys the bullet as soon as the timer is reached
	/// </summary>
	void Perish(){
		perishTimer += 1f * Time.deltaTime;
		if (perishTimer >= 1.5) 
		{
			Destroy (gameObject);
		}
	}
	/// <summary>
	/// Moves the bullet through the world
	/// </summary>
	void Move()
	{
		bulletPos += velo * Time.deltaTime;
		Wrap ();
		transform.position = bulletPos;
	}
	/// <summary>
	/// Wraps bullet when it reaches end of screen	/// </summary>
	void Wrap()
	{
		Vector3 viewPos = mainCam.WorldToViewportPoint (bulletPos);
		if(viewPos.x > 1 || viewPos.x < 0)
		{bulletPos.x=-bulletPos.x;}
		if(viewPos.y > 1 || viewPos.y < 0)
		{bulletPos.y=-bulletPos.y;}
	}
}
