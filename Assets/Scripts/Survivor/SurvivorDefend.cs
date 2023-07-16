using System;
using System.Collections;
using System.Linq;
using Bullet;
using Core;
using DG.Tweening;
using Player;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Survivor {
    public class SurvivorDefend : MonoBehaviour {
        [TitleGroup("Configs")] 
        public float damage;
        public SpriteRenderer bodyRenderer;
        public GameObject arm;
        public GameObject shell;
        public LayerMask enemyLayer;
        public float radius;
        public Transform shootPoint;
        public float spreadAngle;

        public Animator animator;

        private bool _canAttack = true;
        private GameObject _target;

        private void Start() {
            animator.SetBool("SeeEnemy", false);
            animator.SetBool("IsAlive", true);

        }

        private void Update() {

            

            if (!_canAttack) return;
            if (_target == null) {
                animator.SetBool("SeeEnemy", false);
                var targets 
                    = Physics2D.CircleCastAll(
                        transform.position, 
                        radius, 
                        Vector2.up, 
                        0, enemyLayer);

                if (targets.Length < 1) return;

                _target = targets
                    .ToList()
                    .OrderBy(x => Vector2.Distance(transform.position, x.transform.position))
                    .FirstOrDefault().transform.gameObject;
                TurnToTarget();
            }
            else {
                TurnToTarget();
                animator.SetBool("SeeEnemy", true);
                _canAttack = false;
                //Attack();
                StartCoroutine(Attack1());
            }
        }

        //private void Attack() {
        //    animator.SetTrigger("Shoot");
            
        //    var posDif = transform.position.x - _target.transform.position.x;
        //    bodyRenderer.flipX = posDif <= 1;

        //    var dir = _target.transform.position - arm.transform.position;
        //    dir.z = 0;
        //    var rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 180;
        //    arm.transform.rotation = Quaternion.Euler(0, 0, rot);

            

        //    for (var i = 0; i < 3; i++) {
        //        shootPoint.localEulerAngles =
        //            new Vector3(shootPoint.localEulerAngles.x, 
        //                shootPoint.localEulerAngles.y, 
        //                Random.Range(-spreadAngle/2f, spreadAngle/2f));
        //        var shellInst = Instantiate(shell, shootPoint.position, shootPoint.rotation * Quaternion.AngleAxis(180, Vector3.up));
        //        shellInst.GetComponent<BulletBehavior>().damage = damage;
        //    }
            
        //    StartCoroutine(AttackCooldown());
        //}

        //private IEnumerator AttackCooldown() {
        //    yield return new WaitForSeconds(5f);
            
        //    _canAttack = true;
        //}

        public IEnumerator Death()
        {
            //animator.SetTrigger("Death");
            animator.SetBool("IsAlive", false);
            animator.SetTrigger("death");
            yield return new WaitForSeconds(2.3f);
            Destroy(gameObject);
        }

        private IEnumerator Attack1()
        {
            //if(!_canAttack) { yield break; };

            animator.SetTrigger("Shoot");

            TurnToTarget();



            for (var i = 0; i < 3; i++)
            {
                shootPoint.localEulerAngles =
                    new Vector3(shootPoint.localEulerAngles.x,
                        shootPoint.localEulerAngles.y,
                        Random.Range(-spreadAngle / 2f, spreadAngle / 2f));
                var shellInst = Instantiate(shell, shootPoint.position, shootPoint.rotation * Quaternion.AngleAxis(180, Vector3.up));
                shellInst.GetComponent<BulletBehavior>().damage = damage * CombatManager.instance.damageModifier;
            }

            yield return new WaitForSeconds(5f / CombatManager.instance.speedModifier);

            _canAttack = true;
        }

        private void TurnToTarget()
        {
            var posDif = transform.position.x - _target.transform.position.x;
            bodyRenderer.flipX = posDif <= 1;

            var dir = _target.transform.position - arm.transform.position;
            dir.z = 0;
            var rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 180;
            arm.transform.rotation = Quaternion.Euler(0, 0, rot);

        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(this.transform.position, radius);
        }
    }
}