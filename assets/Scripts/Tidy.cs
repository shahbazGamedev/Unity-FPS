using UnityEngine;
using System.Collections;

public class Tidy : MonoBehaviour 
{	
	private bool damageDealt;

	void Start () 
	{
		damageDealt = false;
		Destroy (gameObject, 3f);
	}	

	void OnCollisionEnter(Collision c)
	{
		if (c.gameObject.tag == "Enemy")
		{
			if (!damageDealt) 
			{
				c.gameObject.GetComponent<MobHealth>().damageHealth (globals.damage);
				damageDealt = true;
			}
		}

		if (c.gameObject.tag != "Shot") GetComponent<AudioSource>().Play ();
	}
}
