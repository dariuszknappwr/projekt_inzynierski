using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class SceneTransitionSource : MonoBehaviour
    {
        [SerializeField] int sceneToTransition = -1;
        [SerializeField] SceneTransitionDestination.DestinationTag destinationTag;
        private void OnTriggerEnter(Collider other)
        {
            bool enoughEnemiesKilled = GameObject.FindGameObjectsWithTag("Enemy").Length < 3;
            if (other.gameObject.tag == "Player" && enoughEnemiesKilled)
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut();
            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            wrapper.Save();
            yield return SceneManager.LoadSceneAsync(sceneToTransition);
            wrapper.Load();
            SceneTransitionDestination destination = FindEnterence(destinationTag);
            TeleportPlayer(destination);
            wrapper.Save();
            yield return new WaitForSeconds(0.3f); //to stablize camera
            yield return fader.FadeIn();
            Destroy(gameObject);
        }

        private void TeleportPlayer(SceneTransitionDestination enterence)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.SetActive(false); // To prevent CharacterController from twitching
            player.transform.position = enterence.transform.position;
            player.transform.rotation = enterence.transform.rotation;
            player.SetActive(true);
        }

        private SceneTransitionDestination FindEnterence(SceneTransitionDestination.DestinationTag destinationTag)
        {
            SceneTransitionDestination[] enterences = FindObjectsOfType<SceneTransitionDestination>();
            foreach (SceneTransitionDestination enterence in enterences)
            {
                if (enterence.destinationTag == destinationTag)
                {
                    return enterence;
                }
            }
            Debug.LogWarning("No enterence with destination tag " + destinationTag + " was found");
            return null;
        }
    } 
}
