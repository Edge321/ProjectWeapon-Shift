using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeBehavior : MonoBehaviour
{
    public float maxHealth = 50.0f;
	public float knockAwayForce = 1000.0f;
	[Range (0f, 1f)]
	public float blockDamageModifier = 0.2f;
	public bool DeadPlayer { get; private set; } = false;

    private float currentHealth;
	private float animationLength = 0;

	private bool alreadyHit = false;

	private Rigidbody rb;

	private PlayerAttack attack;
	private PlayerBlock block;
	private NewPlayerMovement movement;
	private void Awake()
	{
		currentHealth = maxHealth;

		rb = GetComponent<Rigidbody>();
		attack = GetComponent<PlayerAttack>();
		movement = GetComponent<NewPlayerMovement>();
		block = GetComponent<PlayerBlock>();
	}
	void Update()
    {
		animationLength = Mathf.Clamp(animationLength - Time.deltaTime, 0, animationLength); 

		if (Mathf.Approximately(currentHealth, 0))
		{
			Debug.Log("Dead!");
			DeadPlayer = true;
		}
		//Prevents player from being hit twice by the same animation
		if (Mathf.Approximately(animationLength, 0))
			alreadyHit = false;
    }
	private void OnTriggerEnter(Collider other)
	{
		WeaponCharacteristics weapon = other.GetComponent<WeaponCharacteristics>();
        EnemyBehavior enemy = other.GetComponentInParent<EnemyBehavior>();

		//If player was hit by enemy with their weapon
        if (weapon != null && enemy != null)
			HitByEnemy(weapon, enemy);
	}
	private void HitByEnemy(WeaponCharacteristics weapon, EnemyBehavior enemy)
	{
		if (!alreadyHit)
		{
			AttackType enemyAttackType = weapon.GetAnimationType(enemy.CurrentAnimation);
			float damage = weapon.GetDamage(enemy.CurrentAnimation);

			if (!Mathf.Approximately(attack.BlockTime, 0))
			{
				if (attack.BlockTime > block.GetParryWindow())
				{
					damage *= blockDamageModifier;
					HealthModifier(-damage, enemy);
				}
				else if (attack.BlockTime <= block.GetParryWindow())
				{
					Debug.Log("Parried hit, undamaged!");
					Debug.Log("Block time: " + attack.BlockTime);
					enemy.Interuppted();
				}
			}
			else
				HealthModifier(-damage, enemy);

			alreadyHit = true;
			//Player getting knocked down depending on strength of attack
			if (enemyAttackType == AttackType.MODERATE ||
				enemyAttackType == AttackType.STRONG)
			{
				Debug.Log("Knocked!");
			}
			else if (enemyAttackType > AttackType.STRONG)
			{
				Debug.Log("Knocked Away!");
				//rb.AddForce(Vector3.one * knockAwayForce, ForceMode.Impulse);
				//movement.CanMove = false;
				//TODO: Find the enemy object and then knock the player opposite of the enemy
			}
		}
	}
	private void HealthModifier(float damage, EnemyBehavior enemy)
	{
		currentHealth = Mathf.Clamp(currentHealth + damage, 0, maxHealth);
		animationLength = enemy.AnimationTime;
		Debug.Log("Hit for " + damage.ToString("F2") + ". Current health is " + currentHealth);
	}
}
