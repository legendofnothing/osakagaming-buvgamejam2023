﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.EventDispatcher;
using DG.Tweening;
using Enemy;
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
        public SpriteData sprites;
        public SpriteRenderer body;
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
            animator.SetBool("IsAlive", true);
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = wanderSpeed;
            _currentState = SurvivorState.Wander;
            _defaultPosition = transform.position;
            body.sprite = sprites.sprites[Random.Range(0, sprites.sprites.Count)];
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
                        


                        if (Math.Abs(_agent.speed - wanderSpeed) > 0.1f) _agent.speed = wanderSpeed;

                        //bool IsMoving = Mathf.Abs(_agent.velocity.x) > 0.5 || Mathf.Abs(_agent.velocity.y) > 0.5 ? true: false ;
                        //animator.SetBool("IsMoving", IsMoving);

                        if (_agent.velocity == Vector3.zero) {
                            var position = _defaultPosition * Random.insideUnitCircle * 2f;
                            moveTo.SetDestination(position);
                            animator.SetBool("IsMoving", true);
                        }
                        break;
                }
            }
            else {
                var baseTarget = Physics2D.CircleCast(transform.position, enterBaseRadius, Vector2.zero, 0, baseLayer);
                if (baseTarget.collider != null) {
                    animator.SetBool("IsMoving", true);
                    moveTo.SetDestination(baseTarget.collider.gameObject);
                   
                    if (_agent.velocity == Vector3.zero) {
                        _hasEnterBase = true;
                        _agent.isStopped = true;
                        _currentTarget = baseTarget.collider.gameObject;
                        EnterBase();
                    }
                }
                
                else {
                    animator.SetBool("IsMoving", true);
                    //bool playerIsMoving = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) == Vector2.zero ? false: true;
                    //animator.SetBool("IsMoving", playerIsMoving);

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

        //protected override void Death()
        //{
        //    _currentTween?.Kill();
        //    Destroy(gameObject);
        //}

        protected override IEnumerator DelayDeath()
        {
            animator.SetTrigger("Hurt");
            animator.SetBool("IsAlive", false);
            
            _agent.isStopped = true;
            _currentTween?.Kill();
            GetComponent<BoxCollider2D>().enabled = false;            
            yield return new WaitForSeconds(3f);
            Destroy(gameObject);
            
        }
    }
}