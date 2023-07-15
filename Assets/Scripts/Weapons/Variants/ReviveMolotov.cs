using System;
using DG.Tweening;
using Player;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Weapons.Variants {
    public class ReviveMolotov : WeaponBase {
        [TitleGroup("ReviveMolotov Config")] 
        public GameObject revivePuddle;
        public float delay;
        public int startingAmount;
        public float radius;

        [TitleGroup("Readonly")] 
        [ReadOnly] public int amount;

        private SpriteRenderer _spriteRenderer;
        private Vector2 _defaultPosition;
        private bool _canAttack = true;

        private void Start() {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _defaultPosition = transform.localPosition;
            amount = startingAmount;
        }

        public override void Attack() {
            if (!_canAttack) return;
            _canAttack = false;
            amount--;
            
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            var s = DOTween.Sequence();
            s
                .Append(transform.DOMove(mousePosition, Vector3.Distance(mousePosition, transform.position) / 2)
                    .SetEase(throwableEaseType).OnComplete(() => {
                        var puddleInst = Instantiate(revivePuddle, mousePosition, Quaternion.identity);
                        puddleInst.transform.localScale = Vector3.one * radius;
                    }))
                .Append(_spriteRenderer.DOFade(0, 0.8f))
                .Append(DOVirtual.DelayedCall(delay, () => {
                    transform.localPosition = _defaultPosition;
                    _spriteRenderer.DOFade(1, 0.8f);
                    if (amount > 0) {
                        _canAttack = true;
                    }
                    else {
                        CombatManager.instance.SwitchWeapon(Slot.Primary);
                    }
                }));
        }

        public override bool CanSwitch() {
            return amount > 0;
        }
    }
}