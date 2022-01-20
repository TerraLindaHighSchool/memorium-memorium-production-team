using System;
using System.Collections.Generic;
using Cinemachine;
using NPC_Control.Behavior_Tree;
using Other;
using Player_Control;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInputManager = Player_Control.PlayerInputManager;

namespace Camera_and_Lighting {
	/// <summary>
	/// Singleton that manages the overall game camera. 
	/// Manages the orbit camera code around the player. 
	/// In the future may also manage switching between VCAMs.
	/// </summary>
	public class CameraManager : Singleton<CameraManager> {
		///Orbit camera sensitivity setting, set in the editor or by settings menu.
		public float sensitivity = 0.7f;

		///Orbit camera vertical angle boundary, set in the editor or by settings menu.
		public int cameraYBound = 60;

		///The empty on the player that gets rotated to make the orbit camera.
		public Transform playerFollowCamTarget;

		public CameraState camState;

		public NPC currentDialogueNPC;

		private List<GameObject> _currentDialogueNPCChildren;

		///Keeps track of the mouse movement delta from frame to frame. 
		private Vector2 _mouseDelta;

		///Controls whether or not the camera is in "orbit mode", controlled by right click.
		private bool _inOrbitMode;

		private CinemachineVirtualCamera _playerOrbitCam;

		private int _frameCount;

		///Returns the float that is the y of the follow target's rotation eulers. 
		///Used for getting the forward vector of the follow target for player movement. 
		public float GetYRotForForwards() { return playerFollowCamTarget.eulerAngles.y; }

		///Gets the PlayerInputActions from the player and subscribes to the necessary events. 
		private void Start() {
			_currentDialogueNPCChildren = new List<GameObject>();

			_playerOrbitCam       = GameObject.Find("Player VCAM").GetComponent<CinemachineVirtualCamera>();
			playerFollowCamTarget = GameObject.Find("LookAtTarget").transform;

			camState = CameraState.PlayerOrbit;

			PlayerInputActions playerInputActions = PlayerInputManager.Instance.PlayerInputActions;

			playerInputActions.Player.MouseDelta.performed += OnMouseDelta;

			playerInputActions.Player.Orbit.started  += OnOrbitStarted;
			playerInputActions.Player.Orbit.canceled += OnOrbitCancelled;
		}

		private void FixedUpdate() {
			_frameCount++;
			if (_frameCount == 300) OnEnterDialogue(GameObject.Find("Test NPC").GetComponent<NPC>());
			if (_frameCount == 600) OnExitDialogue(currentDialogueNPC);
		}

		private void LateUpdate() {
			Camera cam = Camera.main;
			cam.transform.SetPositionAndRotation(cam.transform.position,
			                                     Quaternion.Euler(
				                                     cam.transform.eulerAngles.x,
				                                     cam.transform.eulerAngles.y, 0));
		}

		private void OnEnterDialogue(NPC npc) {
			Debug.Log($"Camera entering dialogue with NPC {npc}");

			camState           = CameraState.Dialogue;
			currentDialogueNPC = npc;
			Vector3 playerPos = playerFollowCamTarget.position;
			Vector3 npcPos    = npc.gameObject.transform.position;
			GameObject midPointObject =
				new GameObject($"{npc.name} Dialogue MidPoint") {
					                                                transform = {
						                                                position =
							                                                Vector3.Lerp(
								                                                playerPos, npcPos, 0.5f)
					                                                }
				                                                };
			midPointObject.transform.SetParent(npc.transform, true);
			_currentDialogueNPCChildren.Add(midPointObject);
			Vector3 midPoint = midPointObject.transform.position;

			//Direction to the player's right, when looking at the npc
			Vector3 dir = Quaternion.Euler(0, -90, 0) * (playerPos - midPoint).normalized;

			bool isRightSideAvailable = !Physics.Raycast(midPoint, dir, 5f);
			bool isLeftSideAvailable  = !Physics.Raycast(midPoint, -dir, 5f);

			Vector3 dialogueCamPos = playerPos + Vector3.up * 5;

			if (isRightSideAvailable) { dialogueCamPos = midPoint + dir * 3; } else if (isLeftSideAvailable)
				dialogueCamPos = midPoint + -dir * 3;

			Vector3 backDir = Quaternion.Euler(0, -90, 0) * dir;

			dialogueCamPos -= backDir * 5;

			GameObject newCameraObject =
				new GameObject($"{npc.name} Dialogue Camera") {transform = {position = dialogueCamPos}};
			newCameraObject.transform.SetParent(npc.transform, true);
			_currentDialogueNPCChildren.Add(newCameraObject);
			CinemachineVirtualCamera dialogueVCam = newCameraObject.AddComponent<CinemachineVirtualCamera>();
			dialogueVCam.Priority = 15;

			GameObject targetGroupObject = new GameObject($"{npc.name} Dialogue Group Composer");
			targetGroupObject.transform.SetParent(npc.transform, true);
			_currentDialogueNPCChildren.Add(targetGroupObject);
			CinemachineTargetGroup targetGroup = targetGroupObject.AddComponent<CinemachineTargetGroup>();

			targetGroup.AddMember(playerFollowCamTarget, 1.0f, 0.0f);
			targetGroup.AddMember(npc.transform, 1.0f, 0.0f);

			CinemachineGroupComposer currentDialogueVCamComposer =
				dialogueVCam.AddCinemachineComponent<CinemachineGroupComposer>();

			dialogueVCam.LookAt = targetGroupObject.transform;

			currentDialogueVCamComposer.m_MinimumFOV     = 60;
			currentDialogueVCamComposer.m_MaximumFOV     = 60;
			currentDialogueVCamComposer.m_AdjustmentMode = CinemachineGroupComposer.AdjustmentMode.ZoomOnly;
		}

		private void OnExitDialogue(NPC npc) {
			Debug.Log($"Camera exiting dialogue with NPC {npc}");

			camState           = CameraState.PlayerOrbit;
			currentDialogueNPC = null;

			foreach (GameObject obj in _currentDialogueNPCChildren) Destroy(obj);

			_currentDialogueNPCChildren.Clear();
		}

		/// <summary>
		/// Orbits the camera if necessary whenever the mouse delta changes. 
		/// </summary>
		/// <param name="context">The Action CallbackContext, passed in from the <c>MouseDelta.performed</c> event.</param>
		private void OnMouseDelta(InputAction.CallbackContext context) {
			if (camState == CameraState.PlayerOrbit && _inOrbitMode) OrbitCamera(context.ReadValue<Vector2>());
		}

		/// <summary>
		/// Sets <c>_inOrbitMode</c> to the state of the right click whenever it changes. 
		/// </summary>
		/// <param name="context">The Action CallbackContext, passed in from the <c>Orbit.started</c> event.</param>
		private void OnOrbitStarted(InputAction.CallbackContext context) { _inOrbitMode = true; }

		/// <summary>
		/// Sets <c>_inOrbitMode</c> to the state of the right click whenever it changes. 
		/// </summary>
		/// <param name="context">The Action CallbackContext, passed in from the <c>Orbit.canceled</c> event.</param>
		private void OnOrbitCancelled(InputAction.CallbackContext context) { _inOrbitMode = false; }

		/// <summary>
		/// Uses the passed in mouse delta to orbit the camera around the player, within the set bounds.
		/// </summary>
		/// <param name="mouseDelta">A Vector2 for the x,y of the mouse movement delta this frame.</param>
		private void OrbitCamera(Vector2 mouseDelta) {
			mouseDelta   *= sensitivity * (1 + Time.deltaTime);
			mouseDelta.y *= -1;

			Transform playerTransform = playerFollowCamTarget.parent;

			//Rotate around up axis by mouse x delta
			playerFollowCamTarget.Rotate(playerTransform.up, mouseDelta.x);

			//Clamp rotation around sideways axis
			float attemptedNewCameraX = playerFollowCamTarget.eulerAngles.x + mouseDelta.y;
			if (attemptedNewCameraX <= cameraYBound) {
				playerFollowCamTarget.Rotate(Vector3.right, mouseDelta.y);
			} else if (attemptedNewCameraX >= 360 - cameraYBound) {
				playerFollowCamTarget.Rotate(Vector3.right, mouseDelta.y);
			}

			Vector3 playerFollowTargetEulers = playerFollowCamTarget.eulerAngles;

			Quaternion removeFollowTargetZComponent =
				Quaternion.Euler(new Vector3(playerFollowTargetEulers.x, playerFollowTargetEulers.y,
				                             0));
			playerFollowCamTarget.SetPositionAndRotation(playerFollowCamTarget.position, removeFollowTargetZComponent);
		}

		public enum CameraState {
			PlayerOrbit,
			Dialogue,
			Fixed,
			OnRails
		}
	}
}