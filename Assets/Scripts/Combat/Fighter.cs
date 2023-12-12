using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;
using RPG.Saving;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, ISaveable
    {
        [SerializeField] Weapon defaultWeapon = null;
        [SerializeField] float timeBetweenAttacks = 1f;
        public LayerMask targetLayers;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] float stompDamage = 0f;
        [SerializeField] float timeSinceLastAttack = Mathf.Infinity;

        private Weapon currentWeapon = null;

        public float TimeBetweenAttacks { get => timeBetweenAttacks; }
        public float TimeSinceLastAttack { get => timeSinceLastAttack; }
        public float StompDamage { get => stompDamage; }
        public Weapon CurrentWeapon { get => currentWeapon; private set => currentWeapon = value; }

        private void Start()
        {
            if (currentWeapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
            timeSinceLastAttack = Mathf.Infinity;

            //Debug.Log(currentWeapon.GetWeaponName());
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        public void Attack()
        {
            if (TimeBetweenAttacksPassed())
            {
                DamageData data = new DamageData
                {
                    owner = gameObject,
                    targetLayers = targetLayers
                };
                CurrentWeapon.Attack(data);
                timeSinceLastAttack = 0;
            }
        }

        public bool TimeBetweenAttacksPassed()
        {
            return timeSinceLastAttack >= timeBetweenAttacks;
        }

        public void EquipWeapon(Weapon weapon)
        {
            Animator animator = GetComponent<Animator>();
            Weapon newWeapon = SpawnWeapon(weapon, leftHandTransform, rightHandTransform, animator, targetLayers);
            newWeapon.SetOwner(gameObject);
            CurrentWeapon = newWeapon;
        }

        public void InvokeOnStomp(GameObject owner, GameObject stomped)
        {
            if (CurrentWeapon != null)
            {
                CurrentWeapon.ActionOnStomp(owner, stomped);
            }
        }

        public static bool IsMatchingTargetLayers(Collider target, LayerMask layers)
        {
            return ((1 << target.gameObject.layer) & layers) != 0;
        }
        public static bool IsMatchingTargetLayers(GameObject target, LayerMask layers)
        {
            return ((1 << target.layer) & layers) != 0;
        }

        private Weapon SpawnWeapon(Weapon weapon, Transform leftHand, Transform rightHand, Animator animator, LayerMask targetLayers)
        {
            DestroyOldWeapon(rightHand, leftHand, weapon);
            GameObject newEquippedWeapon = null;

            if (weapon.EquippedPrefab != null)
            {
                newEquippedWeapon = Instantiate(weapon.EquippedPrefab.gameObject, GetHandTransform(rightHand, leftHand, weapon));
                newEquippedWeapon.name = weapon.GetWeaponName();
            }
            OverrideAnimations(animator, weapon);
            return newEquippedWeapon.GetComponentInChildren<Weapon>();
        }
        private void OverrideAnimations(Animator animator, Weapon weapon)
        {
            var currentRunningOverrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (weapon.AnimatorOverride != null)
            {
                animator.runtimeAnimatorController = weapon.AnimatorOverride;
            }
            else if (currentRunningOverrideController != null)
            {
                animator.runtimeAnimatorController = currentRunningOverrideController.runtimeAnimatorController;
            }
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand, Weapon weapon)
        {
            Transform oldWeapon = rightHand.transform.Find(weapon.GetWeaponName());
            if (oldWeapon == null)
            {
                oldWeapon = leftHand.transform.Find(weapon.GetWeaponName());
            }
            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        protected Transform GetHandTransform(Transform rightHand, Transform leftHand, Weapon weapon)
        {
            return weapon.IsRightHanded ? rightHand : leftHand;
        }

       

        public object CaptureState()
        {
            return CurrentWeapon.ResourceName;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            GameObject weapon = Resources.Load<GameObject>(weaponName);
            EquipWeapon(weapon.GetComponent<Weapon>());
        }
    }
}

