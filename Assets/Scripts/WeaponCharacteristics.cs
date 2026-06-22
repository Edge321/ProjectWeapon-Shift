using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attacks
{
	public AnimationClip[] attackClips;
	public AttackType[] attackTypes;
	public float[] attackDamage;
}
[System.Serializable]
public enum AttackType
{
	WEAK, LIGHT, MODERATE, STRONG, HEFTY
}
[System.Serializable]
public enum WeaponStatus
{
	IDLE, ATTACK, BLOCK
}
public class WeaponCharacteristics : MonoBehaviour
{
	public Attacks[] allAttacks;

	private Dictionary<string, float> animationDamage = new Dictionary<string, float>();
	private Dictionary<string, float> animationLength = new Dictionary<string, float>();
	private Dictionary<string, AttackType> animationType = new Dictionary<string, AttackType>();

	private WeaponStatus status = WeaponStatus.IDLE;

	//Will be used for when the katana shoots out its bullets
	//private float modifiedDamage;
	void Start()
	{
		StoreAll();
	}
	/// <summary>
	/// Puts all animation damages, length, and attack types to their respective animation in dictionaries
	/// </summary>
	private void StoreAll()
	{
		for (int i = 0; i < allAttacks.Length; i++)
		{
			try
			{
				for (int k = 0; k < allAttacks[i].attackClips.Length; k++)
				{
					animationDamage.Add(allAttacks[i].attackClips[k].name, allAttacks[i].attackDamage[k]);
					animationType.Add(allAttacks[i].attackClips[k].name, allAttacks[i].attackTypes[k]);
					animationLength.Add(allAttacks[i].attackClips[k].name, allAttacks[i].attackClips[k].length);
				}	
			}
			catch (System.Exception)
			{
				Debug.LogError(gameObject.name + ": Animation damage, types, and length must be the same length");
			}
		}
	}
	/// <summary>
	/// Gets the damage from the specified animation
	/// </summary>
	/// <param name="animationName"></param>
	/// <returns>Damage from animation, or 0 if it does not exist</returns>
	public float GetDamage(string animationName)
	{
		try
		{
			return animationDamage[animationName];
		}
		catch (System.Exception)
		{
			AnimationNotFound(animationName);
			return 0f;
		}
	}
	/// <param name="animationName"></param>
	/// <returns>Length of an animation</returns>
	public float GetAnimationLength(string animationName)
	{
		try
		{
			return animationLength[animationName];
		} catch (System.Exception)
		{
			AnimationNotFound(animationName);
			return 0f;
		}
	}
	public AttackType GetAnimationType(string animationName)
	{
		try
		{
			return animationType[animationName];
		} catch (System.Exception)
		{
			AnimationNotFound(animationName);
			return AttackType.LIGHT;
		}
	}
	public static float GetAnimationLength(WeaponCharacteristics[] weapons, string animationName)
	{
		for (int i = 0; i < weapons.Length; i++)
		{
			if (weapons[i].animationLength.ContainsKey(animationName))
				return weapons[i].animationLength[animationName];
		}
		Debug.LogWarning("ERROR: " + animationName + " not found");
		return 0f;
	}
	public void SetAttackStatus(WeaponStatus status)
	{
		this.status = status;
	}
	public WeaponStatus GetAttackStatus()
	{
		return status;
	}
	public bool CheckAnimation(string animationName)
	{
		return animationDamage.ContainsKey(animationName);
	}
	private void AnimationNotFound(string animationName)
	{
		Debug.LogWarning("ERROR: " + animationName + " not found");
	}
}
