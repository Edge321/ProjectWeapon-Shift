using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageTextBehavior : MonoBehaviour
{
	public float textSpeed = 3.0f;
	
	public byte alphaDecrease = 1;

	private byte alphaColor = 255;

	private RectTransform rect;

	private TextMeshPro textMesh;

	private GameObject camObject;
	void Awake()
	{
		rect = GetComponent<RectTransform>();
		textMesh = GetComponent<TextMeshPro>();
		Camera cam = Camera.main;
		camObject = cam.gameObject;
	}
	void Update()
	{
		rect.position += textSpeed * Time.deltaTime * Vector3.up;
		//Makes the text always look at the camera
		transform.eulerAngles = camObject.transform.eulerAngles;

		if (textMesh.color.a <= 0)
			Destroy(gameObject);
	}
	private void FixedUpdate()
	{
		DecreaseAlpha();
	}
	/// <summary>
	/// Decreases the alpha component of the text's color
	/// </summary>
	private void DecreaseAlpha()
	{
		alphaColor = (byte) Mathf.Clamp(alphaColor - alphaDecrease, 0, alphaColor);
		Color32 color = new Color32(255, 255, 255, alphaColor);
		textMesh.color = color;
	}
	/// <summary>
	/// Sets the text with a float
	/// </summary>
	/// <param name="number"></param>
	public void SetNumber(float number)
	{
		try
		{
			textMesh.text = number.ToString("F1");
		} catch(System.Exception)
		{
			Debug.LogWarning(number + " is not a valid float-to-string");
			textMesh.text = " ";
		}
	}
}
