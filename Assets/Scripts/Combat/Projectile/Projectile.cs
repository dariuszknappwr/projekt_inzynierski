using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        public UnityEvent onHitTarget;
        [SerializeField] protected float speed = 5.0f;
        [SerializeField] float maxLifeTime = 10f;
        protected float damage = 0;
        protected LayerMask targetLayers;
        protected GameObject owner;
        protected IRangedWeapon weapon;
        private Coroutine destroingCorutine = null;

        private void Start()
        {
            transform.rotation = owner.transform.Find("PlayerModel").rotation;
        }

        void Update()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<HealthBehaviour>() != null)
            {
                if (Fighter.IsMatchingTargetLayers(other, targetLayers))
                {
                    other.GetComponent<HealthBehaviour>().TakeDamage(damage);
                    onHitTarget?.Invoke();
                }
            }
        }

        public void SetTarget(float damage, LayerMask targetLayers, GameObject owner, IRangedWeapon rangedWeapon)
        {
            this.damage = damage;
            this.targetLayers = targetLayers;
            this.owner = owner;
            this.weapon = rangedWeapon;

            DestroyItself(maxLifeTime);
        }

        public void DestroyItself()
        {
            Destroy(this.gameObject);
        }

        public void DestroyItself(float timeToExpire)
        {
            destroingCorutine = StartCoroutine(DestroyProjectileAfterTime(timeToExpire));
        }
        public void StopDestruction()
        {
            if (destroingCorutine != null)
            {
                StopCoroutine(destroingCorutine);
            }
        }

        public IEnumerator DestroyProjectileAfterTime(float time)
        {
            yield return new WaitForSeconds(time);
            DestroyItself();
        }

        private Vector3 GetDirectionToOwner()
        {
            Collider ownerCollider = owner.GetComponent<Collider>();
            return (Vector3.MoveTowards(transform.position, ownerCollider.bounds.center, 1000)).normalized;
        }

        public void ResetAmmo()
        {
            if (weapon != null)
            {
                weapon.ResetAmmo();
            }
        }
    }
}
