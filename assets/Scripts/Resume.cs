using UnityEngine;
using System.Collections;

public class Resume : MonoBehaviour 
{
	private GUIText resume;

	void Start()
	{
		resume = GetComponent <GUIText>();
	}

	void OnMouseOver()
	{
		resume.fontSize = 77;
	}

	void OnMouseExit()
	{
		resume.fontSize = 72;
	}

	void OnMouseDown()
	{
		globals.paused = false;
	}
}