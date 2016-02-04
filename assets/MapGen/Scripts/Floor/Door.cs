using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour 
{
	public Transform to;
	public float smooth = 3.0f;
	bool move    = false;
	bool stop    = false;
	bool inRange = false;
	void Update()
	{
		if (inRange && Input.GetKeyDown(KeyCode.E) && RenderMap.roomComplete)
		{
			move = true;
		}
		if (!stop && move)
		{
			transform.position = Vector3.Lerp(transform.position, to.position, smooth * Time.deltaTime);
			if (Vector3.Distance(transform.position, to.position) < 1f )
			{
				stop = true;
			}
		}
		
	}
	void OnTriggerEnter(Collider c)
	{
		if (c.tag == "Player")
			inRange = true;
	}
}
