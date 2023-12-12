using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace RPG
{
    public class Menu : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene("level01");

        }
        
                    
        public void ExitGame()
        {
            Application.Quit();

        }

        

    }
}
