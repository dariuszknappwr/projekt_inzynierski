using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RPG.Core
{
    public class HealthBehaviour : MonoBehaviour, ISaveable
    {
        public float maxHealthPoints = 4;
        
        public Image[] hearts;
        public Sprite fullHeart;
        public Sprite deadHeart;
        
        public UnityEvent OnDeath, OnDamageReceived, OnHealed, OnResetDamage;

        private Health health;
        [SerializeField] private float currentHealth;

        private void Awake()
        {
            health = new Health(maxHealthPoints);
            health.OnResetDamage = OnResetDamage;
            health.OnDamageReceived = OnDamageReceived;
            health.OnHealed = OnHealed;
            health.OnDeath = OnDeath;
        }

        private void Update()
        {
            currentHealth = health.HealthPoints;
            for (int i = 0; i < hearts.Length; i++)
            {
                if (i < currentHealth)
                {
                    hearts[i].sprite = fullHeart;
                }
                else
                {
                    hearts[i].sprite = deadHeart;
                }

                if (i < maxHealthPoints)
                {
                    hearts[i].enabled = true;

                }
                else
                {
                    hearts[i].enabled = false;
                }
            }
        }

        void LateUpdate()
        {
            if (health.scheduleOnDeath != null)
            {
                health.scheduleOnDeath();
                health.scheduleOnDeath = null;
            }
        }

        public void ResetDamage()
        {
            health.ResetDamage();
        }

        public void TakeDamage(float damage)
        {
            health.TakeDamage(damage);
        }

        public void Heal(float healAmount)
        {
            health.Heal(healAmount);
        }

        public float GetCurrentHealth()
        {
            return health.HealthPoints;
        }

        public bool IsDead()
        {
            return health.IsDead;
        }

        public object CaptureState()
        {
            return health.CaptureState();
        }

        public void RestoreState(object state)
        {
            health.RestoreState(state);
        }
    }
}

