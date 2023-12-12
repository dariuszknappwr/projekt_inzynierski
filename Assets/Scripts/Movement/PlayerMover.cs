using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Movement
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMover : MonoBehaviour, ISaveable
    {

       [Tooltip("Small ammount of gravity applied when character is close to the ground. Used to avoid floating slightly above ground")]

        private Transform playerObject;
        private Transform playerModel;
        public bool isMovementPressed;
        public bool isJumpPressed;
        public bool requireNewJumpPress = false;


        [SerializeField] AudioSource jumpSound;
        [SerializeField] float movementSpeed = 4f;
        [SerializeField] float jumpForce = 4f;
        [SerializeField] float gravity = -9.8f;
        [SerializeField] float groundedGravity = -0.05f;
        [SerializeField] Transform orientation;
        [HideInInspector] public Vector3 currentMovement;
        
        private CharacterController characterController;
        private bool forceJump = false;
        private Vector3 placeToMove = Vector3.zero;  
        private void Start() {

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;    
            
        }
         
        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            float sprintMultiplier = 1f;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                sprintMultiplier = 2.0f;
            }
            else if(Input.GetKey(KeyCode.LeftControl))
            {
                sprintMultiplier = 3.0f;
            }
            else
            {
                sprintMultiplier = 1.0f;
            }

            ApplyJump();
            Vector3 moveVector = new Vector3(placeToMove.x * sprintMultiplier, placeToMove.y, placeToMove.z * sprintMultiplier);
            characterController.Move(moveVector);
            placeToMove = Vector3.zero;
        }

        public void Move()
        {
            Vector3 moveDirection = orientation.forward * currentMovement.z + orientation.right * currentMovement.x;
            placeToMove += movementSpeed * Time.deltaTime * moveDirection;
        }

        public void Move(Vector3 direction, float moveFactor)
        {
            Vector3 moveDirection = new Vector3(direction.x,0, direction.z);
            placeToMove += moveFactor * Time.deltaTime * moveDirection;
        }

        public void AddGravityForce()
        {
            currentMovement.y += gravity * Time.deltaTime;
        }

        public void Jump()
        {
            currentMovement.y = jumpForce;
            requireNewJumpPress = true;
            
        }

        public void SetGravityForce(float gravity)
        {
            currentMovement.y = gravity;
        }

        public void SetGroundedGravity()
        {
            currentMovement.y = groundedGravity;
        }

        public bool CanJump()
        {
            if (forceJump)
            {
                forceJump = false;
                return true;
            }
            return !requireNewJumpPress && isJumpPressed;
        }

        public void ForceJump()
        {
            forceJump = true;
        }

        public bool CanMove()
        {
            return isMovementPressed;
        }

        private void ApplyJump()
        {
            placeToMove += orientation.up * currentMovement.y * jumpForce * Time.deltaTime;
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;
            GetComponent<CharacterController>().enabled = false;
            transform.position = position.ToVector();
            GetComponent<CharacterController>().enabled = true;
        }
    }

}