using System;
using Core.EventDispatcher;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Weapons;
using EventType = Core.EventDispatcher.EventType;

namespace UI {
    public class WeaponStatusUI : MonoBehaviour {
        public TextMeshProUGUI weaponText;
        private Sequence _currSequence;

        private void Start() {
            weaponText.color -= new Color(0, 0, 0, 1);
            this.SubscribeListener(EventType.OnWeaponChange, slot => {
                _currSequence?.Kill();
                weaponText.color -= new Color(0, 0, 0, 1);
                var converted = (WeaponBase.Slot)slot;

                switch (converted) {
                    case WeaponBase.Slot.Primary:
                        weaponText.SetText("-- SHOTGUN --");
                        break;
                    case WeaponBase.Slot.Secondary:
                        weaponText.SetText("-- REVIVE MOLOTOV --");
                        break;
                    case WeaponBase.Slot.Unarmed:
                        weaponText.SetText("-- UNARMED --");
                        break;
                }

                _currSequence = DOTween.Sequence();
                _currSequence
                    .Append(weaponText.DOFade(1, 0.5f))
                    .AppendInterval(1.5f)
                    .Append(weaponText.DOFade(0, 1f));
            });
        }
    }
}
