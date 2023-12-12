using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class PauseMenu : MonoBehaviour
    {

        public bool isGamePaused = false;
        public GameObject pauseMenuUi;
        public GameObject cameraTpp;
        public GameObject cameraAim;
        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if (isGamePaused)
                {

                    Resume();
                }
                else
                {
                    Pause();

                }
            }

            
        }

        public void Resume()
        {
            pauseMenuUi.SetActive(false);
            cameraTpp.SetActive(true);
            cameraAim.SetActive(true);
            Time.timeScale = 1f;
            isGamePaused = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }


        public void Pause()
        {
            pauseMenuUi.SetActive(true);
            cameraTpp.SetActive(false);
            cameraAim.SetActive(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0f;
            isGamePaused = true;

        }
    }
}
