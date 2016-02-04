using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour 
{

	void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.tag == "Player")
		{
			if (this.gameObject.name == "health")      globals.health++;
			if (this.gameObject.name == "fireRate")	   globals.fireRate -= 0.2f;
			if (this.gameObject.name == "speed")       globals.movementSpeed++;
			if (this.gameObject.name == "damage")      globals.damage++;
			if (this.gameObject.name == "dooblehsh0t") globals.shots = 2;

			Destroy(this.gameObject);
		}
	}
}
