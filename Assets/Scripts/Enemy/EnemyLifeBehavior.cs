using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyLifeBehavior : MonoBehaviour
{
	public float maxHealth = 20.0f;
	public float textYOffset = 3;

	public GameObject textDamage;

	private float health;
	private float attackLength;

	private bool[] animationHits;

	private Vector3 damageTextOffset;

	private PlayerAttack playerAttack;

	private EnemyBehavior enemyBehavior;

	private GameObject camObject;
	private void Awake()
	{
		health = maxHealth;

		enemyBehavior = GetComponent<EnemyBehavior>();

		camObject = Camera.main.gameObject;

		damageTextOffset = new Vector3(0, textYOffset, 0);
	}
	private void Update()
	{
		//Checks when player's attack animation is over
		if (playerAttack != null)
		{
			attackLength -= Time.deltaTime;
			if (attackLength < 0)
				ResetHits();
		}
	}
	private void OnTriggerEnter(Collider other)
	{
		WeaponCharacteristics characteristics = other.GetComponent<WeaponCharacteristics>();
		playerAttack = other.GetComponentInParent<PlayerAttack>();

		if (characteristics != null && playerAttack != null)
			HitByPlayer(characteristics);
	}
	/// <summary>
	/// Gets damage from player depending on their attack animation
	/// </summary>
	private void HitByPlayer(WeaponCharacteristics weapon)
	{
		int hitsIndex;
		//Prevents animationHits from being initialized again
		if (animationHits == null)
		{
			//TODO: figure out better way to get animation length from player. Preferably from their weapon
			animationHits = new bool[playerAttack.GetAnimationClipLength()];
			ResetHits();
		}
		hitsIndex = Mathf.Clamp(playerAttack.AnimationNumber - 2, 0, animationHits.Length - 1);
	
		try
		{
			//Prevents the same animation from hitting an enemy multiple times
			if (animationHits[hitsIndex])
				return;
			else
			{
				animationHits[hitsIndex] = true;
				attackLength = playerAttack.TempAnimationLength;
				if (weapon.GetAnimationType(playerAttack.CurrentAnimation) >= AttackType.STRONG)
					enemyBehavior.Interuppted();
			}
		} catch (System.Exception)
		{
			Debug.LogWarning(hitsIndex + " is out of bounds for animation hits");
		}

		ChangeHealth(-weapon.GetDamage(playerAttack.CurrentAnimation));
	}
	/// <summary>
	/// Resets animation attacks that the enemy was hit with
	/// </summary>
	private void ResetHits()
	{
		//Error handling for if player enters enemy's trigger
		//as animationHits will be null if no attack was made by the player
		try
		{
			for (int i = 0; i < animationHits.Length; i++)
				animationHits[i] = false;
		} catch (System.Exception)
		{
			playerAttack = null;
		}
	}
	/// <summary>
	/// Sets the enemy's health, animation, health bar, health bar text, and damage animation
	/// </summary>
	/// <param name="modifier"></param>
	private void ChangeHealth(float modifier)
	{
		health = Mathf.Clamp(health + modifier, 0, health);

		GameObject damage = Instantiate(textDamage, transform.position + damageTextOffset, Quaternion.identity);
		DamageTextBehavior damageText = damage.GetComponent<DamageTextBehavior>();
		damageText.SetNumber(modifier);

		if (health <= 0)
			Death();
	}
	private void Death()
	{
		Destroy(gameObject);
	}
}
