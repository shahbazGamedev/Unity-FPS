using UnityEngine;
using System.Collections;

public class RestartScene : MonoBehaviour 
{
	private GUIText restart;
	
	void Start()
	{
		restart = GetComponent <GUIText>();
	}
	
	void OnMouseOver()
	{
		restart.fontSize = 77;
	}
	
	void OnMouseExit()
	{
		restart.fontSize = 72;
	}
	
	void OnMouseDown()
	{
		globals.paused = false;
		globals.health = 5;
		globals.damage = 1;
		globals.fireRate = 0.8f;
		globals.movementSpeed = 9f;
		globals.shots = 1;
		Application.LoadLevel (Application.loadedLevel);
	}
}