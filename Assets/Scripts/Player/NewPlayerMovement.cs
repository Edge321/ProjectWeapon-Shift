using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerMovement : MonoBehaviour
{
	public float walkSpeed = 10f;
	public float runSpeed = 12f;
	public float turnSmooth = 0.1f;

	public Transform camTransform;

	private float turnSmoothVelocity;

	private Vector3 lastDirection;
	private Vector3 lastCamEuler;

	private Animator animator;

	private Rigidbody rb;

	private PlayerRoll playerRoll;
	public bool CanMove { get; set; } = true;
	private void Awake()
	{
		lastDirection = Vector3.forward;
		lastCamEuler = camTransform.eulerAngles;

		animator = GetComponent<Animator>();
		playerRoll = GetComponent<PlayerRoll>();
		rb = GetComponent<Rigidbody>();
	}
	private void FixedUpdate()
	{
		float horizontal = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw("Vertical");
		float canRun = Input.GetAxis("Shift");
		bool roll = Input.GetKeyDown(KeyCode.E);

		Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

		if (roll && CanMove && playerRoll.CoolDown <= 0)
			playerRoll.Rolling(lastDirection, lastCamEuler);

		if (direction.magnitude > 0 && CanMove)
		{
			if (canRun > 0)
				WalkOrRun(runSpeed, direction);
			else
				WalkOrRun(walkSpeed, direction);
			//Takes into account if a player rolls while being still
			lastDirection = direction;
			lastCamEuler = camTransform.eulerAngles;
		}
		else
		{
			//Set to 0 to prevent unwanted animation transitions
			animator.SetFloat("Speed", 0);

			if (!playerRoll.IsRolling)
			{
				//Stops player from continuing to move
				Vector3 velocity = new Vector3(0, 0, 0);
				velocity.y = rb.velocity.y;
				rb.velocity = velocity;
			}
		}

		//Debug.Log(CanMove);
	}
	/// <summary>
	/// Character walks or run depending on if shift key is pressed
	/// Sets character rotation to where the camera is and what keys the player presses
	/// </summary>
	private void WalkOrRun(float speed, Vector3 direction)
	{
		//Gets the angle for where the player will move on the x and z plane and adds the y eulerAngles of
		//the camera for the player to move where the camera is facing
		float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camTransform.eulerAngles.y;
		//Smoothens the rotation of the player
		float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmooth);
		transform.rotation = Quaternion.Euler(0, angle, 0);

		//Converts Quaternion to a Vector3
		Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

		//Velocity used so player doesn't go through walls
		Vector3 velocity = moveDirection * speed;
		velocity.y = rb.velocity.y; //Apply gravity since there is none at first
		rb.velocity = velocity;

		animator.SetFloat("Speed", direction.magnitude * speed);
	}
}
