using UnityEngine;
using System.Collections;

public class DonutAI : MonoBehaviour 
{
	public float viewDistance;
	public float InterpSpeed;
	public float rotateDelay;
	public float moveSpeed;
	public float moveDelay;
	public float moveDistance;
	public float attackSpeed;
	public float attackRange;

	public Transform shotPos;
	public Rigidbody projectile;
	
	private Transform	target;
	private float		distance;
	private RaycastHit	vision;
	private Vector3		rayDirection;
	private Vector3		newPosition;
	private bool		canSeePlayer;
	private bool		rotating;
	private bool		needDirection;
	private bool		moving;
	private float		currentTime;
	private float		rotateTime;
	private float		attackTime;
	private Quaternion	idleRotate;
	
	void Awake()
	{
		canSeePlayer	= false;
		rotating		= false;
		needDirection	= true;
		moving			= false;
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
			attackTime += Time.deltaTime;
			rotating	= false;
			moving		= false;
		}
		else
		{
			if (!rotating && moving) currentTime += Time.deltaTime;
			else rotateTime	+= Time.deltaTime;
		}
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
			sniperNoSniping();
		}
		else idle();
	}
	
	void lookAtTarget()
	{
		Quaternion rotate	= Quaternion.LookRotation(target.position - transform.position);
		transform.rotation	= Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * 3 * InterpSpeed);
	}
	
	void sniperNoSniping()
	{
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		if (attackTime >= attackSpeed)
		{
			Rigidbody shot = Instantiate (projectile, shotPos.position, shotPos.rotation) as Rigidbody;
			shot.AddForce (shotPos.forward * attackRange);
			attackTime		= 0.0f;
			needDirection	= true;
		}
	}
	
	void idle()
	{
		if (rotateTime >= rotateDelay)
		{
			rotateTime	= 0.0f;
			newPosition = transform.position + (transform.forward * moveDistance);
			rotating	= false;
			moving		= true;
		}
		
		if (needDirection)
		{
			idleRotate = Quaternion.Euler (0, Random.Range (-180, 180), 0);
			needDirection = false;
			rotating = true;
			GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
		
		else if (rotating)
		{
			Quaternion step		= Quaternion.Slerp(transform.rotation, idleRotate, Time.deltaTime * InterpSpeed);
			transform.rotation	= Quaternion.Euler(0, step.eulerAngles.y, 0);
		}
		
		else if (moving)
		{
			GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime);
			if (currentTime >= moveDelay)
			{
				currentTime		= 0.0f;
				needDirection	= true;
				moving			= false;
			}
		}
	}
}