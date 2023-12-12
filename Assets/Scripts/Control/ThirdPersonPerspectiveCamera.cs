using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace RPG.Control
{
    public class ThirdPersonPerspectiveCamera : MonoBehaviour
    {
        [HideInInspector] public Vector3 currentMovement;
        public bool isAimPressed;
        [SerializeField] GameObject crosshairPrefab;
        [SerializeField] float rotationSpeed = 20;
        [SerializeField] Transform cameraFocus;
        [SerializeField] Transform playerModel;
        [SerializeField] Transform playerObject;
        [SerializeField] Transform orientation;
        [SerializeField] CharacterController characterController;
        [SerializeField] CinemachineFreeLook thirdPersonCamera;
        [SerializeField] CinemachineFreeLook aimCam;
        [SerializeField] CameraStyle currentStyle;
        public enum CameraStyle
        {
            Basic,
            Aim
        }
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            crosshairPrefab.SetActive(false);
        }
        void Update()
        {
            if (Input.GetKey(KeyCode.Mouse1)) SwitchCameraStyle(CameraStyle.Aim);
            if (!Input.GetKey(KeyCode.Mouse1)) SwitchCameraStyle(CameraStyle.Basic);
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            Vector3 lookDirection = playerObject.position - new Vector3(transform.position.x, playerObject.position.y, transform.position.z);
            orientation.forward = lookDirection.normalized;

            if (currentStyle == CameraStyle.Basic)
            {
                //basic
                Vector3 inputDirection = orientation.forward * moveZ + orientation.right * moveX;

                if (inputDirection != Vector3.zero)
                {
                    playerModel.forward = Vector3.Slerp(playerModel.forward, inputDirection.normalized, Time.deltaTime * rotationSpeed);
                }
            }
            else if (currentStyle == CameraStyle.Aim)
            {
                Vector3 aimDirection = cameraFocus.position - new Vector3(transform.position.x, cameraFocus.position.y, transform.position.z);
                orientation.forward = aimDirection.normalized;
                playerModel.forward = aimDirection.normalized;
            }
        }

        

        private void SwitchCameraStyle(CameraStyle newStyle)
        {
            if (newStyle == CameraStyle.Basic)
            {
                crosshairPrefab.SetActive(false);
                thirdPersonCamera.Priority = 20;
                aimCam.Priority = 10;
            }
            if (newStyle == CameraStyle.Aim)
            {
                crosshairPrefab.SetActive(true);
                thirdPersonCamera.Priority = 10;
                aimCam.Priority = 20;
            }
            currentStyle = newStyle;
        }
    }
}