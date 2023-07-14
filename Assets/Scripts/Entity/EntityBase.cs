using Sirenix.OdinInspector;
using UnityEngine;

namespace Entity {
    public class EntityBase : MonoBehaviour {
        [Header("Entity Config")]
        public float hp;
        [ReadOnly] public float currentHP;

        protected virtual void Start() {
            currentHP = hp;
        }
    
        public virtual void TakeDamage(float amount) {
            currentHP -= amount;
            if (currentHP <= 0) {
                Death();
            }
        }

        protected virtual void Death() {
            Destroy(gameObject);
        }
    }
}
    