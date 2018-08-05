using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableItem : MonoBehaviour {
	protected Rigidbody RigidBody;
	protected PhotonView PhotonView;
	protected bool originalKinematicState;
	protected Transform originalParent;

	public bool Holding = false;
	public ControllerInput Controller;


	public float KProportional = 100f;
	public float KIntegral = 1;
	public float KDerivative = 10;
	
	private Vector3 PosOffset; 
	private Vector3 RotOffset; 

	private Vector3 PosPrevError;
	private Vector3 PosIntegral;
	private Vector3 RotPrevError;
	private Vector3 RotIntegral;

	private bool OwnershipRequested = false;
	
	void Awake()
	{
		RigidBody = GetComponent<Rigidbody>();
		PhotonView = GetComponent<PhotonView>();
	
		//Capture object's original parent and kinematic state
		originalParent = transform.parent;
		originalKinematicState = RigidBody.isKinematic;
	}

	//Do this when the mouse is clicked over the selectable object this script is attached to.
	// public void OnPointerDown(PointerEventData eventData)
	// {
	//     Debug.Log(this.gameObject.name + " Was Clicked.");
	// }
	
	public void Pickup(ControllerInput controller)
	{
		//Make object kinematic
		//(Not effected by physics, but still able to effect other objects with physics)
		// RigidBody.isKinematic = true;
	
		//Parent object to hand
		// transform.SetParent(controller.gameObject.transform);
		Holding = true;
		Controller = controller;
		PosIntegral = Vector3.zero;
		PosPrevError = Vector3.zero;
		RotIntegral = Vector3.zero;
		RotPrevError = Vector3.zero;

		OwnershipRequested = false;
	}

	Vector3 GetTorque(Transform a, Transform b)
	{
		// The conversion to euler demands we check each axis
		Vector3 torqueF = OrientTorque(Quaternion.FromToRotation(a.forward, b.forward).eulerAngles);
		Vector3 torqueR = OrientTorque(Quaternion.FromToRotation(a.right, b.right).eulerAngles);
		Vector3 torqueU = OrientTorque(Quaternion.FromToRotation(a.up, b.up).eulerAngles);

		float magF = torqueF.magnitude;
		float magR = torqueR.magnitude;
		float magU = torqueU.magnitude;
 
		// Here we pick the axis with the least amount of rotation to use as our torque.
		return magF < magR ? (magF < magU ? torqueF : torqueU) : (magR < magU ? torqueR : torqueU);
	}

	private Vector3 OrientTorque(Vector3 torque)
	 {
		// Quaternion's Euler conversion results in (0-360)
		// For torque, we need -180 to 180.

		return new Vector3
		(
			torque.x > 180f ? 180f - torque.x : torque.x,
			torque.y > 180f ? 180f - torque.y : torque.y,
			torque.z > 180f ? 180f - torque.z : torque.z
		);
	 }

	void FixedUpdate()
	{
		if (Holding && Controller)
		{
			if (!OwnershipRequested)
			{
				PhotonView.RequestOwnership();
				// PhotonView.TransferOwnership(PhotonNetwork.player);
				OwnershipRequested = true;
			}

			RigidBody.isKinematic = false;

			// transform.position = Controller.gameObject.transform.position;
			// transform.rotation = Controller.gameObject.transform.rotation;
			var posError = Controller.gameObject.transform.position - transform.position;
			PosIntegral = PosIntegral + posError * Time.deltaTime;
  			var posDerivative = (posError - PosPrevError) / Time.deltaTime;
			var posForce = KProportional * posError + KIntegral * PosIntegral + KDerivative * posDerivative;
			PosPrevError = posError;
			RigidBody.AddForce(posForce);

			// var rotError = GetTorque(Controller.gameObject.transform, transform);

			// var requiredRotation = Quaternion.FromToRotation(transform.forward, Controller.gameObject.transform.forward);
 			// var rotError = requiredRotation.eulerAngles;
			// var rotError = transform.rotation - Controller.gameObject.transform.rotation;

			var rotA = transform.rotation;
			var rotB = Controller.gameObject.transform.rotation;
			var rotError = Quaternion.Angle(rotA, rotB);
			
			
			var rotateTowards = Quaternion.RotateTowards(rotA, rotB, rotError * 0.2f);

			// Quaternion rotateTowards = Quaternion.RotateTowards(transform.rotation, Controller.gameObject.transform.rotation, 0.1f);
			// Quaternion rotateTowards = Quaternion.Inverse(transform.rotation) * Controller.gameObject.transform.rotation;
			// var rotError = rotateTowards.eulerAngles;
			// var rotError = Controller.gameObject.transform.rotation.eulerAngles - transform.rotation.eulerAngles;

			// Debug.Log(transform.rotation.eulerAngles);
			// Debug.Log(Controller.gameObject.transform.rotation.eulerAngles);
			// Debug.Log(rotError);

			//  transform.rotation * targetOri;
			// Matrix3 rotMat = rotError.ToRotationMatrix();
			// Radian r1, r2, r3;
			// rotMat.ToEulerAnglesXYZ(out r1, out r2, out r3);
			// currTorque =
			// new Vector3(r1.ValueRadians, r2.ValueRadians, r3.ValueRadians);


			// rigidBody.AddTorque(rotError * 0.1f);
			RigidBody.MoveRotation(rotateTowards);
		}
		else
		{
			// RigidBody.isKinematic = PhotonView.isOwnerActive;
			RigidBody.isKinematic = PhotonView.owner != PhotonNetwork.player;
			// if (PhotonView.owner == PhotonNetwork.player)
			// {
			// 	PhotonView.TransferOwnership
			// }
		}
	}

	public void Release(ControllerInput controller)
	{
		Holding = false;
		//Make sure the hand is still the parent. 
		//Could have been transferred to anothr hand.
		if (transform.parent == controller.gameObject.transform)
		{
			//Return previous kinematic state
			RigidBody.isKinematic = originalKinematicState;
	
			//Set object's parent to its original parent
			if (originalParent != controller.gameObject.transform)
			{
				//Ensure original parent recorded wasn't somehow the controller (failsafe)
				// transform.SetParent(originalParent);
			}
			else
			{
				// transform.SetParent(null);
			}

			// Holding = false;
	
			//Throw object
			// rigidBody.velocity = controller.device.velocity;
			// rigidBody.angularVelocity = controller.device.angularVelocity;
		}
	
	}


}
