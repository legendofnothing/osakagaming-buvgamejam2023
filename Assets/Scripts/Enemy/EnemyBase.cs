using System;
using System.Collections;
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

namespace Enemy {
    public class EnemyBase : EntityBase {
        [TitleGroup("Enemy Config")] 
        public GameObject survivorPrefab;
        [Space]
        public float damage;
        public float detectionRadius = 5f;
        public float attackRadius = 1.2f;
        public float attackDelay = 1.2f;

        [TitleGroup("Target")] 
        public LayerMask targets;
        public LayerMask lowPriorityTargets;
        public LayerMask highPriorityTargets;

        [TitleGroup("Refs")] 
        public EntityMoveTo moveTo;
        public SpriteRenderer body;

        private GameObject _currentTarget;
        private NavMeshAgent _agent;
        private bool _canSwitchState = true;
        private bool _canAttack = true;
        private bool _hasChanged;
        private RaycastHit2D[] _targets;
        private float _defaultSpeed;

        private Tween _currSlowdownTween;

        protected override void Start() {
            base.Start();
            _agent = GetComponent<NavMeshAgent>();
            _defaultSpeed = _agent.speed;
        }
        
        private void Update() {
            if (_canSwitchState) {
                _targets = Physics2D.CircleCastAll(transform.position, detectionRadius, Vector2.zero, 0, targets);

                if (_targets.Length > 0) {
                    var sorted = from target in _targets.ToList()
                        orderby CheckLayerMask.IsInLayerMask(target.transform.gameObject, highPriorityTargets) descending
                        select target;
                    _currentTarget = sorted.FirstOrDefault().transform.gameObject;
                }
                else {
                    _currentTarget = Base.Base.instance.transform.gameObject;
                }   
            }
            
            if (_currentTarget != null) {
                FaceTarget(body, _currentTarget);
                moveTo.SetDestination(_currentTarget);

                if (Math.Abs(_agent.remainingDistance - _agent.stoppingDistance) < 0.2f && _canAttack) {
                    _canAttack = false;
                    Attack();
                }
            }
        }

        private void Attack() {
            var layerMaskToDetect =
                CheckLayerMask.IsInLayerMask(_currentTarget.gameObject, highPriorityTargets)
                    ? highPriorityTargets
                    : lowPriorityTargets;
            var targets  = Physics2D.CircleCastAll(transform.position, attackRadius, Vector2.zero, 0, layerMaskToDetect);
            var target =targets
                .ToList()
                .OrderBy(x => Vector2.Distance(transform.position, x.transform.position))
                .FirstOrDefault();

            if (target.collider != null) {
                if (target.transform.gameObject.TryGetComponent<EntityBase>(out var entity)) {
                    entity.TakeDamage(damage);
                }
                else if (target.transform.gameObject.TryGetComponent<Base.Base>(out var b)) {
                    b.TakeDamage(damage);
                }
            }

            DOVirtual.DelayedCall(attackDelay, () => _canAttack = true);
        }

        public override void TakeDamage(float amount) {
            base.TakeDamage(amount);
            
            if (currentHP <= 0) return;
            _currSlowdownTween?.Kill();
            _agent.speed *= 2f / 3f;
            _currSlowdownTween = DOVirtual.Float(_agent.speed, _defaultSpeed, 2.4f, value => {
                _agent.speed = value;
            });
        }

        protected override void Death() {
            _currSlowdownTween?.Kill();
            this.SendMessage(EventType.OnEnemyDie, this);
            Destroy(gameObject);
        }

        public void Convert() {
            if (_hasChanged) return;
            _hasChanged = true;
            _canSwitchState = false;
            _canAttack = false;
            canTakeDamage = false;
            var s = DOTween.Sequence();
            s
                .Append(DOVirtual.Float(_agent.speed, 0, 2f, value => {
                    _agent.speed = value;
                }).OnComplete(() => {
                    _agent.enabled = false;
                    _currentTarget = null;
                }))
                .AppendInterval(0.8f)
                .Append(transform.DOShakePosition(4f, 0.5f, 10, 90, false, false))
                .Append(transform.DOScale(0, 1.4f).SetEase(Ease.InElastic))
                .OnComplete(() => {
                    Instantiate(survivorPrefab, transform.position, Quaternion.identity);
                    this.SendMessage(EventType.OnEnemyDie, this);
                    Destroy(gameObject);
                });
        }
    }
}
