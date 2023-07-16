using System;
using Core.EventDispatcher;
using DG.Tweening;
using Player;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;
using EventType = Core.EventDispatcher.EventType;

namespace Weapons.Variants {
    public class ReviveMolotov : WeaponBase {
        [TitleGroup("ReviveMolotov Config")] 
        public GameObject molotovPrefab;
        public GameObject revivePuddle;
        public float delay;
        public int startingAmount;
        public float radius;

        [TitleGroup("Readonly")] 
        [ReadOnly] public int amount;

        private SpriteRenderer _spriteRenderer;
        private Vector2 _defaultPosition;
        private bool _canAttack = true;

        protected override void Start() {
            base.Start();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _defaultPosition = transform.localPosition;
            amount = startingAmount;
            this.SendMessage(EventType.OnTextUIChange, new TextMessage() {
                type = TextUI.TextType.MolotovCount,
                message = $"x{amount}"
            });
            this.SubscribeListener(EventType.OnMolotovAdded, _ => {
                amount++;
                this.SendMessage(EventType.OnTextUIChange, new TextMessage() {
                    type = TextUI.TextType.MolotovCount,
                    message = $"x{amount}"
                });
            });
        }

        public override void Attack() {
            if (!_canAttack) return;
            _canAttack = false;
            amount--;

            CombatManager.instance.animator.SetTrigger("Throw");
            this.SendMessage(EventType.OnTextUIChange, new TextMessage() {
                type = TextUI.TextType.MolotovCount,
                message = $"x{amount}"
            });

            DOVirtual.DelayedCall(0.3f, () =>
            {
                var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0;
                var molotovInst = Instantiate(molotovPrefab, transform.position, Quaternion.identity);
                _spriteRenderer.color -= new Color(0, 0, 0, 1);
                
                var s = DOTween.Sequence();
                s
                    .Append(molotovInst.transform.DOMove(mousePosition, Vector3.Distance(mousePosition, transform.position) / 2)
                        .SetEase(throwableEaseType))
                    .Append(molotovInst.GetComponent<SpriteRenderer>()
                        .DOFade(0, 0.3f)
                        .OnComplete(() => {
                            var puddleInst = Instantiate(revivePuddle, mousePosition, Quaternion.identity);
                            puddleInst.transform.localScale = Vector3.one * radius;
                        }));
            });
            
            DOVirtual.DelayedCall(delay, () =>
            {
                transform.localPosition = _defaultPosition;
                _spriteRenderer.DOFade(1, 0.8f);
                if (amount > 0)
                {
                    _canAttack = true;
                }
                else
                {
                    CombatManager.instance.SwitchWeapon(Slot.Primary);
                }
            });
        }

        public override bool CanSwitch() {
            return amount > 0;
        }
    }
}