using Entity;
using TMPro;
using UnityEngine;

namespace Player {
    public class PlayerHealth : EntityBase
    {
        public override void TakeDamage(float amount) {
            base.TakeDamage(amount);
        }

        protected override void Death() {
            
        }
    }
}


