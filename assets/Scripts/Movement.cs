using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]
public class Movement : MonoBehaviour 
{
	public float upDownRange = 60.0f;

	float verticalRotation = 0;
	
	// If true, diagonal speed (when strafing + moving forward or back) can't exceed normal move speed; otherwise it's about 1.4 times faster
	public bool limitDiagonalSpeed = true;
	public float jumpSpeed = 8.0f;
	public float gravity = 20.0f;
	
	// If the player ends up on a slope which is at least the Slope Limit as set on the character controller, then he will slide down
	public bool slideWhenOverSlopeLimit = false;
	// If checked and the player is on an object tagged "Slide", he will slide down it regardless of the slope limit
	public bool slideOnTaggedObjects = false;
	public float slideSpeed = 12.0f;
	
	// If checked, then the player can change direction while in the air
	public bool airControl = false;
	
	// Small amounts of this results in bumping when walking down slopes, but large amounts results in falling too fast
	public float antiBumpFactor = .75f;
	// Player must be grounded for at least this many physics frames before being able to jump again; set to 0 to allow bunny hopping
	public int antiBunnyHopFactor = 0;

	private bool                grounded      = false;
	private bool 				playerControl = false;
	private bool          		falling;
	private Vector3             moveDirection = Vector3.zero;
	private Vector3 			contactPoint;
	private CharacterController controller;
	private Transform           myTransform;
	private RaycastHit 			hit;
	private float               speed;
	private float               sensitivity;
	private float 				slideLimit;
	private float 				rayDistance;
	private int					jumpTimer;
	private float currentTime;
	private float footstep;
	private AudioSource audio;

	void Start()
	{
		controller  = GetComponent<CharacterController>();
		myTransform = transform;
		speed       = globals.movementSpeed;
		sensitivity = globals.mouseSensitivity;
		rayDistance = controller.height * .5f + controller.radius;
		slideLimit  = controller.slopeLimit - .1f;
		jumpTimer   = antiBunnyHopFactor;
		audio 		= transform.GetChild(0).GetChild (0).GetComponent<AudioSource>();
		Debug.Log (audio);
		footstep = 0.45f;
		currentTime = 0.0f;
	}

	void Update()
	{
		currentTime += Time.deltaTime;
	}
	
	void FixedUpdate()
	{
		// Rotation
		float rotLeftRight = Input.GetAxis("Mouse X") * sensitivity;
		transform.Rotate(0, rotLeftRight, 0);
		
		verticalRotation -= Input.GetAxis("Mouse Y") * sensitivity;
		verticalRotation  = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
		Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

		// Movement
		float inputX = Input.GetAxis("Horizontal");
		float inputY = Input.GetAxis("Vertical");
		// If both horizontal and vertical are used simultaneously, limit speed (if allowed), so the total doesn't exceed normal move speed
		float inputModifyFactor = (inputX != 0.0f && inputY != 0.0f && limitDiagonalSpeed)? .7071f : 1.0f;
		
		if (grounded)
		{
			if (currentTime >= footstep && (inputX != 0.0f || inputY != 0.0f))
			{
				audio.Play();
				currentTime = 0.0f;
			}

			bool sliding = false;
			// See if surface immediately below should be slid down. We use this normally rather than a ControllerColliderHit point,
			// because that interferes with step climbing amongst other annoyances
			if (Physics.Raycast(myTransform.position, -Vector3.up, out hit, rayDistance))
			{
				if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit) sliding = true;
			}
			// However, just raycasting straight down from the center can fail when on steep slopes
			// So if the above raycast didn't catch anything, raycast down from the stored ControllerColliderHit point instead
			else
			{
				Physics.Raycast(contactPoint + Vector3.up, -Vector3.up, out hit);
				if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit) sliding = true;
			}
			
			// If we were falling, and we fell a vertical distance greater than the threshold, run a falling damage routine
			if (falling) falling = false;
			
			// If sliding (and it's allowed), or if we're on an object tagged "Slide", get a vector pointing down the slope we're on
			if ((sliding && slideWhenOverSlopeLimit) || (slideOnTaggedObjects && hit.collider.tag == "Slide"))
			{
				Vector3 hitNormal = hit.normal;
				moveDirection = new Vector3(hitNormal.x, -hitNormal.y, hitNormal.z);
				Vector3.OrthoNormalize (ref hitNormal, ref moveDirection);
				moveDirection *= slideSpeed;
				playerControl = false;
			}
			// Otherwise recalculate moveDirection directly from axes, adding a bit of -y to avoid bumping down inclines
			else
			{
				moveDirection = new Vector3(inputX * inputModifyFactor, -antiBumpFactor, inputY * inputModifyFactor);
				moveDirection = myTransform.TransformDirection(moveDirection) * speed;
				playerControl = true;
			}
			
			// Jump! But only if the jump button has been released and player has been grounded for a given number of frames
			if (!Input.GetButton("Jump")) jumpTimer++;
			
			else if (jumpTimer >= antiBunnyHopFactor)
			{
				moveDirection.y = jumpSpeed;
				jumpTimer = 0;
			}
		}
		else
		{
			// If we stepped over a cliff or something, set the height at which we started falling
			if (!falling) falling = true;
			
			// If air control is allowed, check movement but don't touch the y component
			if (airControl && playerControl)
			{
				moveDirection.x = inputX * speed * inputModifyFactor;
				moveDirection.z = inputY * speed * inputModifyFactor;
				moveDirection = myTransform.TransformDirection(moveDirection);
			}
		}
		
		// Apply gravity
		moveDirection.y -= gravity * Time.deltaTime;
		
		// Move the controller, and set grounded true or false depending on whether we're standing on something
		grounded = (controller.Move(moveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;
	}
	
	// Store point that we're in contact with for use in FixedUpdate if needed
	void OnControllerColliderHit (ControllerColliderHit hit)
	{
		contactPoint = hit.point;
	}
}