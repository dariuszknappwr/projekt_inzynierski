using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weapon;
        [SerializeField] float respawnTime = 5;
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                other.GetComponent<Fighter>().EquipWeapon(weapon);
                StartCoroutine(HideAndRespawnAfterTime(respawnTime));
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }

        private IEnumerator HideAndRespawnAfterTime(float seconds)
        {
            MakeVisible(false);
            yield return new WaitForSeconds(seconds);
            MakeVisible(true);
        }

        private void MakeVisible (bool isVisible)
        {
            GetComponent<Collider>().enabled = isVisible;
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(isVisible);
            }
        }
    }
}

