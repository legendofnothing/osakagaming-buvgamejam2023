using System;
using Entity;
using UnityEngine;

namespace Enemy {
    public class EnemyBase : EntityBase {
        [Header("Enemy Config")]
        public float damage;
        public float speed = 2f;
        public float stoppingDistance = 8f;
        public float detectionRadius = 5f;

        [Header("Refs")] 
        public EntityMoveTo moveTo;

        protected override void Start() {
            base.Start();
            moveTo.speed = speed;
            moveTo.stoppingDistance = stoppingDistance;
        }

        private void Update() {
            moveTo.SetTarget(Base.Base.instance.transform.position);
        }
    }
}
