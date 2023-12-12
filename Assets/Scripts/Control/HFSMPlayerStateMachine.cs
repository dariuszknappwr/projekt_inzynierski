using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;

namespace RPG.Control
{
    public class HFSMPlayerStateMachine : MonoBehaviour
    {
        [SerializeField] AudioSource attackMeleSound;
        [SerializeField] AudioSource runningSound;
        [SerializeField] AudioSource BubbleGunSound;
        [SerializeField] AudioSource JumpSound;
        [SerializeField] AudioSource LandingSound;

        [HideInInspector] public bool isAttackPressed;
        [SerializeField] LayerMask enemyLayer;
        [SerializeField] Vector3 enemyCheckHalfExtends;
        [Tooltip("Time that player doesnt check for gravity after jump." +
            "Used to avoid being grounded first frame after jump")]
        [SerializeField] float minJumpDuration = 0.1f;
        [SerializeField] LayerMask ignoreGravityCollisionLayers;
        [SerializeField] float knockbackTime = 0.5f;
        [SerializeField] float knockbackStunTime = 0.5f;
        [SerializeField] float knockbackForce = 5f;

        private StateMachine movement;
        private StateMachine airborne;
        private StateMachine combat;
        private StateMachine status;
        private Animator animator;
        private Fighter fighter;
        
        private PlayerMover mover;
        private CharacterController characterController;
        private bool isTouchingEnemy = false;
        private ControllerColliderHit characterColliderHitInfo;
        private bool isJumpDisabled = false;
        private bool isAttackDisabled = false;
        private bool isMovementDisabled = false;

        void Start()
        {
            animator = GetComponent<Animator>();
            fighter = GetComponent<Fighter>();
            mover = GetComponent<PlayerMover>();
            characterController = GetComponent<CharacterController>();

            #region StatusStateMachine
            status = new StateMachine(needsExitTime: true);
            status.AddState("Neutral", 
                onEnter: (state) =>
                {
                    isMovementDisabled = false;
                    isJumpDisabled = false;
                    isAttackDisabled = false;
                    isTouchingEnemy = false;
                }
                );
            status.AddState("Knockback",
                onEnter: (state) =>
                {
                    isJumpDisabled = true;
                    isAttackDisabled = true;
                    isMovementDisabled = true;
                },
                onLogic: (state) =>
                {
                    if (state.timer.Elapsed > knockbackTime)
                        state.fsm.RequestStateChange("Stun", forceInstantly: true);
                    mover.SetGravityForce(0);
                    Vector2 directionAwayFromEnemy = GetDirectionAwayFromEnemy();
                    mover.Move(directionAwayFromEnemy.normalized, knockbackForce);
                },
                needsExitTime: true
                );
            status.AddState("Stun",
                onEnter: (state) =>
                {
                    animator.SetBool("IsRunning", false);
                },
                onLogic: (state) =>
                {
                    if (state.timer.Elapsed > knockbackStunTime)
                        state.fsm.StateCanExit();
                },
                needsExitTime: true
                );
            status.SetStartState("Neutral");
            status.AddTransition("Neutral", "Knockback", (transition) => isTouchingEnemy);
            status.AddTransition("Knockback", "Stun", (transition) => fighter);
            status.AddTransition("Stun", "Neutral");
            status.Init();
            #endregion
            #region AirborneStateMachine
            airborne = new StateMachine(needsExitTime: false);

            airborne.AddState("Fall",
                onLogic: (state) =>
                {
                    mover.AddGravityForce();
                    animator.SetBool("IsFalling", true);
                    
                    
                });
            airborne.AddState("Grounded",
                onEnter: (state) =>
                {
                    //LandingSound.Play();
                    GameObject stompedEnemy = GetEnemyOnWhichStomped();
                    if (stompedEnemy != null)
                    {
                        fighter.InvokeOnStomp(gameObject, stompedEnemy);
                    }
                    animator.SetBool("IsGrounded", true);
                    animator.SetBool("IsJumping", false);
                    animator.SetBool("IsFalling", false);
                },
                onLogic: (state) =>
                {
                    mover.SetGroundedGravity();
                }
                );
            airborne.AddState("Jump",
                onEnter: (state) =>
                {
                    JumpSound.Play();
                    mover.Jump();
                    animator.SetBool("IsGrounded", false);
                    animator.SetBool("IsJumping",true);
                    
                },
                onLogic: (state) =>
                {
                    mover.AddGravityForce();
                    if (state.timer.Elapsed > minJumpDuration)
                    {
                        state.fsm.StateCanExit();
                    }
                },
                needsExitTime: true
                );

            airborne.SetStartState("Grounded");
            airborne.AddTransition("Fall", "Grounded", (transition) => GroundCheck());
            airborne.AddTransition("Grounded", "Fall", (transition) => !GroundCheck());
            airborne.AddTransition("Grounded", "Jump", (transition) => mover.CanJump() && !isJumpDisabled);
            airborne.AddTransition("Jump", "Fall");
            airborne.Init();
            #endregion
            #region CombatStateMachine
            combat = new StateMachine(needsExitTime: false);

            combat.AddState("Attack",
                onEnter: (state) =>
                {
                    runningSound.Stop();
                    fighter.Attack();
                    Debug.Log(fighter.CurrentWeapon.GetRealWeaponName());
                    if (fighter.CurrentWeapon.GetRealWeaponName() == "Bubble gun")
                    {
                        BubbleGunSound.Play();
                        animator.SetBool("IsAttacking", false);
                        animator.SetBool("IsAttackRange", false);

                    }
                    else if(fighter.CurrentWeapon.GetRealWeaponName() == "Bow") {
                        animator.SetBool("IsAttackRange", true);
                        animator.SetBool("IsAttacking", false);
                        
                        
                    }else if(fighter.CurrentWeapon.GetRealWeaponName() == "Crossbow"){
                        animator.SetBool("IsAttacking", false);
                        
                    }else{
                        attackMeleSound.Play();

                    }
                    
                },
                onExit: (state) =>
                {
                    animator.SetBool("IsAttacking", false);
                    animator.SetBool("IsAttackRange", false);
                }
                );
            combat.AddState("Unarmed");

            combat.AddTransition("Unarmed", "Attack", (transition) => isAttackPressed && !isAttackDisabled);
            combat.AddTransition("Attack", "Unarmed", (transition) => fighter.TimeSinceLastAttack >= fighter.TimeBetweenAttacks);
            combat.AddTransitionFromAny("Unarmed", (transition) => isTouchingEnemy);
            combat.SetStartState("Unarmed");
            combat.Init();
            #endregion
            #region MovementStateMachine
            movement = new StateMachine();

            movement.AddState("Idle",
                onEnter: (state) =>
                {
                    animator.SetBool("IsRunning", false);
                    runningSound.Stop();
                }
                );

            movement.AddState("Run",
                onEnter: (state) =>
                {
                    animator.SetBool("IsRunning", true);
                    runningSound.Play();
                },
                onLogic: (state) =>
                {
                    mover.Move();
                   
                }
                );

            movement.SetStartState("Idle");
            movement.AddTransition("Idle", "Run", (transition) => mover.CanMove() && !isMovementDisabled);
            movement.AddTransition("Run", "Idle", (transition) => !mover.CanMove() && !isMovementDisabled);
            movement.AddTransitionFromAny("Idle", (transition) => isMovementDisabled);
            movement.Init();
            #endregion

        }

        void Update()
        {
            status.OnLogic();
            movement.OnLogic();
            airborne.OnLogic();
            combat.OnLogic();

        }

        private bool GroundCheck()
        {
            return characterController.isGrounded;
        }

        private GameObject GetEnemyOnWhichStomped()
        {
            Ray ray = new Ray(transform.position, -transform.up);
            Collider col = CollisionChecker.GetCollisionContacts(ray, enemyCheckHalfExtends, transform.rotation, enemyLayer);
            if (col != null)
            {
                return col.gameObject;
            }
            return null;
        }
        private Vector2 GetDirectionAwayFromEnemy()
        {
            Vector3 enemyPosition = characterColliderHitInfo.collider.gameObject.transform.position;
            Vector2 directionAwayFromEnemy = new Vector2(transform.position.x, transform.position.z) - new Vector2(enemyPosition.x, enemyPosition.z);
            if (directionAwayFromEnemy == Vector2.zero)
                directionAwayFromEnemy = Vector2.right;
            return directionAwayFromEnemy;
        }

        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (fighter.CurrentWeapon.DisableDamageOnTouchingEnemy) return;
            if (Fighter.IsMatchingTargetLayers(hit.collider.gameObject, enemyLayer))
            {
                isTouchingEnemy = true;
                characterColliderHitInfo = hit;
            }
        }
    }

}