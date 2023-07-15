using Core.EventDispatcher;
using Entity;
using TMPro;
using UnityEngine;
using EventType = Core.EventDispatcher.EventType;

namespace Player {
    public class PlayerHealth : EntityBase
    {
        public override void TakeDamage(float amount) {
            base.TakeDamage(amount);
            this.SendMessage(EventType.OnPlayerTakeDamage);
        }

        protected override void Death() {
            
        }
    }
}


