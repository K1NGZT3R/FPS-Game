using System.Dynamic;
using UnityEngine;
using UnityEngine.Animations;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool fire;

		[Header("Ammo Control")]
        public GameObject AmmoCount;
		public GameObject ReloadText;

		public float ammo;

        [Header("Movement Settings")]
		public bool analogMovement;

		[Header("Movement Control for Cutscene")]
        public bool movementSwitch;

        [Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value)
		{
            if (movementSwitch == true)
                MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
            if (movementSwitch == true)
                if (cursorInputForLook)
				{
				LookInput(value.Get<Vector2>());
				}
		}

		public void OnJump(InputValue value)
		{
            if (movementSwitch == true)
                JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
            if (movementSwitch == true)
                SprintInput(value.isPressed);
		}

		public void OnFire (InputValue value)
		{
            if (movementSwitch == true)
                FireInput(value.isPressed);
		}
#endif
		public void Start()
		{
			resetAmmo();
			movementSwitch = true;
		}

		public void Update()
		{
			if (Input.GetKeyDown(KeyCode.R))
			{
				resetAmmo();
			}
		}

		public void resetAmmo()
		{
            ammo = 7;
        }
		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void FireInput(bool newFireState)
		{
            fire = newFireState;
            ReloadText.SetActive(true);
        }
		
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}