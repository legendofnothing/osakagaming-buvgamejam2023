using Sirenix.OdinInspector;
using UnityEngine;

namespace Entity {
    public class EntityBase : MonoBehaviour {
        [Header("Entity Config")]
        public float hp;
        [ReadOnly] public float currentHP;
        [ReadOnly] public bool canTakeDamage = true;

        protected virtual void Start() {
            currentHP = hp;
        }
    
        public virtual void TakeDamage(float amount) {
            if (!canTakeDamage) return;
            currentHP -= amount;
            if (currentHP <= 0) {
                Death();
            }
        }

        public virtual void FaceTarget(SpriteRenderer body, GameObject target)
        {
            float posDif = body.transform.position.x - target.transform.position.x;
            body.flipX = posDif >= 1 ? true : false;
        }

        protected virtual void Death() {
            
        }
    }
}
    