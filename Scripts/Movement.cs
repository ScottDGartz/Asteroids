using UnityEngine;
using System.Collections;
/// <summary>
/// Author: Scott Gartz
///
/// Handles movement of the player. Player can turn with A and D keys, and moves forward
/// with the W key. Upon going off the screen, the player wraps to the other side
/// </summary>
public class Movement : MonoBehaviour {

	//Position of the player in the world
	private Vector3 shipPos;
	//Direction vector for the player
	private Vector3 dir;
	//Velocity of the player
	private Vector3 velo;
	//Acceleration of the player
	private Vector3 accel;
	//Rate at which the player acclerates
	private float accelRate=7f;
	//The max speed the player can move at
	private float maxSpeed = 25f;
	//Total rotation the player has undergone
	private float totalRot = 0;
	//Used for the speed of rotation
	private float angle= 5;
	//Stores Main Camera for wrapping
	private Camera cam;

	// Use this for initialization
	void Start () 
	{
		accel = Vector3.zero;
		velo = Vector3.zero;
		dir = new Vector3(0,1,0);
		shipPos = transform.position;
		cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Turn ();
		Move ();
		Wrapping ();
		transform.position = shipPos;

			
	}
	/// <summary>
	/// When "A" is pressed, the player rotates to the left, and "D" rotates player to the right
	/// </summary>
	void Turn()
	{
		if (Input.GetKey (KeyCode.A)) 
		{
			dir = (Quaternion.Euler (0,0,angle))*dir;
			totalRot+=angle;
			transform.Rotate(0,0,angle);
		}
		if (Input.GetKey (KeyCode.D)) 
		{
			dir = Quaternion.Euler (0,0, -angle) * dir;
			totalRot -= angle;
			transform.Rotate(0,0,-angle);
		}

	}
	/// <summary>
	/// Calculates the player's velocity based on acceleration, direction and Time.deltatime
	/// Added to position to move the player. All while "W" is down.
	/// If "W" is not being held down, the player decelerates to 0;
	/// </summary>
	void Move()
	{
		if (Input.GetKey(KeyCode.W)) {
			accel = dir * accelRate * Time.deltaTime;
			velo += accel;
			velo = Vector3.ClampMagnitude (velo, maxSpeed);
			shipPos += velo * Time.deltaTime;
			transform.position = shipPos;
		} 
		else {
			velo = velo * .98f;

			if (velo.sqrMagnitude < 0.000001f) {
				velo = Vector3.ClampMagnitude (velo, 0);
			}
			shipPos += velo * Time.deltaTime;

		}
	}
	/// <summary>
	/// Upon exiting the screen, the player wraps to the other side
	/// </summary>
	void Wrapping(){
		Vector3 camPos = cam.WorldToViewportPoint (shipPos);
		if (camPos.x < 0 || camPos.x > 1) {
			shipPos.x = -shipPos.x;
		}
		if (camPos.y < 0 || camPos.y > 1) {
			shipPos.y = -shipPos.y;
		}

	}
}
