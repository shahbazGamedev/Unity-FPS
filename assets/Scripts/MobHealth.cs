using UnityEngine;
using System.Collections;

public class MobHealth : MonoBehaviour 
{
	public float health = 5.0f;

	void Update () 
	{
		if (this.gameObject.tag != "Dead" && health <= 0.0f) 
		{
			this.gameObject.tag = "Dead";
			this.gameObject.SetActive(false);
		}
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
