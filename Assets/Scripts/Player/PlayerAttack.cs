using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
	public float attackPoseTime = 3.0f;
	[Tooltip("How early the player can continue their combo. Higher is earlier")]
	[Range(0, 1f)]
	public float comboWindowTime = 0.7f;

	public AnimationClip[] attackAnimations;

	public GameObject[] weaponObjects;
	public string CurrentAnimation { get; private set; }
	public float TempAnimationLength { get; private set; }
	public float BlockTime { get; private set; } = 0;
	public int AnimationNumber { get; private set; } = 0;

	private float tempAttackPoseTime = 0;
	private float animationLength;
	private float minAnimationLength = 0;
	private float animationRatio;
	
	private int currentWeapon = 0;
	//116 is RigidbodyConstraint's FreezePositionY and FreezeRotation are added up
	private RigidbodyConstraints freezeY = (RigidbodyConstraints)116;
	private RigidbodyConstraints unFreezeY = RigidbodyConstraints.FreezeRotation;

	private Animator animator;

	private Rigidbody rb;

	private WeaponCharacteristics[] weaponScripts;
	private NewPlayerMovement playerMovement;
	private PlayerRoll playerRoll;
	private PlayerJump playerJump;
	void Awake()
	{
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();
		playerMovement = GetComponent<NewPlayerMovement>();
		playerRoll = GetComponent<PlayerRoll>();
		playerJump = GetComponent<PlayerJump>();

		AssignWeaponArrays();
	}
	void Update()
	{
		bool lightButton = Input.GetKeyDown(KeyCode.Mouse0);
		bool heavyButton = Input.GetKeyDown(KeyCode.Mouse1);
		bool blockButton = Input.GetKey(KeyCode.R);

		if (!playerRoll.IsRolling)
		{
			if (lightButton)
				AttackCombo("Light");
			else if (heavyButton)
				AttackCombo("Heavy");
			else if (blockButton && TempAnimationLength <= minAnimationLength)
				SetAttackStatus(WeaponStatus.BLOCK);
		}

		ChangeTempAnimationLength();
		BlockStatus();
		EndOfAttack();
		SetAttackPose();

		if (weaponScripts[currentWeapon].GetAttackStatus() == WeaponStatus.ATTACK)
			rb.velocity = Vector3.zero;

		animationRatio = TempAnimationLength / animationLength;
	}
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
	/// Checks what attack in an attack combo the player is on.
	/// Also enables the hitbox for the weapon.
	/// </summary>
	/// <param name="attackType">Attack is light or strong</param>
	private void AttackCombo(string attackType)
	{
		string attack = attackType + "Attack_";

		//animationRatio is used for continuing an attack combo
		if ((animationRatio > minAnimationLength && animationRatio < comboWindowTime)
			|| AnimationNumber == 1)
		{
			string animationName = attack + AnimationNumber;
			AnimationNumber++;
			//condition is in case player spams mouse and increments AnimationNumber too much
			if (weaponScripts[currentWeapon].CheckAnimation(animationName))
			{
				string triggerType = attackType + "Trigger";
				animator.SetTrigger(triggerType);

				playerMovement.CanMove = false;

				animationLength = WeaponCharacteristics.GetAnimationLength(weaponScripts, animationName);
				TempAnimationLength = animationLength;
				
				CurrentAnimation = animationName;
				tempAttackPoseTime = attackPoseTime;

				SetAttackStatus(WeaponStatus.ATTACK);
				rb.constraints = freezeY;
			}
		}
	}
	private void ChangeTempAnimationLength()
	{
		if (TempAnimationLength > minAnimationLength)
			TempAnimationLength = Mathf.Clamp(TempAnimationLength - Time.deltaTime, 
											minAnimationLength, animationLength);
	}
	private void SetAttackStatus(WeaponStatus status)
	{
		weaponScripts[currentWeapon].SetAttackStatus(status);
	}
	public WeaponStatus GetAttackStatus()
	{
		return weaponScripts[currentWeapon].GetAttackStatus();
	}
	/// <summary>
	/// Resets necessary variables to player to be able to move and start a combo
	/// </summary>
	private void EndOfAttack()
	{
		if (TempAnimationLength <= minAnimationLength)
		{
			if (!playerRoll.IsRolling)
				playerMovement.CanMove = true;

			AnimationNumber = 1;
			CurrentAnimation = null;
			rb.constraints = unFreezeY;
			SetAttackStatus(WeaponStatus.IDLE);
		}
	}
	/// <summary>
	/// Sets the time for how long the player has had their block stance
	/// </summary>
	private void BlockStatus()
	{
		//TODO: Add a cooldown so the player can't spam block to get a parry
		if (weaponScripts[currentWeapon].GetAttackStatus() == WeaponStatus.BLOCK)
			BlockTime += Time.deltaTime;
		else
			BlockTime = 0;
	}
	private void SetAttackPose()
	{
		tempAttackPoseTime = Mathf.Clamp(tempAttackPoseTime - Time.deltaTime,
										minAnimationLength, attackPoseTime);
		animator.SetFloat("Offense", tempAttackPoseTime);
	}
	public int GetAnimationClipLength()
	{
		return attackAnimations.Length;
	}
}
