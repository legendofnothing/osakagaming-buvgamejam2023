using Core.EventDispatcher;
using Entity;
using System.Collections;
using TMPro;
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

        }

        public override void TakeDamage(float amount) {
            animator.SetTrigger("Hurt");
            base.TakeDamage(amount);
            this.SendMessage(EventType.OnPlayerTakeDamage);

            StartCoroutine(Recover());
        }

        protected override IEnumerator DelayDeath() {
            
            _isAlive = false;
            animator.SetBool("IsAlive", false);
            GetComponent<BoxCollider2D>().enabled = false;
            this.SendMessage(EventType.OnPlayerDeath);
            yield return new WaitForSeconds(2.65f);
            gameObject.SetActive(false);

        }
    }
}


