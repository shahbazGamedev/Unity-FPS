using UnityEngine;
using System.Collections;

public class Stat : MonoBehaviour 
{
	private GUIText text;

	void Start()
	{
		text = GetComponent <GUIText>();
	}

	void Update () 
	{
		if (text.name == "Damage") text.text = "DMG: " + globals.damage;
		if (text.name == "MoveSpeed") text.text = "MOVE SPD: " + (globals.movementSpeed - 8);
		if (text.name == "ASPD") text.text = "ATK SPD: " + globals.fireRate;
		if (text.name == "Shots") text.text = "SHOTS: " + globals.shots;
	}
}
