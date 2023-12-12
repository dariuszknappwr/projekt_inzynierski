using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class ReturningProjectile : HomingProjectile
    {
        public UnityEvent onHitOwner, onReturnToOwnerEnter;
        private bool isReturningToOwner = false;
        private bool didHitTarget;

        private void Start()
        {
            target = FindClosestAliveCombatTarget(owner);
            if (target != null)
            {
                targetHealth = target.GetComponent<HealthBehaviour>();
            }
        }

        private void Update()
        {
            if (isReturningToOwner)
            {
                transform.position -= (transform.position - owner.GetComponent<Collider>().bounds.center) * speed * Time.deltaTime;
            }
            else if (!didHitTarget)
            {
                if (!targetHealth.IsDead())
                {
                    transform.LookAt(GetAimLocation());
                }
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
        }

        public void LieOnGroundForSeconds(float time)
        {
            StartCoroutine(LieOnGround(time));
        }
        private IEnumerator LieOnGround(float time)
        {
            transform.position = target.transform.position;
            Quaternion parallelToGround = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            transform.rotation = parallelToGround;
            yield return new WaitForSeconds(time);
            ReturnToOwner();
        }

        public void ReturnToOwner()
        {
            isReturningToOwner = true;
            onReturnToOwnerEnter?.Invoke();
        }

        private Vector3 GetAimLocation()
        {
            Collider targetCollider = target.GetComponent<Collider>();
            if (targetCollider == null) return target.transform.position;
            return targetCollider.bounds.center;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (didHitTarget)
            {
                if (other.gameObject == owner)
                {
                    onHitOwner?.Invoke();
                }
            }
            else if (Fighter.IsMatchingTargetLayers(other, targetLayers) && target.gameObject.Equals(other.gameObject))
            {
                didHitTarget = true;
                targetHealth.TakeDamage(damage);
                onHitTarget?.Invoke();
            }
        }
    }

}