/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldWeaponBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StoreDamages();
        StoreAnimationLength();
        StoreAttackTypes();
    }
	private void StoreAttackTypes()
	{
		for (int i = 0; i < allAttacks.Length; i++)
		{
			if (allAttacks[i].attackClips.Length != allAttacks[i].attackTypes.Length)
			{
				Debug.LogWarning(gameObject.name + ": AllAttacks[" + i + "]: AttackClips length " +
								"does not match AttackTypes length. " +
								"Setting all attacks to LIGHT");
				allAttacks[i].attackTypes = new AttackType[allAttacks[i].attackClips.Length];

				for (int k = 0; k < allAttacks[i].attackTypes.Length; k++)
				{
					allAttacks[i].attackTypes[k] = AttackType.LIGHT;
				}

			}

			for (int k = 0; k < allAttacks[i].attackTypes.Length; k++)
				animationType.Add(allAttacks[i].attackClips[k].name, allAttacks[i].attackTypes[k]);
		}
	}
	/// <summary>
	/// Creates a dictionary with the animations' length in seconds
	/// </summary>
	private void StoreAnimationLength()
	{
		for (int i = 0; i < allAttacks.Length; i++)
		{
			for (int k = 0; k < allAttacks[i].attackClips.Length; k++)
				animationLength.Add(allAttacks[i].attackClips[k].name, allAttacks[i].attackClips[k].length);
		}
	}
	/// <summary>
	/// Gets all damages the programmer inputted in Unity and stores them in a Dictionary
	/// </summary>
	private void StoreDamages()
	{
		for (int i = 0; i < allAttacks.Length; i++)
		{
			int k = 0;
			try
			{
				for (k = 0; k < allAttacks[i].attackClips.Length; k++)
					animationDamage.Add(allAttacks[i].attackClips[k].name, allAttacks[i].attackDamage[k]);
			}
			catch (System.Exception)
			{
				Debug.LogWarning("AllAttacks[" + i + "]: AttackClips length " +
											"does not match AttackDamage length. " +
											allAttacks[i].attackClips[k].name + " is disabled");
			}
		}
	}
}
*/