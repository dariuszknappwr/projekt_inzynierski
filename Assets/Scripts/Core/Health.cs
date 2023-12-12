using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RPG.Saving;

namespace RPG.Core
{
    public class Health : ISaveable
    {
        public float HealthPoints { get => healthPoints; private set { healthPoints = value; } }
        public bool IsDead { get => isDead; }
        public UnityEvent OnDeath, OnDamageReceived, OnHealed, OnResetDamage;
        public Action scheduleOnDeath;

        private float healthPoints;
        private bool isDead = false;
        private float maxHealthPoints;


        public Health(float maxHealthPoints)
        {
            this.maxHealthPoints = maxHealthPoints;
            HealthPoints = maxHealthPoints;
        }

        public void ResetDamage()
        {
            HealthPoints = maxHealthPoints;
            OnResetDamage?.Invoke();
        }

        public void TakeDamage(float damage)
        {
            if (damage < 0) throw new ArgumentOutOfRangeException("damage must be positive");
            if (damage == 0) return;

            HealthPoints = Mathf.Max(HealthPoints - damage, 0);
            OnDamageReceived?.Invoke();

            ApplyOnDeath();
        }

        public void Heal(float healAmount)
        {
            if (healAmount < 0) throw new ArgumentOutOfRangeException("healAmount must be positive");
            if (isDead || healAmount == 0) return;

            HealthPoints += Mathf.Min(maxHealthPoints - HealthPoints, healAmount);
            OnHealed?.Invoke();
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;
            ApplyOnDeath();
            ApplyOnResetDamage();
        }

        private void ApplyOnDeath()
        {
            if (healthPoints <= 0)
            {
                isDead = true;
                if (OnDeath != null)
                {
                    scheduleOnDeath += OnDeath.Invoke;
                }
            }
        }
        private void ApplyOnResetDamage()
        {
            if (healthPoints > 0)
            {
                isDead = false;
                OnResetDamage?.Invoke();
            }
        }
    }

}