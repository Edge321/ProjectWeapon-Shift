using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoll : MonoBehaviour
{
	public float rollDistance = 5.0f;
	public float rollDuration = 3.0f;
	public float rollCooldown = 3.0f;

	public LayerMask environment;

	private bool hitObstacle = false;

	private CapsuleCollider capsuleCollider;

	private NewPlayerMovement playerMovement;

	private Rigidbody rb;

	private RigidbodyConstraints freezeY = (RigidbodyConstraints)116;
	private RigidbodyConstraints unFreezeY = RigidbodyConstraints.FreezeRotation;

	/// <summary>
	/// Cooldown for rolling
	/// </summary>perh
	public float CoolDown { private set; get; }
	public bool IsRolling { private set; get; } = false;
	void Awake()
	{
		playerMovement = GetComponent<NewPlayerMovement>();
		capsuleCollider = GetComponent<CapsuleCollider>();
		rb = GetComponent<Rigidbody>();

		CoolDown = 0;
	}
	private void Update()
	{
		CoolDown = Mathf.Clamp(CoolDown - Time.deltaTime, 0, rollCooldown);
	}
	/// <summary>
	/// Player rolls in the direction they wanted to walk to. Distance determined by <c>rollDistance</c>
	/// </summary>
	/// <param name="direction">Player the direction is facing</param>
	/// <param name="camEuler">Location of player camera</param>
	public void Rolling(Vector3 direction, Vector3 camEuler)
	{
		playerMovement.CanMove = false;
		capsuleCollider.enabled = false;
		IsRolling = true;
		CoolDown = rollCooldown;

		float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camEuler.y;
		Vector3 moveDirection = Quaternion.Euler(0, angle, 0) * Vector3.forward;
		//make the character face where he is rolling
		transform.rotation = Quaternion.Euler(0, angle, 0);
		Vector3 rollDestination = transform.position + (moveDirection * rollDistance);

		//rb.AddForce(direction * 100f, ForceMode.Impulse);

		StartCoroutine(RollingLength(rollDestination, direction));
	}
	IEnumerator RollingLength(Vector3 destination, Vector3 direction)
	{
		float timeElapsed = 0;
		Vector3 start = transform.position;
		//TODO: Figure out how to make player not phase through walls
		while (timeElapsed < rollDuration)
		{
			rb.constraints = freezeY;
			Vector3 moddedLine = transform.position + new Vector3(0, 1, 0);

			Debug.DrawLine(moddedLine, destination + new Vector3(0, 1, 0), Color.red);
			/**if (Physics.Raycast(moddedLine, destination + new Vector3(0, 1, 0), 3, environment))
			{
				Debug.Log("Obstacle hit!");
				hitObstacle = true;
				break;
			}**/

			//Debug.DrawLine(start, destination);
			rb.MovePosition(Vector3.Lerp(start, destination, timeElapsed / rollDuration));
			//rb.MovePosition(Vector3.MoveTowards(rb.position, destination, 30 * Time.deltaTime));
			//transform.position = Vector3.Lerp(start, destination, timeElapsed / rollDuration);
			//rb.velocity = Vector3.Lerp(start, destination, timeElapsed / rollDuration);
			timeElapsed += Time.deltaTime;

			yield return null;
		
		}
		//If the player hits an obstacle they will be stopped mid roll
		if (hitObstacle)
			hitObstacle = false;
		//This is to make sure the player lerps to the destination
		else
			//transform.position = destination;
			rb.MovePosition(destination);

		//rb.AddForce(direction * 5000f, ForceMode.Impulse);

		IsRolling = false;
		capsuleCollider.enabled = true;
		playerMovement.CanMove = true;
		rb.constraints = unFreezeY;
	}
	/**private void OnDrawGizmos()
	{
		Vector3 moddedLine = transform.position + new Vector3(0, capsuleCollider.height, 0);
		Vector3 gizmoLine = moddedLine + (Vector3.forward * 5);
		Gizmos.DrawLine(moddedLine, gizmoLine);

		moddedLine = transform.position + new Vector3(0, 1, 0);
		gizmoLine = moddedLine + (Vector3.forward * 1);
		Gizmos.DrawLine(moddedLine, gizmoLine);

		moddedLine = transform.position + new Vector3(0, capsuleCollider.height / (float) 2 , 0);
		gizmoLine = moddedLine + (Vector3.forward * 5);
		Gizmos.DrawLine(moddedLine, gizmoLine);
	}**/
}
