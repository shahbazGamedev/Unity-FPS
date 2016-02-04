using UnityEngine;
using System.Collections;

public class SetPosition : MonoBehaviour {

	public Transform position;

	public int constX = 0;
	public int constY = 0;
	public int constZ = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = position.position;
		if (constX != 0)
		{
			transform.position = new Vector3(constX, transform.position.y, transform.position.z);
		}
		if (constY != 0)
		{
			transform.position = new Vector3(transform.position.x, constY, transform.position.z);
		}
		if (constZ != 0)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, constZ);
		}
	}
}
