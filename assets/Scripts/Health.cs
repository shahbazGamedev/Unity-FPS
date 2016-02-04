using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour 
{
	public float health = 5.0f;

	void Update () 
	{
		if (health <= 0.0f) Destroy (this.gameObject);
	}

	public float getHealth()
	{
		return health;
	}

	public void damageHealth(float newHealth)
	{
		health -= newHealth;
	}
}
