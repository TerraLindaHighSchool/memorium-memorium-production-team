using System.Collections;
using Game_Managing.Game_Context;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

namespace Player_Control
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    public class PlayerHandheldWeaponController : MonoBehaviour
    {
    
        public GameObject projectile;
	private GameContextManager _gameContextManager;
    
        // called when object is enabled
        void OnEnable()
	{
	    _gameContextManager = GameContextManager.Instance;
	    PlayerInputActions playerInputActions = PlayerInputManager.Instance.PlayerInputActions;
    
	    playerInputActions.Player.Fire.performed += OnFire;
        }
    
        // called when object is disabled
        void OnDisable()
        {
    	    PlayerInputActions playerInputActions = PlayerInputManager.Instance.PlayerInputActions;
        }
    
        // Update is called once per frame
        private void OnFire(InputAction.CallbackContext context)
        {
	    IGameContext activeContext         = _gameContextManager.ActiveContext;
	    Transform    playerFollowCamTarget = activeContext.GetPlayerFollowCamTarget();
	    Quaternion storedCamTargetRot = playerFollowCamTarget.rotation;
            // spawn projectile in front of player with a velocity forward and slightly up
	    GameObject projectileInstance = Instantiate(projectile, playerFollowCamTarget.position + playerFollowCamTarget.forward * 1.5f + Vector3.up * 0.5f, storedCamTargetRot);
	    projectileInstance.GetComponent<Rigidbody>().velocity = playerFollowCamTarget.forward * 10f;
        }
    }
}
