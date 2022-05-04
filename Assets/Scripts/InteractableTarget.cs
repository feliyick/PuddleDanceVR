 //======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Demonstrates how to create a simple interactable object
//
//=============================================================================

using UnityEngine;
using System.Collections;

namespace Valve.VR.InteractionSystem.Sample
{
	//-------------------------------------------------------------------------
	[RequireComponent( typeof( Interactable ) )]
	public class InteractableTarget : MonoBehaviour
    {
		public string type;

        private TextMesh generalText;
        private TextMesh hoveringText;
        private Vector3 oldPosition;
		private Quaternion oldRotation;

		private float attachTime;

		private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & ( ~Hand.AttachmentFlags.SnapOnAttach ) & (~Hand.AttachmentFlags.DetachOthers) & (~Hand.AttachmentFlags.VelocityMovement);

        private Interactable interactable;
		private Mesh target;

		private float lifespan = 0;
		private float maxLifespan;

		// Textures to update materials in realtime if player hits right or wrong target
		public Texture2D WrongAlbedo;
		public Texture2D WrongEmissive;
		public Texture2D RightAlbedo;
		public Texture2D RightEmissive;

		private bool isCorrect;
		AudioSource audioSource;

		//-------------------------------------------------
		void Awake()
		{
			// var textMeshs = GetComponentsInChildren<TextMesh>();
            // generalText = textMeshs[0];
            // hoveringText = textMeshs[1];

            // generalText.text = type;
            // hoveringText.text = "Hovering: False";

            interactable = this.GetComponent<Interactable>();
			target = this.GetComponent<Mesh>();


			// how long a target is "active for" in seconds
			maxLifespan = 3;

			isCorrect = false;
			audioSource  = GetComponent<AudioSource>();
		}


		//-------------------------------------------------
		// Called when a Hand starts hovering over this object
		//-------------------------------------------------
		private void OnHandHoverBegin( Hand hand )
		{
			// generalText.text = "Hovering hand: " + hand.name + " TYPE: " + type;
			//add score
			Debug.Log("Hovering hand: " + hand.name + " TYPE: " + type + 
			" IS RIGHT HAND: " + ((hand.name == "RightHand") && (type == "Right Hand")));
			if ((type == "Right Hand") && (hand.name == "RightHand")) {
				correctTargetUpdate();
				Debug.Log("CORRECT");
			} else if ((type == "Left Hand") && (hand.name == "LeftHand")) {
				correctTargetUpdate();
				Debug.Log("CORRECT");	
			} else if ((type == "Right Foot") && (hand.name == "RightFoot")) {
				correctTargetUpdate();
				Debug.Log("CORRECT");
			} else if ((type == "Left Foot") && (hand.name == "LeftFoot")) {
				correctTargetUpdate();
				Debug.Log("CORRECT");
			} else {
				wrongTargetUpdate();
				Debug.Log("WRONG");
			}

			// Play Sound
			// audioSource.Play();
		}

		private void wrongTargetUpdate() {
			GameObject target = this.gameObject.transform.GetChild(3).GetChild(0).gameObject;
			MeshRenderer meshRenderer = target.GetComponent<MeshRenderer>();
			meshRenderer.material.mainTexture = WrongAlbedo;
			meshRenderer.material.EnableKeyword("_EMISSION");
			meshRenderer.material.SetTexture("_EmissionMap", WrongEmissive);

			isCorrect = false;
			
		}

		private void correctTargetUpdate() {

			GameObject target = this.gameObject.transform.GetChild(3).GetChild(0).gameObject;
			MeshRenderer meshRenderer = target.GetComponent<MeshRenderer>();
			meshRenderer.material.mainTexture = RightAlbedo;
			meshRenderer.material.EnableKeyword("_EMISSION");
			meshRenderer.material.SetTexture("_EmissionMap", RightEmissive);

			isCorrect = true;
		}


		void OnDestroy() {
			
			if (isCorrect) {
				SumScore.Add(1);
			} else {
				SumScore.Add(-1);
			}
		}


		//-------------------------------------------------
		// Called when a Hand stops hovering over this object
		//-------------------------------------------------
		private void OnHandHoverEnd( Hand hand )
		{
			// generalText.text = type;
			KillTarget();
			
		}


		//-------------------------------------------------
		// Called every Update() while a Hand is hovering over this object
		//-------------------------------------------------
		private void HandHoverUpdate( Hand hand )
		{
			// Debug.Log("HAND HOVER UPDATE");
			// if ((type == "Left Hand" && hand.name == "LeftHand") || 
			// 	(type == "Right Hand" && hand.name == "RightHand") ||
			// 	(type == "Left Foot" && hand.name == "LeftFoot") || 
			// 	(type == "Right Foot" && hand.name == "RightFoot")) {
			// 		generalText.text = "CORRECT " + target.name;
			// 		Debug.Log("KILL TARGET CORRECT");
			// 		SumScore.Add(1);
			// 	// GrabTypes startingGrabType = hand.GetGrabStarting();
			// 	// bool isGrabEnding = hand.IsGrabEnding(this.gameObject);

			// 	// if (interactable.attachedToHand == null && startingGrabType != GrabTypes.None)
			// 	// {
			// 	// 	// Save our position/rotation so that we can restore it when we detach
			// 	// 	oldPosition = transform.position;
			// 	// 	oldRotation = transform.rotation;

			// 	// 	// Call this to continue receiving HandHoverUpdate messages,
			// 	// 	// and prevent the hand from hovering over anything else
			// 	// 	hand.HoverLock(interactable);

			// 	// 	// Attach this object to the hand
			// 	// 	hand.AttachObject(gameObject, startingGrabType, attachmentFlags);
			// 	// }
			// 	// else if (isGrabEnding)
			// 	// {
			// 	// 	// Detach this object from the hand
			// 	// 	hand.DetachObject(gameObject);

			// 	// 	// Call this to undo HoverLock
			// 	// 	hand.HoverUnlock(interactable);

			// 	// 	// Restore position/rotation
			// 	// 	transform.position = oldPosition;
			// 	// 	transform.rotation = oldRotation;
			// 	// }
			// } else {
			// 	generalText.text = "Wrong Hand " + hand.name ;
			// 	//penalty score
			// 	// SumScore.Add(-1);
			// }
		}


		//-------------------------------------------------
		// Called when this GameObject becomes attached to the hand
		//-------------------------------------------------
		private void OnAttachedToHand( Hand hand )
        {
            // generalText.text = string.Format("Attached: {0}", hand.name);
            attachTime = Time.time;
		}



		//-------------------------------------------------
		// Called when this GameObject is detached from the hand
		//-------------------------------------------------
		private void OnDetachedFromHand( Hand hand )
		{
            // generalText.text = string.Format("Detached: {0}", hand.name);
		}


		//-------------------------------------------------
		// Called every Update() while this GameObject is attached to the hand
		//-------------------------------------------------
		private void HandAttachedUpdate( Hand hand )
		{
            // generalText.text = string.Format("Attached: {0} :: Time: {1:F2}", hand.name, (Time.time - attachTime));
		}

        private bool lastHovering = false;
        private void Update()
        {
            if (interactable.isHovering != lastHovering) //save on the .tostrings a bit
            {
                // hoveringText.text = string.Format("Hovering: {0}", interactable.isHovering);
                lastHovering = interactable.isHovering;
            }
			if (lifespan < maxLifespan) {
				lifespan += Time.deltaTime;
			} else {
				Debug.Log("KILL TARGET OVER LIFE SPAN");
				wrongTargetUpdate();
				KillTarget(1);
			}
        }


		//-------------------------------------------------
		// Called when this attached GameObject becomes the primary attached object
		//-------------------------------------------------
		private void OnHandFocusAcquired( Hand hand )
		{
		}


		//-------------------------------------------------
		// Called when another attached GameObject becomes the primary attached object
		//-------------------------------------------------
		private void OnHandFocusLost( Hand hand )
		{
		}

		//-------------------------------------------------
		// Called when the correct hand hits a target, player misses, or runs out of time
		//-------------------------------------------------
		private void KillTarget(int delay = 0)
		{
			GameObject.Destroy(gameObject, delay);
		}


	}
}
