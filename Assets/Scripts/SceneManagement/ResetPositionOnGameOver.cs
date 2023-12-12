using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class ResetPositionOnGameOver : MonoBehaviour
    {
        
        public void ResetPosition()
        {
            GameObject.FindObjectOfType<SavingWrapper>().GetComponent<SavingWrapper>().LoadLastScene();
        }
    }
}
