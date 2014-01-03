using UnityEngine;
using System.Collections;

[RequireComponent (typeof (ThirdPersonControllerNET))]
public class AnimationController : MonoBehaviour
{
	enum CharacterState
	{
		Normal,
		Jumping,
		Falling,
		Landing
	}
	
	
	public Animation target;
		// The animation component being controlled
	new public Rigidbody rigidbody;
		// The rigidbody movement is read from
	public Transform root;//, spine, hub;
		// The animated transforms used for lower body rotation
	public float
		walkSpeed = 0.2f,
		runSpeed = 1.0f,
			// Walk and run speed dictate at which rigidbody velocity, the animation should blend
		rotationSpeed = 6.0f,
			// The speed at which the lower body should rotate
		shuffleSpeed = 7.0f,
			// The speed at which the character shuffles his feet back into place after an on-the-spot rotation
		runningLandingFactor = 0.2f;
			// Reduces the duration of the landing animation when the rigidbody has hoizontal movement
	
	
	private ThirdPersonControllerNET controller;
	private CharacterState state = CharacterState.Normal;
	private bool canLand = true;
	private float currentRotation;
	private Vector3 lastRootForward;
	
	
	private Vector3 HorizontalMovement
	{
		get
		{
			return new Vector3 (rigidbody.velocity.x, 0.0f, rigidbody.velocity.z);
		}
	}
	
	
	void Reset ()
	// Run setup on component attach, so it is visually more clear which references are used
	{
		Setup ();
	}
	
	
	void Setup ()
	// If target or rigidbody are not set, try using fallbacks
	{
		if (target == null)
		{
			target = GetComponent<Animation> ();
		}
		
		if (rigidbody == null)
		{
			rigidbody = GetComponent<Rigidbody> ();
		}
	}
	
	
	void Start ()
	// Verify setup, configure
	{
		Setup ();
			// Retry setup if references were cleared post-add
			
		if (VerifySetup ())
		{
			controller = GetComponent<ThirdPersonControllerNET> ();
			
			currentRotation = 0.0f;
			lastRootForward = root.forward;
		}
	}
	
	
	bool VerifySetup ()
	{
		return VerifySetup (target, "target") &&
			VerifySetup (rigidbody, "rigidbody") &&
			VerifySetup (root, "root");// &&
			//VerifySetup (spine, "spine") &&
			//VerifySetup (hub, "hub");
	}
	
	
	bool VerifySetup (Component component, string name)
	{
		if (component == null)
		{
			Debug.LogError ("No " + name + " assigned. Please correct and restart.");
			enabled = false;
			
			return false;
		}
		
		return true;
	}
	
	void FixedUpdate ()
	// Handle changes in groundedness
	{
		
	}


	void Update ()
	// Animation control
	{
		switch (state)
		{
			case CharacterState.Normal:
				Vector3 movement = HorizontalMovement; 
			
				if (movement.magnitude < walkSpeed)
				{
					target.CrossFade("idle");
				}
				else
				{
					target.CrossFade ("walk");
				}
			break;
		default:
			break;
		}
	}
	
	
	void LateUpdate ()
	// Apply directional rotation of lower body
	{
		
	}
}
