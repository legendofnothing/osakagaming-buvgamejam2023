using Core.EventDispatcher;
using Entity;
using System.Collections;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.AI;
using EventType = Core.EventDispatcher.EventType;

namespace Player {
    public class PlayerHealth : EntityBase
    {
        public Animator animator;
        private bool _isAlive = true;

        protected override void Start()
        {
            base.Start();
            animator.SetBool("IsAlive", true);
            this.SendMessage(EventType.OnBarUIChange, new BarMessage() {type = BarUI.BarType.Health, value = 1});
        }
            
            
        public override void TakeDamage(float amount) {
            animator.SetTrigger("Hurt");
            base.TakeDamage(amount);
            this.SendMessage(EventType.OnPlayerTakeDamage);
            this.SendMessage(EventType.OnBarUIChange, new BarMessage() {type = BarUI.BarType.Health, value = currentHP / hp});
            StartCoroutine(Recover());
        }

        protected override IEnumerator DelayDeath() {
            
            _isAlive = false;
            animator.SetBool("IsAlive", false);
            animator.SetTrigger("dead");
            GetComponent<BoxCollider2D>().enabled = false;
            this.SendMessage(EventType.OnPlayerDeath);
            yield return new WaitForSeconds(1.8f);
            animator.ResetTrigger("dead");
            this.SendMessage(EventType.OnEndGame);
            gameObject.SetActive(false);
        }
    }
}


