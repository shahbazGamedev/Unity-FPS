using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour 
{
	public GameObject heart;

	private List<Object>  Health;
	private int           prevHealth;
	private bool		  damageDealt;

	void Start () 
	{
		Health = new List<Object>();
		prevHealth = globals.health;
		damageDealt = false;
		for (int i = 0; i < prevHealth; ++i)
		{
			Health.Add(Instantiate(heart, heart.transform.position + new Vector3(0.025f * i, 0 ,0), new Quaternion()));
		}
	}

	void Update () 
	{
		if (prevHealth != globals.health)
		{
			foreach(Object o in Health) Destroy(o);
			prevHealth = globals.health;
			
			for (int i = 0; i < prevHealth; ++i)
			{
				Health.Add(Instantiate(heart, heart.transform.position + new Vector3(0.025f * i, 0 ,0), new Quaternion()));
			}
		}
	}
	
	void OnCollisionEnter(Collision c)
	{
		if (c.gameObject.tag == "Enemy" && !damageDealt)
		{
			globals.health--;
			if (globals.health <= 0) globals.paused = true;
			damageDealt = true;

			GetComponent<AudioSource>().Play();
			
			StartCoroutine(Invulnerable ());
		}
	}
	
	IEnumerator Invulnerable()
	{
		yield return new WaitForSeconds(1.0f);
		damageDealt = false;
	}
}
