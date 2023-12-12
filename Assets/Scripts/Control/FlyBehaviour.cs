using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using UnityEngine.AI;
using RPG.Combat;
using RPG.Core;
using RPG.Saving;

namespace RPG.Control
{
    public class FlyBehaviour : MonoBehaviour, ISaveable
    {
        [SerializeField] float playerScanningRange = 4f;
        [SerializeField] float ownScanningRange = 6f;
        [SerializeField] int rageSpeed = 5;
        [SerializeField] int minRageMoves = 1;
        [SerializeField] int maxRageMoves = 5;
        [SerializeField] float rageMoveRange = 4f;
        [Tooltip("When max number is reached, Spy will enter Rage state")]
        [SerializeField] float maxAttackTime = 5;

        private Vector3 guardPosition;
        private StateMachine fsm;
        private NavMeshAgent agent;
        private Fighter fighter;
        private float defaultAgentSpeed;
        private List<Vector3> rageRandomPositions = new();
        private float attackTimeSinceLastRage = 0;
        private bool isRageTriggered = false;
        Animator animator;

        void Start()
        {
            guardPosition = transform.position;
            fsm = new StateMachine();
            agent = GetComponent<NavMeshAgent>();
            fighter = GetComponent<Fighter>();
            defaultAgentSpeed = agent.speed;
            animator = GetComponentInChildren<Animator>();

            fsm.AddState("Attack",
                onEnter: (state) =>
                {
                    agent.isStopped = true;
                    animator.SetBool("isFlying", false);
                },
                onLogic: (state) =>
                {
                    fighter.Attack();
                    attackTimeSinceLastRage += Time.deltaTime;
                },
                onExit: (state) =>
                {
                    attackTimeSinceLastRage = 0;
                }
                );

            fsm.AddState("Rage",
                onEnter: (state) =>
                {
                    isRageTriggered = false;
                    animator.SetBool("isFlying", true);
                    int movesLeft = GenerateRandomNumberOfMoves(minRageMoves, maxRageMoves);
                    for (int i = 0; i < movesLeft; i++)
                    {
                        rageRandomPositions.Add(GetRandomPositionInRange(rageMoveRange));
                    }
                },
                onLogic: (state) =>
                {
                    agent.isStopped = false;
                    agent.speed = rageSpeed;
                    float stoppingDistance = 0.1f;

                    agent.SetDestination(rageRandomPositions[0]);
                    if (Vector3.Distance(transform.position, rageRandomPositions[0]) < stoppingDistance)
                    {
                        rageRandomPositions.RemoveAt(0);
                    }

                    if (rageRandomPositions.Count == 0)
                    {
                        state.fsm.StateCanExit();
                    }
                },
                onExit: (State) =>
                {
                    agent.speed = defaultAgentSpeed;
                }
                , needsExitTime: true
                );
            fsm.AddState("FollowPlayer",
                onEnter: (state) =>{
                    animator.SetBool("isFlying", true);
                },
                onLogic: (state) =>
                {
                    agent.isStopped = false;
                    agent.SetDestination(this.transform.position + DirectionToPlayer());
                }
                );
            fsm.AddState("FleeFromPlayer",
                onEnter: (state) => {
                    animator.SetBool("isFlying", true);
                },
                onLogic: (state) =>
                {
                    agent.isStopped = false;
                    agent.SetDestination(this.transform.position + -DirectionToPlayer());
                }
                );
            fsm.AddState("Guard",
                onEnter: (state) => {
                    animator.SetBool("isFlying", true);
                },
                onLogic: (state) =>
                {
                    agent.isStopped = false;
                    agent.SetDestination(guardPosition);
                }
                );
            fsm.AddState("Idle",
                onEnter: (state) => {
                    animator.SetBool("isFlying", false);
                },
                onLogic: (state) =>
                {
                    agent.isStopped = true;
                });
            fsm.SetStartState("Guard");
            fsm.AddTransition("Rage", "Attack", (transition) => PlayerIsAlive() && DistanceToPlayer() < ownScanningRange);
            fsm.AddTransition("Rage", "FollowPlayer", (transition) => PlayerIsAlive() && DistanceToPlayer() > ownScanningRange);
            fsm.AddTransition("Attack", "Rage", (transition) => attackTimeSinceLastRage >= maxAttackTime);
            fsm.AddTransition("Attack", "Guard", (transition) => PlayerIsAlive() && DistanceToPlayer() > ownScanningRange);
            fsm.AddTransition("FollowPlayer", "Attack", (transition) => PlayerIsAlive() && DistanceToPlayer() < ownScanningRange);
            fsm.AddTransition("FollowPlayer", "Guard", (transition) => PlayerIsAlive() && DistanceToPlayer() > ownScanningRange);
            fsm.AddTransition("Guard", "Idle", (transition) => PlayerIsAlive() && IsAtGuardPosition());
            fsm.AddTransition("Guard", "Attack", (transition) => PlayerIsAlive() && IsInAttackRange());
            fsm.AddTransition("Idle", "Attack", (transition) => PlayerIsAlive() && IsInAttackRange());
            fsm.AddTransition("Attack", "FleeFromPlayer", (transition) => PlayerIsAlive() && DistanceToPlayer() < playerScanningRange);
            fsm.AddTransition("Guard", "FleeFromPlayer", (transition) => PlayerIsAlive() && DistanceToPlayer() < playerScanningRange);
            fsm.AddTransition("Idle", "FleeFromPlayer", (transition) => PlayerIsAlive() && DistanceToPlayer() < playerScanningRange);
            fsm.AddTransition("FleeFromPlayer", "Attack", (transition) => PlayerIsAlive() && DistanceToPlayer() > playerScanningRange);
            fsm.AddTransitionFromAny("Rage", (transition) => isRageTriggered);
            fsm.Init();
        }

        public void TriggerRage()
        {
            isRageTriggered = true;
        }

        private bool IsInAttackRange()
        {
            return (DistanceToPlayer() < ownScanningRange) && (DistanceToPlayer() > playerScanningRange);
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

        Vector3 DirectionToPlayer()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            return player.transform.position - transform.position;
        }
        private bool PlayerIsAlive()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return false;
            if (player.GetComponent<HealthBehaviour>().IsDead()) return false;
            return true;
        }

        bool IsAtGuardPosition()
        {
            return Vector3.Distance(guardPosition, this.transform.position) < 0.1f;
        }

        private int GenerateRandomNumberOfMoves(int minMoves, int maxMoves)
        {
            if (maxMoves < 1 || maxMoves < minMoves)
            {
                Debug.LogError("Number of moves is invalid.");
                return 1;
            }

            return Random.Range(minMoves, maxMoves + 1);
        }

        Vector3 GetRandomPositionInRange(float range)
        {
            Vector3 point;
            if (RandomPointOnNavMesh(transform.position, range, out point))
            {
                return point;
            }
            return Vector3.zero;
        }

        bool RandomPointOnNavMesh(Vector3 center, float range, out Vector3 result)
        {
            for (int i = 0; i < 30; i++)
            {
                Vector3 randomPoint = center + Random.insideUnitSphere * range;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
                {
                    result = hit.position;
                    return true;
                }
            }
            result = Vector3.zero;
            return false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, playerScanningRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(this.transform.position, ownScanningRange);
            Gizmos.color= Color.green;
            Gizmos.DrawWireSphere(guardPosition, 0.5f);
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3) state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = position.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}