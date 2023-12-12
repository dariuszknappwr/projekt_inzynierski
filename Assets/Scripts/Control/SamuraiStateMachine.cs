using FSM;
using RPG.Combat;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SamuraiStateMachine : MonoBehaviour
{
    [SerializeField] int attackTypeCount = 3;
    [SerializeField] float ownScanningRange = 10f;
    [SerializeField] float ownAttackRange = 2f;
    [SerializeField] float maxAttackTime = 1f;
    [SerializeField] float stoppingDistance = 3f;

    private StateMachine fsm;
    private NavMeshAgent agent;
    private Fighter fighter;
    Animator animator;
    private Vector3 guardPosition;

    private void Start()
    {
        fsm = new StateMachine(needsExitTime: true);
        agent = GetComponent<NavMeshAgent>();
        fighter = GetComponent<Fighter>();
        animator = GetComponent<Animator>();
        guardPosition = transform.position;

        fsm.AddState("Attack",
            onEnter: (state) =>
            {
                agent.isStopped = true;
                animator.SetBool("IsRunnning", false);
                
            },
            onLogic: (state) =>
            {
                if (attackTypeCount > 0)
                {
                    animator.SetInteger("AttackType", Random.Range(0, attackTypeCount));
                }
                fighter.Attack();

                if (state.timer.Elapsed > maxAttackTime )
                    state.fsm.StateCanExit();

            }, needsExitTime: true
            );

        fsm.AddState("ChasePlayer",
            onEnter: (state) =>
            {
                animator.SetBool("IsRunning", true);
            },
            onLogic: (state) =>
            {
                agent.isStopped = DistanceToPlayer() <= stoppingDistance;
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                agent.SetDestination(player.transform.position);
            }
            );

        fsm.AddState("Guard",
                onEnter: (state) =>
                {
                    animator.SetBool("IsRunning", false);
                },
                onLogic: (state) =>
                {
                    agent.isStopped = false;
                    agent.SetDestination(guardPosition);
                }
                );

        fsm.AddState("Idle",
            onEnter: (state) =>
            {
                animator.SetBool("IsRunning", false);
            },
            onLogic: (state) =>
            {
                agent.isStopped = true;
            });

        fsm.SetStartState("Guard");
        fsm.AddTransition("Guard", "ChasePlayer", (transition) => PlayerIsAlive() && DistanceToPlayer() < ownScanningRange);
        fsm.AddTransition("Idle", "ChasePlayer", (transition) => PlayerIsAlive() && DistanceToPlayer() < ownScanningRange);
        fsm.AddTransition("ChasePlayer", "Attack", (transition) => PlayerIsAlive() && DistanceToPlayer() < ownAttackRange);
        fsm.AddTransition("Idle", "Attack", (transition) => PlayerIsAlive() && DistanceToPlayer() < ownAttackRange);
        fsm.AddTransition("Attack", "Idle");
        fsm.AddTransitionFromAny("Guard", (transition) => !PlayerIsAlive() || DistanceToPlayer() > ownScanningRange);
        fsm.AddTransition("Guard", "Idle", (transition) => IsAtGuardPosition());
        fsm.Init();
    }


    void Update()
    {
        fsm.OnLogic();
    }
    private bool PlayerIsAlive()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return false;
        if (player.GetComponent<HealthBehaviour>().IsDead()) return false;
        return true;
    }

    private bool IsAtGuardPosition()
    {
        return Vector3.Distance(transform.position, guardPosition) < stoppingDistance + 0.5f;
    }

    float DistanceToPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        return Vector3.Distance(transform.position, player.transform.position);
    }
}
