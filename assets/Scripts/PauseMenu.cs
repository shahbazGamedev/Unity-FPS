using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour 
{
	public GameObject menu;

	void Awake()
	{
		Cursor.visible = false;
		Screen.lockCursor = true;
		menu.SetActive(false);
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape) && globals.health > 0) globals.paused = !globals.paused;

		if (globals.paused && globals.health <= 0) 
		{
			Time.timeScale = 0;
			
			GameObject.Find("Player").GetComponent<Movement>().enabled = false;
			
			Cursor.visible = true;
			Screen.lockCursor = false;
			
			menu.SetActive(true);
			GameObject.Find("Resume").GetComponent<GUIText>().enabled = false;
			GameObject.Find("crosshair").GetComponent<GUITexture>().enabled = false;
		}

		else if (globals.paused) 
		{
			Time.timeScale = 0;

			GameObject.Find("Player").GetComponent<Movement>().enabled = false;

			Cursor.visible = true;
			Screen.lockCursor = false;

			menu.SetActive(true);

			GameObject.Find("crosshair").GetComponent<GUITexture>().enabled = false;
		}

		else 		
		{
			Time.timeScale = 1;

			GameObject.Find("Player").GetComponent<Movement>().enabled = true;;

			Cursor.visible = false;
			Screen.lockCursor = true;

			menu.SetActive(false);

			GameObject.Find("crosshair").GetComponent<GUITexture>().enabled = true;
		}
	}
}
