using System;
using UnityEngine;

namespace Entity {
    public class EntityMoveTo : MonoBehaviour {
        [Header("Config")]
        public float stoppingDistance = 2f;
        public float speed = 8f;
        
        [Header("Readonly")]
        public bool canMove = true;
        public Vector2 target;

        public void SetTarget(Vector2 target) {
            this.target = target;
        }

        public void ResetTarget() => target = Vector2.zero;

        private void FixedUpdate() {
            if (target == Vector2.zero) return;
            
            if (canMove && Vector2.Distance(transform.position, target) > stoppingDistance) {
                transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime);
            }
        }
    }
}
