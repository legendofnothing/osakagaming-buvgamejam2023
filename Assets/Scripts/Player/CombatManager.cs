using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.EventDispatcher;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using Weapons;
using EventType = Core.EventDispatcher.EventType;

namespace Player {
    public class CombatManager : Singleton<CombatManager> {
        public List<WeaponBase> weapons;
        public LayerMask uiLayer;

        private Rigidbody2D _rb;
        private WeaponBase _currentWeapon;

        public Animator animator;
        [ReadOnly] public float damageModifier = 1;
        [ReadOnly] public float speedModifier = 1;

        private Tween _currentKnockbackTween;

        private void Awake() {
            foreach (var weapon in weapons) {
                weapon.gameObject.SetActive(true);
            }
        }

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
                if (IsOverUI()) return;
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

        public void SwitchWeapon(WeaponBase.Slot targetSlot) {
            if (_currentWeapon.slot == targetSlot) return;
            var nextWeapon = weapons.Find(weapon => weapon.slot == targetSlot);
            if (!nextWeapon.CanSwitch()) return;
            nextWeapon.gameObject.SetActive(true);
            _currentWeapon.gameObject.SetActive(false);
            _currentWeapon = nextWeapon;
            this.SendMessage(EventType.OnWeaponChange, targetSlot);
            PlayerMovement.instance.currentSpeed = _currentWeapon.currentSpeed * speedModifier;
            PlayerMovement.instance.maxSpeed = _currentWeapon.maxSpeed;
        }

        public void Knockback(Vector3 dir, float force) {
            _rb.AddForce(dir * (force * Time.fixedDeltaTime), ForceMode2D.Impulse);
        }
        
        private bool IsOverUI() {
            //raycast from mouse pos to all UI elements 
            var results = new List<RaycastResult>();
            var eventData = new PointerEventData(EventSystem.current) {
                position = Input.mousePosition
            };
            EventSystem.current.RaycastAll(eventData, results);
            var hit = results.Find(result => CheckLayerMask.IsInLayerMask(result.gameObject, uiLayer));
            return hit.gameObject != null;
        }
    }
}
