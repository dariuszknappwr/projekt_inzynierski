using RPG.Movement;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Control
{
    public class PlayerInputHandler : MonoBehaviour
    {
       
        HFSMPlayerStateMachine ctx;
        PlayerMover mover;
        UserInput userInput;
        ThirdPersonPerspectiveCamera tppCamera;
        
        private void Awake()
        {
            userInput = new UserInput();
            ctx = GetComponent<HFSMPlayerStateMachine>();
            mover = GetComponent<PlayerMover>();
            
            tppCamera = Camera.main.GetComponent<ThirdPersonPerspectiveCamera>();

            OnUserInput();
        }

        public void OnUserInput()
        {
            userInput.CharacterControls.Move.started += OnMovementInput;
            userInput.CharacterControls.Move.canceled += OnMovementInput;
            userInput.CharacterControls.Move.performed += OnMovementInput;

            userInput.CharacterControls.Jump.started += OnJump;
            userInput.CharacterControls.Jump.canceled += OnJump;

            userInput.CharacterControls.Attack.started += OnAttack;
            userInput.CharacterControls.Attack.canceled += OnAttack;

            

           
            
        }

        void OnMovementInput(InputAction.CallbackContext inputContext)
        {
            mover.currentMovement.x = inputContext.ReadValue<Vector2>().x;
            mover.currentMovement.z = inputContext.ReadValue<Vector2>().y;

            //tppCamera.currentMovement.x = inputContext.ReadValue<Vector2>().x;
            //tppCamera.currentMovement.z = inputContext.ReadValue<Vector2>().y;


            mover.isMovementPressed = mover.currentMovement.x != 0 || mover.currentMovement.z != 0;
        }
        public void OnJump(InputAction.CallbackContext context)
        {
            mover.isJumpPressed = context.ReadValueAsButton();
            mover.requireNewJumpPress = false;
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            ctx.isAttackPressed = context.ReadValueAsButton();
        }

        

        
        void OnEnable()
        {
            userInput.CharacterControls.Enable();
        }

        void OnDisable()
        {
            userInput.CharacterControls.Disable();
        }
    }

}