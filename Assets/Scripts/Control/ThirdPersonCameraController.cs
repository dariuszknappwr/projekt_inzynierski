using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
namespace RPG
{
    
    public class ThirdPersonCameraController : MonoBehaviour
    {
        [HideInInspector] public bool isAimPressed;

        [SerializeField] private CinemachineFreeLook aimCamera;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (isAimPressed)
            {

                aimCamera.gameObject.SetActive(true);

            }
            else {
                aimCamera.gameObject.SetActive(false);
            }
        }
    }
}
