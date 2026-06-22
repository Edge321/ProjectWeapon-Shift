using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
	[Range(0.05f, 2f)]
	public float parryWindow = 0.1f;
	public float GetParryWindow()
	{
		return parryWindow;
	}
}
