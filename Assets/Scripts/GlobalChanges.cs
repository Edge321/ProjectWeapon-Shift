using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalChanges : MonoBehaviour
{
	[Range(9.8f, 100f)]
    public float gravity = 69f;
    void Start()
    {
        Physics.gravity = new Vector3(0, -gravity, 0);
    }
}
