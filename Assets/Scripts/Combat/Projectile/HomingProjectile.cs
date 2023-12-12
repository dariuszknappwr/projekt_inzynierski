using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class HomingProjectile : Projectile
    {
        protected CombatTarget target;
        protected HealthBehaviour targetHealth;
        [SerializeField] bool rotateToTargetOnAttack = false;

        void Start()
        {
            target = FindClosestAliveCombatTarget(owner);
            if (target != null)
            {
                targetHealth = target.GetComponent<HealthBehaviour>();
            }
            if (rotateToTargetOnAttack)
            {
                if (target != null)
                {
                    owner.transform.LookAt(target.transform.position);
                }
            }
        }

        void Update()
        {
            if (targetHealth != null && !targetHealth.IsDead())
            {
                transform.LookAt(target.GetComponent<Collider>().bounds.center);
            }
            transform.Translate(speed * Time.deltaTime * Vector3.forward);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Fighter.IsMatchingTargetLayers(other, targetLayers))
            {
                targetHealth.TakeDamage(damage);
                onHitTarget?.Invoke();
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
                if (!Fighter.IsMatchingTargetLayers(target.gameObject, targetLayers)) continue;
                HealthBehaviour health = target.GetComponent<HealthBehaviour>();
                if (health == null) continue;
                if (health.IsDead()) continue;
                aliveTargets.Add(target);
            }
            return aliveTargets.ToArray();
        }
    }
}
