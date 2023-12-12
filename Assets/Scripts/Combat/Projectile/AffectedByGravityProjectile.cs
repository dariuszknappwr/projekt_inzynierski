using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class AffectedByGravityProjectile : Projectile
    {
        [SerializeField] float startingUpForce = 10f;
        [SerializeField] float startingForwardForce = 100f;
        [SerializeField] int maxBounces = 4;
        [SerializeField] float maxBloom = 1;
        [SerializeField] float maxTimeOnFloor = 0.1f;

        private int bouncesCount = 0;
        private Rigidbody rb;
        private float currentTimeOnFloor = 0f;
        private void Start()
        {
            this.transform.rotation = owner.transform.Find("PlayerModel").rotation;
            Physics.IgnoreLayerCollision(owner.layer, this.gameObject.layer);
            rb = this.gameObject.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * startingForwardForce);
            rb.AddForce(transform.up * startingUpForce);
            float bloom = Random.Range(-maxBloom, maxBloom);
            rb.AddForce(transform.right * bloom);
        }

        void Update()
        {

        }

        private void OnCollisionEnter(Collision collision)
        {
            currentTimeOnFloor = 0f;
            Collider other = collision.collider;

            if (other.GetComponent<HealthBehaviour>() != null)
            {
                if (Fighter.IsMatchingTargetLayers(other, targetLayers))
                {
                    other.GetComponent<HealthBehaviour>().TakeDamage(damage);
                    onHitTarget?.Invoke();
                }
            }
            else
            {
                bouncesCount++;
                if (bouncesCount >= maxBounces)
                {
                    DestroyItself();
                }
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            currentTimeOnFloor += Time.deltaTime;
            if(currentTimeOnFloor > maxTimeOnFloor)
            {
                DestroyItself();
            }
        }
    }
}
