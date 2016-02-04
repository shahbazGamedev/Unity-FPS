using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour 
{
	public Rigidbody projectile;
	public Transform shotPos;
	public Transform shotPos2;

	private float range = 1500f;
	private float nextShot = 0.0f;
	
	void Update () 
	{
		if (Input.GetButton ("Fire1") && Time.time > nextShot && !globals.paused)
		{
			nextShot = Time.time + globals.fireRate;

			Rigidbody shot = Instantiate (projectile, shotPos.position, shotPos.rotation * Quaternion.Euler(0, 90, 0)) as Rigidbody;
			shot.AddForce  (shotPos.forward * range);
			shot.AddTorque (20, 0, 20);

			if (globals.shots == 2)
			{
				Rigidbody shot2 = Instantiate (projectile, shotPos2.position, shotPos2.rotation * Quaternion.Euler(0, 90, 0)) as Rigidbody;
				shot2.AddForce  (shotPos.forward * range);
				shot2.AddTorque (20, 0, 20);
			}

			GetComponent<Animation>().Play();
			GetComponent<AudioSource>().Play();
		}
	}
}
