using UnityEngine;
using System.Collections;

/// <summary>
/// Author: Scott Gartz
///
/// Dictates asteroid movement. Sets the direction that the asteroid moves in
/// Also handles wrapping of asteroids
/// </summary>
public class AstMovement : MonoBehaviour 
{
	//position of asteroid
	private Vector3 pos;
	//direction the asteroid is traveling in
	public Vector3 direction;
	//Velocity vector (asteroids don't accelerate)
	private Vector3 velocity;
	//Speed of the asteroid
	private float speed = 10f;
	//Stores the main Camera for use for wrapping
	private Camera mainCam;

	// Use this for initialization
	void Start () 
	{
		pos = transform.position;
		direction = Vector3.zero;
		velocity = Vector3.zero;
		mainCam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (direction == Vector3.zero) 
		{
			direction = new Vector3(Random.Range (-1f,1f),Random.Range(-1f,1f),0);
		}

		velocity = direction * speed * Time.deltaTime;
		pos += velocity;
		Wrap ();
		transform.position = pos;
	}

	/// <summary>
	/// If the asteroid goes off the screen, it wraps to the other side
	/// </summary>
	void Wrap()
	{
		Vector3 viewPos = mainCam.WorldToViewportPoint (pos);
		if(viewPos.x > 1 || viewPos.x < 0){pos.x = -pos.x;}
		if(viewPos.y > 1 || viewPos.y < 0){pos.y = -pos.y;}
	}	
}
