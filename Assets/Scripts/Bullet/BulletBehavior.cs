using System;
using Core;
using Entity;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Bullet {
    public class BulletBehavior : MonoBehaviour {
        [TitleGroup("Config")] 
        [ReadOnly] public float damage;
        public LayerMask enemyLayer;
        public float _bulletSpeed;
        
        private void Start() {
            Destroy(gameObject, 3f);
        }

        void Update() {
            transform.Translate(Vector3.left * (Time.fixedDeltaTime * _bulletSpeed), Space.Self);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (CheckLayerMask.IsInLayerMask(other.gameObject, enemyLayer) && other.gameObject.TryGetComponent<EntityBase>(out var entity)) {
                entity.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
