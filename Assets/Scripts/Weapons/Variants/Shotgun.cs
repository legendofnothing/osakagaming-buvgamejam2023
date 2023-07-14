using System;
using Bullet;
using DG.Tweening;
using Entity;
using Player;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Weapons.Variants {
    public class Shotgun : WeaponBase {
        [TitleGroup("Shotgun Config")] 
        public GameObject shell;
        public Transform shootPoint;
        public GameObject playerHands;
        [Space] 
        public float reloadTime;
        public float knockbackForce;
        [Space]
        public float amountOfShots = 3f;
        public float spreadAngle = 20f;
        public float shotLength = 2f;

        private bool _canAttack = true;
        
        public override void Attack() {
            if (!_canAttack) return;

            _canAttack = false;
            CombatManager.instance.Knockback(playerHands.transform.right, knockbackForce);

            for (var i = 0; i < amountOfShots; i++) {
                shootPoint.localEulerAngles =
                    new Vector3(shootPoint.localEulerAngles.x, 
                        shootPoint.localEulerAngles.y, 
                        Random.Range(-spreadAngle/2f, spreadAngle/2f));
                var shellInst = Instantiate(shell, shootPoint.position, shootPoint.rotation * Quaternion.AngleAxis(180, Vector3.up));
                shellInst.GetComponent<BulletBehavior>().damage = damage;
            }
            
            DOVirtual.DelayedCall(reloadTime, () => _canAttack = true);
        }
    }
}