using Core.EventDispatcher;
using Entity;
using TMPro;
using UI;
using UnityEngine;
using EventType = Core.EventDispatcher.EventType;

namespace Player {
    public class PlayerHealth : EntityBase
    {
        protected override void Start() {
            base.Start();
            this.SendMessage(EventType.OnBarUIChange, new BarMessage() {type = BarUI.BarType.Health, value = 1});
        }

        public override void TakeDamage(float amount) {
            base.TakeDamage(amount);
            this.SendMessage(EventType.OnPlayerTakeDamage);
            this.SendMessage(EventType.OnBarUIChange, new BarMessage() {type = BarUI.BarType.Health, value = currentHP / hp});
        }

        protected override void Death() {
            
        }
    }
}


