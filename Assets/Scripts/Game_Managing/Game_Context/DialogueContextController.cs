using System;
using System.Collections.Generic;
using Cinemachine;
using NPC_Control;
using Player_Control;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game_Managing.Game_Context {
	public class DialogueContextController : IGameContext {
		private NPC _currentDialogueNPC;

		private List<GameObject> _currentDialogueNPCChildren;

		private Transform _playerFollowCamTarget;

		private AnimationManager _animationManager;

		public DialogueContextController(NPC npc) { _currentDialogueNPC = npc; }

		public void GCStart() {
			_animationManager = AnimationManager.Instance;
			
			_playerFollowCamTarget = GameObject.Find("LookAtTarget").transform;

			_currentDialogueNPCChildren = new List<GameObject>();

			Vector3 playerPos = _playerFollowCamTarget.position;
			Vector3 npcPos    = _currentDialogueNPC.gameObject.transform.position;
			GameObject midPointObject =
				new GameObject($"{_currentDialogueNPC.name} Dialogue MidPoint") {
					transform = {
						            position =
							            Vector3.Lerp(
								            playerPos, npcPos, 0.5f)
					            }
				};
			midPointObject.transform.SetParent(_currentDialogueNPC.transform, true);
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

			dialogueCamPos.y = playerPos.y;

			GameObject newCameraObject =
				new GameObject($"{_currentDialogueNPC.name} Dialogue Camera") {transform = {position = dialogueCamPos}};
			newCameraObject.transform.SetParent(_currentDialogueNPC.transform, true);
			_currentDialogueNPCChildren.Add(newCameraObject);

			CinemachineVirtualCamera vCam = newCameraObject.AddComponent<CinemachineVirtualCamera>();
			vCam.Priority = 15;

			GameObject targetGroupObject = new GameObject($"{_currentDialogueNPC.name} Dialogue Group Composer");
			targetGroupObject.transform.SetParent(_currentDialogueNPC.transform, true);
			_currentDialogueNPCChildren.Add(targetGroupObject);
			CinemachineTargetGroup targetGroup = targetGroupObject.AddComponent<CinemachineTargetGroup>();

			targetGroup.AddMember(_playerFollowCamTarget, 1.0f, 0.0f);
			targetGroup.AddMember(_currentDialogueNPC.transform, 1.0f, 0.0f);

			CinemachineGroupComposer currentDialogueVCamComposer =
				vCam.AddCinemachineComponent<CinemachineGroupComposer>();

			vCam.LookAt = targetGroupObject.transform;

			currentDialogueVCamComposer.m_MinimumFOV     = 60;
			currentDialogueVCamComposer.m_MaximumFOV     = 60;
			currentDialogueVCamComposer.m_AdjustmentMode = CinemachineGroupComposer.AdjustmentMode.ZoomOnly;
			
			_animationManager.EnterDialogueWithPlayer(_currentDialogueNPC);
		}

		public void GCUpdateDelta(Vector2 mouseDelta, bool lcDown, bool rcDown) { }
		public void GCUpdatePos(Vector2   mousePos,   bool lcDown, bool rcDown) { }

		// These two methods should never be called in a dialogue context
		public float     GetYRotForForwards()       { throw new NotImplementedException(); }
		public Transform GetPlayerFollowCamTarget() { throw new NotImplementedException(); }

		public void GCExit() {
			_animationManager.ExitDialogueWithPlayer(_currentDialogueNPC);
			
			_currentDialogueNPC = null;

			foreach (GameObject obj in _currentDialogueNPCChildren) Object.Destroy(obj);

			_currentDialogueNPCChildren.Clear();

			OnExit?.Invoke();
		}

		public event Action OnExit;
	}
}