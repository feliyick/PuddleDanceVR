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
		public GameObject explodeCorrect;
		public GameObject explodeStreak2;
		public GameObject explodeStreak4;
		public GameObject explodeStreak8;
		AudioSource audioSource;

		private ScoreScript sumScore;


		//-------------------------------------------------
		void Awake()
		{

            interactable = this.GetComponent<Interactable>();
			target = this.GetComponent<Mesh>();


			// how long a target is "active for" in seconds
			maxLifespan = 3;

			isCorrect = false;
			audioSource  = GetComponent<AudioSource>();
			sumScore = GameObject.Find("sumScore").GetComponent<ScoreScript>();
		}


		//-------------------------------------------------
		// Called when a Hand starts hovering over this object
		//-------------------------------------------------
		private void OnHandHoverBegin( Hand hand )
		{
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
			
			float particleScale = 0.07f;
			if (isCorrect) {
				explodeCorrect.transform.localScale = new Vector3(particleScale, particleScale, particleScale);
				explodeStreak2.transform.localScale = new Vector3(particleScale, particleScale, particleScale);
				explodeStreak4.transform.localScale = new Vector3(particleScale, particleScale, particleScale);
				explodeStreak8.transform.localScale = new Vector3(particleScale, particleScale, particleScale);

				if (sumScore.scoreMultiplier == 1) {
					Instantiate(explodeCorrect, gameObject.transform.position, Quaternion.AngleAxis(-90, Vector3.right));
				} else if (sumScore.scoreMultiplier == 2) {
					Instantiate(explodeStreak2, gameObject.transform.position, Quaternion.AngleAxis(-90, Vector3.right));
				} else if (sumScore.scoreMultiplier == 4) {
					Instantiate(explodeStreak4, gameObject.transform.position, Quaternion.AngleAxis(-90, Vector3.right));
				} else if (sumScore.scoreMultiplier == 8) {
					Instantiate(explodeStreak8, gameObject.transform.position, Quaternion.AngleAxis(-90, Vector3.right));
				}
				
				sumScore.AddPoints(1);
			} else {
				sumScore.AddPoints(-1);
			}
		}


		//-------------------------------------------------
		// Called when a Hand stops hovering over this object
		//-------------------------------------------------
		private void OnHandHoverEnd( Hand hand )
		{
			KillTarget();
			
		}


		//-------------------------------------------------
		// Called every Update() while a Hand is hovering over this object
		//-------------------------------------------------
		private void HandHoverUpdate( Hand hand )
		{

		}


		//-------------------------------------------------
		// Called when this GameObject becomes attached to the hand
		//-------------------------------------------------
		private void OnAttachedToHand( Hand hand )
        {
            attachTime = Time.time;
		}



		//-------------------------------------------------
		// Called when this GameObject is detached from the hand
		//-------------------------------------------------
		private void OnDetachedFromHand( Hand hand )
		{
		}


		//-------------------------------------------------
		// Called every Update() while this GameObject is attached to the hand
		//-------------------------------------------------
		private void HandAttachedUpdate( Hand hand )
		{
		}

        private bool lastHovering = false;
        private void Update()
        {
            if (interactable.isHovering != lastHovering) //save on the .tostrings a bit
            {
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
