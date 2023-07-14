using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Weapons;

namespace Player {
    public class CombatManager : Singleton<CombatManager> {
        public List<WeaponBase> weapons;

        private Rigidbody2D _rb;
        private WeaponBase _currentWeapon;

        private Tween _currentKnockbackTween;

        private void Start() {
            _rb = GetComponent<Rigidbody2D>();
            
            foreach (var weapon in weapons) {
                if (weapon.slot == WeaponBase.Slot.Primary) {
                    _currentWeapon = weapon;
                    weapon.gameObject.SetActive(true);
                    PlayerMovement.instance.currentSpeed = _currentWeapon.speed;
                    PlayerMovement.instance.maxSpeed = _currentWeapon.maxSpeed;
                } else weapon.gameObject.SetActive(false);
            }
        }

        private void Update() {
            if (Input.GetMouseButton(0)) {
                _currentWeapon.Attack();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                SwitchWeapon(WeaponBase.Slot.Primary);
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                SwitchWeapon(WeaponBase.Slot.Secondary);
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                SwitchWeapon(WeaponBase.Slot.Unarmed);
            }
        }

        private void SwitchWeapon(WeaponBase.Slot targetSlot) {
            if (_currentWeapon.slot == targetSlot) return;
            var nextWeapon = weapons.Find(weapon => weapon.slot == targetSlot);
            if (!nextWeapon.CanSwitch()) return;
            nextWeapon.gameObject.SetActive(true);
            _currentWeapon.gameObject.SetActive(false);
            _currentWeapon = nextWeapon;
            PlayerMovement.instance.currentSpeed = _currentWeapon.speed;
            PlayerMovement.instance.maxSpeed = _currentWeapon.maxSpeed;
        }

        public void Knockback(Vector3 dir, float force) {
            _rb.AddForce(dir * (force * Time.fixedDeltaTime), ForceMode2D.Impulse);
        }
    }
}
