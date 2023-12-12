using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SceneTransitionDestination : MonoBehaviour
    {
        public enum DestinationTag
        {
            A, B, C, D, E, F, G,
        }

        public DestinationTag destinationTag;
    }

}