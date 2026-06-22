using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyBehavior : MonoBehaviour
{
	private enum AIStage
	{
		PATROL, CHASE, MONITOR, ATTACK_CHASE, ATTACK
	};

	public bool stationaryAI = false;
	
	public LayerMask whatIsEnvironment;
	public LayerMask whatIsPlayer;

	public GameObject[] weaponObjects;

	private float collisionDistanceCheck = 2.0f;

	private NavMeshAgent agent;

	private Animator animator;

	private Transform playerLocation;

	private Vector3 point;
	private bool pointSet = false;

	//Patrolling
	public float walkPointRange = 20.0f;

	//Look Around
	public float lookTime = 2.0f;
	private bool lookAround;

	//Monitoring
	public float monitorMovement = 5.0f;
	private bool boolCheck = true; //Prevents point being set multiple times

	/* Have a temp Monitor Range that has radius increased when player in monitor range
	*  If player leaves monitor range, change back to normal
	*/

	//Chasing
	private bool chasedEnough = false;

	//Attacking
	public float attackDelay = 2.0f;
	[Tooltip("The higher the number, the more aggressive the AI will be")]
	[Range(1, 20)]
	public int aggresion = 3;
	public string CurrentAnimation { get; private set; }
	public float AnimationTime { get; private set; } = 0;
	private int maxRandomAggression = 21;
	private bool attackReady = false;
	private bool alreadyAttacked;
	private float animationDelay = 0.25f;
	private WeaponCharacteristics[] weaponScripts;

	//States
	public float sightRange = 10.0f;
	public float monitorRange = 6.0f;
	public float attackRange = 3.0f;

	void Awake()
	{
		playerLocation = GameObject.Find("Protagonist").transform;
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
		AssignWeaponArrays();
	}
	private void Update()
	{
		AIStage aiStage = AIStage.PATROL;

		if (Physics.CheckSphere(transform.position, sightRange, whatIsPlayer))
			aiStage = AIStage.CHASE;
		if (Physics.CheckSphere(transform.position, monitorRange, whatIsPlayer))
			aiStage = AIStage.MONITOR;
		if (attackReady)
			aiStage = AIStage.ATTACK_CHASE;

		Debug.Log(aiStage.ToString());

		if (aiStage < AIStage.MONITOR)
			chasedEnough = false;

		AnimationTime = Mathf.Clamp(AnimationTime - Time.deltaTime, 0, AnimationTime);

		if (Mathf.Approximately(AnimationTime, 0))
			alreadyAttacked = false;
			
		animator.SetFloat("Speed", agent.velocity.magnitude);

		DecideAI(aiStage);
	}
	/// <summary>
	/// State of AI will be chosen through a switch statement
	/// </summary>
	/// <param name="aiStage"></param>
	private void DecideAI(AIStage aiStage)
	{
		switch (aiStage)
		{
			case AIStage.PATROL:
				Patrolling();
				break;
			case AIStage.CHASE:
				ChasePlayer();
				break;
			case AIStage.MONITOR:
				MonitorPlayer();
				break;
			case AIStage.ATTACK_CHASE:
				ChaseNAttack();
				break;
			//case AIStage.ATTACK:
				//AttackPlayer();
				//break;
			default:
				Debug.LogError("Error: Invalid AI stage for " + gameObject.name);
				break;
		}
	}
	/// <summary>
	/// Initializes and assigns the scripts and colliders the enemy's weapon has
	/// </summary>
	private void AssignWeaponArrays()
	{
		weaponScripts = new WeaponCharacteristics[weaponObjects.Length];
		for (int i = 0; i < weaponObjects.Length; i++)
		{
			weaponScripts[i] = weaponObjects[i].GetComponent<WeaponCharacteristics>();

			if (weaponScripts[i] == null)
				Debug.LogError(weaponObjects[i].name + 
					" does not have the WeaponCharacteristics script");
		}
	}
	/// <summary>
	/// Either obtain new coordinates to walk to or keep walking to the provided destination
	/// </summary>
	private void Patrolling()
	{
		if (!pointSet)
			SearchPoint(walkPointRange);
		else
		{
			if (!agent.SetDestination(point))
				Debug.Log("Unable to set patrolling destination");
		}

		Vector3 distanceToWalkPoint = transform.position - point;
		//If the enemy is near the chosen coordinate
		if (distanceToWalkPoint.x <= 1.0f && distanceToWalkPoint.z <= 1.0f)
		{
			//walkPointSet = false;
			pointSet = false;
			lookAround = true;
		}
			
	}
	/// <summary>
	/// After reaching their destination, will idle for <c>lookTime</c> seconds before moving on
	/// </summary>
	private void LookAround()
	{
		animator.SetFloat("Speed", 0);
		StartCoroutine(LookAroundTimer());
	}
	IEnumerator LookAroundTimer()
	{
		yield return new WaitForSeconds(lookTime);
		lookAround = false;
	}
	private void ChasePlayer()
	{
		if (!agent.SetDestination(playerLocation.transform.position))
			Debug.Log("Unable to chase player");
		//TODO: what the hell is going on in this? Might not need to worry about this method TBH
		Vector3 distanceToWalkPoint = transform.position - playerLocation.transform.position;

		//If the enemy is near the chosen coordinate
		if (distanceToWalkPoint.x <= 1.0f && distanceToWalkPoint.z <= 1.0f)
		{
			//pointSet = false;
			chasedEnough = true;
			agent.ResetPath();
		}
	}
	private void MonitorPlayer()
	{
		//Enemy automatically close enough to player, not chasing anymore
		chasedEnough = false;
		transform.LookAt(playerLocation);

		if (!pointSet)
			SearchPoint(monitorMovement);
		else
		{
			if (!agent.SetDestination(point))
				Debug.Log("ERROR: Destination unable to be set");
		}

		Vector3 distanceToWalkPoint = transform.position - point;

		//If the enemy is near the chosen coordinate
		if (distanceToWalkPoint.x <= 0.1f && distanceToWalkPoint.z <= 0.1f)
		{
			float randomTime = Random.Range(0.0f, 5.0f);
			//This bool prevents coroutine from being called multiple times
			if (boolCheck)
			{
				boolCheck = false;
				StartCoroutine(MakeMonitorPointFalse(randomTime));
			}
		}
	}
	IEnumerator MakeMonitorPointFalse(float time)
	{
		//Debug.Log(time);
		yield return new WaitForSeconds(time);
		pointSet = false;
		boolCheck = true;
		attackReady = ShouldAttack();
	}
	private bool ShouldAttack()
	{
		int attackRandom = Random.Range(0, maxRandomAggression);

		if (attackRandom <= aggresion)
			return true;
		else
			return false;
	}
	private void SearchPoint(float pointRange)
	{
		float randomX = Random.Range(-pointRange, pointRange);
		float randomZ = Random.Range(-pointRange, pointRange);

		point = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

		if (Physics.Raycast(point, -transform.up, collisionDistanceCheck, whatIsEnvironment))
			pointSet = true;
	}
	/// <summary>
	/// Chases the player and attacks them when they are close enough
	/// </summary>
	private void ChaseNAttack()
	{
		agent.SetDestination(playerLocation.transform.position);

		if (Physics.CheckSphere(transform.position, attackRange, whatIsPlayer))
		{
			Debug.Log("Time to attack!");
			//pointSet = true;
			AttackPlayer();
		}
			
	}
	/// <summary>
	/// When near the player, will start attacking them at an interval set by <c>attackDelay</c>
	/// </summary>
	private void AttackPlayer()
	{
		agent.SetDestination(transform.position);

		animator.SetFloat("Speed", 0);

		if (Mathf.Approximately(AnimationTime, 0))
			transform.LookAt(playerLocation.transform);

		if (!alreadyAttacked)
		{
			//int randomAttack = Random.Range(1, 3);
			int randomAttack = 1;
			CurrentAnimation = "Attack_" + randomAttack;
			animator.SetTrigger(CurrentAnimation);
			AnimationTime = GetAnimationTime(CurrentAnimation) + animationDelay;
			alreadyAttacked = true;
			attackReady = false;
		}
	}
	private float GetAnimationTime(string animationName)
	{
		return WeaponCharacteristics.GetAnimationLength(weaponScripts, animationName);
	}
	public void Interuppted()
	{
		animator.SetTrigger("Damaged");
		alreadyAttacked = false;
		AnimationTime = 0;
	}
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, sightRange);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, monitorRange);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, attackRange);
	}
}
