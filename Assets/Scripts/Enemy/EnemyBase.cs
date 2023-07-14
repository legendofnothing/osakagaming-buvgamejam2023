using System;
using Entity;
using UnityEngine;

namespace Enemy {
    public class EnemyBase : EntityBase {
        [Header("Enemy Config")]
        public float damage;
        public float detectionRadius = 5f;

        [Header("Refs")] 
        public EntityMoveTo moveTo;

        protected override void Start() {
            base.Start();
        }

        private void Update() {
            moveTo.SetDestination(Base.Base.instance.gameObject);
        }
    }
}
