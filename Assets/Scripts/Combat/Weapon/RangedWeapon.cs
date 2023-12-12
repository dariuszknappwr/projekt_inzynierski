using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;

namespace RPG.Combat
{
    public class RangedWeapon : Weapon, IRangedWeapon
    {
        [SerializeField] Projectile projectile = null;
        [SerializeField] Transform projectileSpawn = null;
        [SerializeField] int maxAmmo;
        [Tooltip("Set how many instances of projectiles you want to spawn")]
        [SerializeField] int spawnCount = 1;
        [SerializeField] float timeBetweenSpawning = 0.1f;

        private int currentAmmo = 0;

        protected void OnEnable()
        {
            ResetAmmo();
        }

        public override void Attack(DamageData data)
        {
            if (currentAmmo <= 0) return;

            StartCoroutine(LaunchProjectile(owner, data.targetLayers));
            data.owner.GetComponent<Animator>().SetBool("IsAttacking", true);
        }
        public IEnumerator LaunchProjectile(GameObject owner, LayerMask targetLayers)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                Projectile projectileInstance = Instantiate(projectile, projectileSpawn.position, owner.transform.rotation);
                projectileInstance.SetTarget(Damage, targetLayers, owner, this);
                currentAmmo--;
                yield return new WaitForSeconds(timeBetweenSpawning);
            }
        }

        public void ResetAmmo()
        {
            currentAmmo = maxAmmo;
        }
    }

}