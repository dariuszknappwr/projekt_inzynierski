using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] bool disableDamageOnTouchingEnemy = false;
        [SerializeField] float range = 2f;
        [SerializeField] float damage = 5f;
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] string resourceName = "";
        protected GameObject owner = null;

        const string weaponName = "Weapon";
        public float Range { get => range; }
        public float Damage { get => damage; }
        public GameObject EquippedPrefab { get => equippedPrefab;}
        public bool IsRightHanded { get => isRightHanded; }
        public AnimatorOverrideController AnimatorOverride { get => animatorOverride; set => animatorOverride = value; }
        public bool DisableDamageOnTouchingEnemy { get => disableDamageOnTouchingEnemy; }
        public string ResourceName { get => resourceName; }

        abstract public void Attack(DamageData damageData);
        public virtual void ActionOnStomp(GameObject owner, GameObject stomped)
        {

        }

        public string GetWeaponName()
        {
            return weaponName;
        }

        public string GetRealWeaponName() {
            return resourceName;
        }
        public void SetOwner(GameObject owner)
        {
            this.owner = owner;
        }

#if UNITY_EDITOR
        public void SetTestData(GameObject equippedPrefab)
        {
            this.equippedPrefab = equippedPrefab;
        }
#endif
    }

}