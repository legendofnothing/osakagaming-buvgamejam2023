using System;
using System.Linq;
using Core;
using Core.EventDispatcher;
using DG.Tweening;
using Entity;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using EventType = Core.EventDispatcher.EventType;
using Random = UnityEngine.Random;

namespace Survivor {
    public class SurvivorBase : EntityBase {
        public enum SurvivorState {
            Wander,
            RunAway,
            RunToPlayer,
        }

        [TitleGroup("Targets")] 
        public LayerMask targetLayers;
        public LayerMask playerLayers;
        public LayerMask threatLayers;
        public LayerMask baseLayer;

        [TitleGroup("Enter Base Tween Config")]
        public float enterBaseDuration;
        public Ease enterBaseEaseType;

        [TitleGroup("Config")] 
        public float detectionRadius;
        public float enterBaseRadius;
        
        [TitleGroup("Speed Config")] 
        public float wanderSpeed;
        public float runAwaySpeed;
        public float runToPlayerSpeed;

        [TitleGroup("Refs")] 
        public EntityMoveTo moveTo;
        public Animator animator;
        
        private NavMeshAgent _agent;
        [ReadOnly] [SerializeField] private SurvivorState _currentState;
        private RaycastHit2D[] _targets;
        private GameObject _currentTarget;
        private Vector3 _defaultPosition;
        private bool _hasSwitched;
        private bool _hasEnterBase;
        private Tween _currentTween;

        protected override void Start() {
            base.Start();
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = wanderSpeed;
            _currentState = SurvivorState.Wander;
            _defaultPosition = transform.position;
        }

        private void Update() {
            if (_hasEnterBase) return;
            
            if (!_hasSwitched) {
                if (_currentState != SurvivorState.RunToPlayer) {
                    _targets = Physics2D.CircleCastAll(transform.position, detectionRadius, Vector2.zero, 0, targetLayers);

                    if (_targets.Length > 0) {
                        var sorted = from target in _targets.ToList()
                            orderby CheckLayerMask.IsInLayerMask(target.transform.gameObject, playerLayers) descending
                            select target;
                    
                        _currentState =
                            CheckLayerMask.IsInLayerMask(sorted.FirstOrDefault().transform.gameObject, playerLayers)
                                ? SurvivorState.RunToPlayer
                                : SurvivorState.RunAway;

                        _currentTarget = sorted.FirstOrDefault().transform.gameObject;
                    }
                    else {
                        _currentState = SurvivorState.Wander;
                    }   
                }

                switch (_currentState) {
                    case SurvivorState.RunAway:
                        animator.SetBool("IsMoving", true);

                        if (Math.Abs(_agent.speed - runAwaySpeed) > 0.1f) _agent.speed = runAwaySpeed;
                        
                        if (_agent.velocity == Vector3.zero) {
                            var position = _defaultPosition * Random.insideUnitCircle * 1.2f;
                            moveTo.SetDestination(position);
                        }
                        break;
                    case SurvivorState.RunToPlayer:                       
                        animator.SetBool("IsMoving", true);

                        if (Math.Abs(_agent.speed - runToPlayerSpeed) > 0.1f) _agent.speed = runToPlayerSpeed;
                        var movePos = Vector3.MoveTowards(_currentTarget.transform.position, transform.position, 0.5f);
                        moveTo.SetDestination(movePos);

                        if (_agent.remainingDistance < _agent.stoppingDistance && !_hasSwitched) {
                            _hasSwitched = true;
                            _currentTween = transform.DOScale(0.6f, 1f);
                            this.SendMessage(EventType.OnSurvivorAdded, this);
                        }
                        break;
                    default:
                        if (Math.Abs(_agent.speed - wanderSpeed) > 0.1f) _agent.speed = wanderSpeed; animator.SetBool("IsMoving", true);
                        

                        if (_agent.velocity == Vector3.zero) {
                            animator.SetBool("IsMoving", false);
                            var position = _defaultPosition * Random.insideUnitCircle * 2f;
                            moveTo.SetDestination(position);
                        }
                        break;
                }
            }
            else {
               var baseTarget = Physics2D.CircleCast(transform.position, enterBaseRadius, Vector2.zero, 0, baseLayer);
               if (baseTarget.collider != null) {
                    
                   moveTo.SetDestination(baseTarget.collider.gameObject);
                   
                   if (_agent.velocity == Vector3.zero) {
                       _hasEnterBase = true;
                       _agent.isStopped = true;
                       _currentTarget = baseTarget.collider.gameObject;
                       EnterBase();
                   }
               }
               else {
                   if (Math.Abs(_agent.speed - runToPlayerSpeed) > 0.1f) _agent.speed = runToPlayerSpeed;
                   var movePos = Vector3.MoveTowards(_currentTarget.transform.position, transform.position, 0.5f);
                   moveTo.SetDestination(movePos);
               }
            }
        }

        private void EnterBase() {
            canTakeDamage = false;
            transform
                .DOMove(_currentTarget.transform.position, enterBaseDuration)
                .SetEase(enterBaseEaseType)
                .OnComplete(() => {
                    this.SendMessage(EventType.OnSurvivorEnteredBase, this);
                    this.SendMessage(EventType.OnSurvivorDecreased, this);
                    Destroy(gameObject);
                });
        }

        protected override void Death() {
            _currentTween?.Kill();
            Destroy(gameObject);
        }
    }
}