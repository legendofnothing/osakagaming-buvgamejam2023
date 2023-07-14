using System;
using System.Collections;
using System.Linq;
using Core;
using Entity;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Enemy {
    public class EnemyBase : EntityBase {
        [Header("Enemy Config")]
        public float damage;
        public float detectionRadius = 5f;
        public float attackDelay = 1.2f;

        [Header("Target")] 
        public LayerMask targets;
        public LayerMask lowPriorityTargets;
        public LayerMask highPriorityTargets;

        [Header("Refs")] 
        public EntityMoveTo moveTo;

        private GameObject _currentTarget;
        private NavMeshAgent _agent;
        private bool _canSwitchState = true;
        private bool _canAttack = true;
        private RaycastHit2D[] _targets;

        protected override void Start() {
            base.Start();
            _agent = GetComponent<NavMeshAgent>();
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
                moveTo.SetDestination(_currentTarget);

                if (_agent.velocity == Vector3.zero && _canAttack) {
                    _canAttack = false;
                    //Do animation based attack
                }
            }
        }
    }
}
