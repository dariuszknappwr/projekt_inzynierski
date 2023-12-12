using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class FindNearestEnemy : MonoBehaviour
    {
        [SerializeField] GameObject arrow;

        void Update()
        {
            CombatTarget nearestEnemy = FindClosestAliveCombatTarget(this.gameObject);
            if(nearestEnemy != null)
            {
                arrow.transform.LookAt(nearestEnemy.transform);
            }
        }

        public CombatTarget FindClosestAliveCombatTarget(GameObject owner)
        {
            CombatTarget[] targets = FindAliveCombatTargets(owner);
            if (targets.Length == 0) return null;

            float minDistance = Vector3.Distance(targets[0].transform.position, owner.transform.position);
            CombatTarget closestTarget = targets[0];
            for (int i = 0; i < targets.Length; i++)
            {
                float distance = Vector3.Distance(targets[i].transform.position, owner.transform.position);
                if (distance < minDistance)
                {
                    closestTarget = targets[i];
                    minDistance = distance;
                }
            }
            return closestTarget;
        }

        private CombatTarget[] FindAliveCombatTargets(GameObject owner)
        {
            CombatTarget[] targets = GameObject.FindObjectsOfType<CombatTarget>();
            List<CombatTarget> aliveTargets = new List<CombatTarget>();
            foreach (CombatTarget target in targets)
            {
                if (target.gameObject == owner.gameObject) continue;
                if (!Fighter.IsMatchingTargetLayers(target.gameObject, GetComponent<Fighter>().targetLayers)) continue;
                HealthBehaviour health = target.GetComponent<HealthBehaviour>();
                if (health == null) continue;
                if (health.IsDead()) continue;
                aliveTargets.Add(target);
            }
            return aliveTargets.ToArray();
        }
    }
}
