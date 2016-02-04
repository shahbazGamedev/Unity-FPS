using UnityEngine;
using System.Collections;

public class OnionTidy : MonoBehaviour 
{	
	private bool damageDealt;
	
	void Start () 
	{
		damageDealt = false;
		Destroy (gameObject, 1.5f);
	}	
	
	void OnCollisionEnter(Collision c)
	{
		if (c.gameObject.tag == "Player")
		{
			if (!damageDealt) 
			{
				globals.health--;
				if (globals.health <= 0) globals.paused = true;

				GameObject.Find ("Player").GetComponent<AudioSource>().Play ();
			}
		}
		damageDealt = true;
		
		if (c.gameObject.tag != "OnionShot") GetComponent<AudioSource>().Play ();
	}
}
