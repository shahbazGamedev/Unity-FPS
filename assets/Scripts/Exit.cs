using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour 
{
	private GUIText exit;
	
	void Start()
	{
		exit = GetComponent <GUIText>();
	}
	
	void OnMouseOver()
	{
		exit.fontSize = 77;
	}
	
	void OnMouseExit()
	{
		exit.fontSize = 72;
	}
	
	void OnMouseDown()
	{
		Application.Quit();
	}
}
