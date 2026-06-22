using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerCamera : MonoBehaviour
{
	public GameObject followObject;
	public float mouseSensitivity = 2.0f;

	private float xMovement;
	private float zMovement;
	private float firstMousePositionX;
	private float firstMousePositionY;
	private void Awake()
	{
		firstMousePositionX = Input.mousePosition.x;
		firstMousePositionY = Input.mousePosition.y;
	}
	// Update is called once per frame
	void Update()
	{
		xMovement = Input.GetAxis("Horizontal");
		zMovement = Input.GetAxis("Vertical");

		CameraRotation();
	}
	/// <summary>
	/// Rotates the player's camera based on mouse movement
	/// </summary>
	private void CameraRotation()
	{
		float secondMousePositionX = Input.mousePosition.x;
		float secondMousePositionY = Input.mousePosition.y;
		
		float deltaMouseX = secondMousePositionX - firstMousePositionX;
		float deltaMouseY = firstMousePositionY - secondMousePositionY;

		//How the camera moves based off of how fast the mouse is moving and mouse sensitivity
		followObject.transform.rotation *= Quaternion.AngleAxis(deltaMouseX / mouseSensitivity, Vector3.up);
		followObject.transform.rotation *= Quaternion.AngleAxis(deltaMouseY / mouseSensitivity, Vector3.right);

		followObject.transform.position = transform.position + new Vector3(0, 3.82f, 0);

		Vector3 angles = followObject.transform.localEulerAngles;
		angles.z = 0; //Locks camera into x and y directions

		float angleX = followObject.transform.localEulerAngles.x;

		//Angles not done in negatives as camera acts weird when setting negative values
		//Clamps X angle so camera does not go too high or too low
		if (angleX > 180 && angleX < 340)
			angles.x = 340;
		else if (angleX < 180 && angleX > 40)
			angles.x = 40;

		followObject.transform.localEulerAngles = angles;
		//Assigned to second position for next deltaMouse
		firstMousePositionX = secondMousePositionX;
		firstMousePositionY = secondMousePositionY;
		//Able to move camera around without moving the character
		if ((xMovement == 0 && zMovement == 0) || !PlayerMovement.CanMove)
			return;

		//Set character rotation based on the followObject rotation
		transform.rotation = Quaternion.Euler(0, followObject.transform.rotation.eulerAngles.y, 0);
		//Resets the y rotation of the look transform
		//followObject.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
	}
}
