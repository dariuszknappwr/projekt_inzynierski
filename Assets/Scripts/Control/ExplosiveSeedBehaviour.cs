using FSM;
using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class ExplosiveSeedBehaviour : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float explodeDistance = 1f;
        [SerializeField] float maxChaseTime = 5f;
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
                    animator.SetBool("IsAlerted", true);  
                },
                onLogic: (state) =>
                {
                    agent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);

                    if (state.timer.Elapsed > maxChaseTime || DistanceToPlayer() < explodeDistance)
                        state.fsm.StateCanExit();
                }, needsExitTime: true
                );
            fsm.AddState("Explode",
                onEnter: (state) =>
            {
                Die();
            });

            fsm.SetStartState("Idle");
            fsm.AddTransition("Idle", "Chase", (transition) => DistanceToPlayer() < chaseDistance);
            fsm.AddTransition("Chase", "Explode");
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
            animator.SetBool("IsExploding", true);
            fighter.Attack();
            agent.isStopped = true;
            GetComponent<Collider>().enabled = false;
            model.SetActive(false);
            deathParticles.Play();
            Destroy(gameObject, 1);
        }

    }
}
