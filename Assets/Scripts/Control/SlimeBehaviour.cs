using FSM;
using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class SlimeBehaviour : MonoBehaviour
    {
        [SerializeField] float attackDistance = 2f;
        [SerializeField] float minchaseDistance = 5f;
        [SerializeField] float maxchaseDistance = 10f;
        [SerializeField] GameObject model;
        [SerializeField] ParticleSystem deathParticles;
        private StateMachine fsm;
        private NavMeshAgent agent;
        private Fighter fighter;
        private Animator animator;

        private void Start()
        {
            fsm = new StateMachine(needsExitTime: true);
            agent = GetComponent<NavMeshAgent>();
            fighter = GetComponent<Fighter>();
            animator = GetComponent<Animator>();

            fsm.AddState("Idle");

            fsm.AddState("Chase",
                onEnter: (state) =>
                {
                    animator.SetBool("IsRunning", true);
                },
                onLogic: (state) =>
                {
                    agent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
                },
                onExit: (state) =>
                {
                    animator.SetBool("IsRunning", false);
                }, needsExitTime: true);
            fsm.AddState("Attack",
                onEnter: (state) =>
                {
                    animator.SetBool("IsAttacking", true);
                },
                onLogic:(state) =>
                {
                    fighter.Attack();
                },
                onExit: (state) =>
                {
                    animator.SetBool("IsAttacking", false);
                });

            fsm.SetStartState("Idle");
            fsm.AddTransition("Idle", "Chase", (transition) => DistanceToPlayer() < minchaseDistance);
            fsm.AddTransition("Chase", "Attack", (transition) => DistanceToPlayer() > attackDistance);
            fsm.AddTransition("Chase", "Idle", (transition) => DistanceToPlayer() > maxchaseDistance);
            fsm.AddTransition("Attack", "Idle", (transition) => DistanceToPlayer() > maxchaseDistance);
            fsm.Init();
        }

        private void Update()
        {
            fsm.OnLogic();
        }

        float DistanceToPlayer()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            return Vector3.Distance(transform.position, player.transform.position);
        }

        public void Die()
        {
            agent.isStopped = true;
            GetComponent<Collider>().enabled = false;
            model.SetActive(false);
            deathParticles.Play();
            Destroy(gameObject, 1);
        }

    }
}
