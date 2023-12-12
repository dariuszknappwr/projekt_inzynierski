using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class MeleeWeapon : Weapon
    {
        [System.Serializable]
        public class AttackPoint
        {
            public float radius;
            public Vector3 offset;
            public Transform attackRoot;
        }

        public AttackPoint[] attackPoints = new AttackPoint[0];
        [Tooltip("Duration when weapon is active, checks for collider and deals damage")]
        [SerializeField] float checkingDamageDuration = 1;
        private bool isAttacking = false;
        private Vector3[] previousPositionOfAttackPoints = null;
        private List<GameObject> damagedTargets = new List<GameObject>();
        private float timeSinceAttackStarted = 0f;
        [HideInInspector] public LayerMask targetLayers;

        public override void Attack(DamageData data)
        {
            owner = data.owner;
            targetLayers = data.targetLayers;
            isAttacking = true;
            previousPositionOfAttackPoints = new Vector3[attackPoints.Length];
            UpdateAnimator(owner);
        }

        private void UpdateAnimator(GameObject owner)
        {
            Animator animator;
            if (owner.TryGetComponent(out animator))
            {
                animator.SetBool("IsAttacking", true);
            }
            else
            {
                Debug.LogWarning("Owner: " + owner.name + " does not have an Animator");
            }
        }

        private void FixedUpdate()
        {
            if (!isAttacking) return;
            timeSinceAttackStarted += Time.deltaTime;
            for (int i = 0; i < attackPoints.Length; i++)
            {
                AttackPoint point = attackPoints[i];
                Vector3 worldPosition = point.attackRoot.position + point.attackRoot.TransformVector(point.offset);
                Vector3 attackVector = worldPosition - previousPositionOfAttackPoints[i];
                attackVector = CollisionChecker.MakeVectorNonZero(attackVector);

                Ray ray = new Ray(worldPosition, attackVector);
                RaycastHit[] raycasthit = new RaycastHit[32];
                int contacts = Physics.SphereCastNonAlloc(ray, point.radius, raycasthit, attackVector.magnitude, ~0, QueryTriggerInteraction.Ignore);

                for (int k = 0; k < contacts; ++k)
                {
                    Collider col = raycasthit[k].collider;

                    if (col != null)
                        CheckDamage(point, col);
                }

                previousPositionOfAttackPoints[i] = worldPosition;
            }
            if (timeSinceAttackStarted > checkingDamageDuration)
            {
                StopAttack();
            }
        }

        public void StopAttack()
        {
            isAttacking = false;
            damagedTargets.Clear();
            timeSinceAttackStarted = 0;
        }

        private void CheckDamage(AttackPoint point, Collider other)
        {
            HealthBehaviour health = other.GetComponent<HealthBehaviour>();
            if (health == null) return;
            if (health.gameObject == owner) return;
            if (damagedTargets.Contains(health.gameObject)) return;
            if (Fighter.IsMatchingTargetLayers(other, targetLayers))
            {
                damagedTargets.Add(health.gameObject);
                health.TakeDamage(Damage);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            for (int i = 0; i < attackPoints.Length; ++i)
            {
                AttackPoint pts = attackPoints[i];

                if (pts.attackRoot != null)
                {
                    Vector3 worldPos = pts.attackRoot.TransformVector(pts.offset);
                    Gizmos.color = new Color(1.0f, 1.0f, 1.0f, 0.4f);
                    Gizmos.DrawSphere(pts.attackRoot.position + worldPos, pts.radius);
                }
            }
        }
#endif
    }

}