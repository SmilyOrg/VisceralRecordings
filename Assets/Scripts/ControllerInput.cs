using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using SteamVR;

public class ControllerInput : MonoBehaviour {

		//Should only ever be one, but just in case
	protected List<InteractableItem> heldObjects;

	public string TriggerAxis = "";
	
	//Controller References
	// protected SteamVR_TrackedObject trackedObj;
	// public SteamVR_Controller.Device device
	// {
	// 	get
	// 	{
	// 		// return SteamVR_Controller.Input((int)trackedObj.index);
	// 	}
	// }
	
	void Awake()
	{
		//Instantiate lists
		// trackedObj = GetComponent<SteamVR_TrackedObject>();
		heldObjects = new List<InteractableItem>();
	}
	
	void OnTriggerStay(Collider collider)
	{
		//If object is an interactable item
		InteractableItem interactable = collider.GetComponent<InteractableItem>();
		if (interactable != null)
		{
			//If trigger button is down
			if (Input.GetAxis(TriggerAxis) == 1f)
			{
				//Pick up object
				interactable.Pickup(this);
				heldObjects.Add(interactable);
			}
		}
	}

	
	
	// Update is called once per frame
	void Update()
	{
		// var joystickNames = Input.GetJoystickNames();
		// foreach (var name in joystickNames)
		// {
		// 	Debug.Log(name);
		// }

		// Debug.Log(Input.GetAxis("Left Trigger") + " " + Input.GetAxis("Right Trigger"));
		// Debug.Log(Input.GetButton("") + " " + Input.GetAxis("Right Trigger"));

		if (heldObjects.Count > 0)
		{
			//If trigger is releasee
			if (Input.GetAxis(TriggerAxis) < 0.5f)
			{
				//Release any held objects
				for (int i = 0; i < heldObjects.Count; i++)
				{
					heldObjects[i].Release(this);
				}
				heldObjects.Clear();
			}
		}
	}
}
