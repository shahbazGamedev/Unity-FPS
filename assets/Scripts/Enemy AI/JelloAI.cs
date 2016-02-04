using UnityEngine;
using System.Collections;

public class JelloAI : MonoBehaviour 
{
	public float viewDistance;
	public float InterpSpeed;
	public float attackSlideForce;
	public float idleSlideForce;
	public float slideDelay;
	public float rotateDelay;
	
	private Transform	target;
	private float		distance;
	private RaycastHit	vision;
	private Vector3		rayDirection;
	private bool		canSeePlayer;
	private bool		rotating;
	private bool		needDirection;
	private float		currentTime;
	private float		rotateTime;
	private Quaternion	idleRotate;
	
	void Awake()
	{
		canSeePlayer	= false;
		rotating		= false;
		needDirection	= false;
		target			= GameObject.Find("Player").transform;
		currentTime		= 0.0f;
		rotateTime		= 0.0f;
		idleRotate		= Quaternion.Euler (0, Random.Range (-180, 180), 0);
	}
	
	void Update()
	{
		if (canSeePlayer)
		{
			lookAtTarget();
			rotating = false;
		}

		if (!rotating)	currentTime += Time.deltaTime;
		else			rotateTime	+= Time.deltaTime;
	}
	
	void FixedUpdate()
	{
		// Check for Player
		distance = Vector3.Distance(target.position, transform.position);
		
		if (distance <= viewDistance)
		{
			rayDirection = target.position - transform.position;
			if (Physics.Raycast(transform.position, rayDirection, out vision))
			{
				if (vision.transform == target) canSeePlayer = true;
				else canSeePlayer = false;
			}
		}
		else canSeePlayer = false;
		
		// Actions
		if (canSeePlayer)
		{
			lookAtTarget();
			tryToKillTarget();
		}
		else idle();
	}
	
	void lookAtTarget()
	{
		Quaternion rotate	= Quaternion.LookRotation(target.position - transform.position);
		Quaternion step		= Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * 3 * InterpSpeed);
		transform.rotation	= Quaternion.Euler(0, step.eulerAngles.y, 0);
	}
	
	void tryToKillTarget()
	{
		if (currentTime >= slideDelay)
		{
			GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			transform.GetComponent<Rigidbody>().AddForce(transform.forward * attackSlideForce * 1000);
			currentTime = 0.0f;
		}
	}
	
	void idle()
	{
		if (rotateTime >= rotateDelay)
		{
			rotateTime	= 0.0f;
			rotating	= false;
		}
		
		if (needDirection)
		{
			if (GetComponent<Rigidbody>().velocity.magnitude <= 0.5)
			{
				idleRotate = Quaternion.Euler (0, Random.Range (-180, 180), 0);
				needDirection = false;
				rotating = true;
			}
			else return;
		}
		
		else if (rotating)
		{
			Quaternion step		= Quaternion.Slerp(transform.rotation, idleRotate, Time.deltaTime * InterpSpeed);
			transform.rotation	= Quaternion.Euler(0, step.eulerAngles.y, 0);
		}
		
		else if (currentTime >= slideDelay && !needDirection && !rotating)
		{
			GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			transform.GetComponent<Rigidbody>().AddForce(transform.forward * idleSlideForce * 1000);
			currentTime = 0.0f;
			needDirection = true;
		}
	}
	
	public void setTarget(Transform t)
	{
		this.target = t;
	}
}