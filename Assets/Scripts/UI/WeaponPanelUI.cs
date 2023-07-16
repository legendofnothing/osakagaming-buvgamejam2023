using System;
using System.Collections.Generic;
using Core.EventDispatcher;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Weapons;
using EventType = Core.EventDispatcher.EventType;

namespace UI {
    public class WeaponPanelUI : MonoBehaviour {
        [Serializable]
        public struct WeaponPanel {
            public WeaponBase.Slot slot;
            public Image panel;
        }

        public List<WeaponPanel> panels = new();

        [TitleGroup("Config")] 
        public float targetFade;
        public float fadeDuration;
        public Ease easeType;

        private Sequence _currSequence;
        private WeaponPanel _currentPanel;

        private void Start() {
            this.SubscribeListener(EventType.OnWeaponChange, slot => OnWeaponChange((WeaponBase.Slot) slot));
            
            foreach (var panel in panels) {
                if (panel.slot == WeaponBase.Slot.Primary) {
                    panel.panel.color = new Color(0, 0, 0, 0);
                    _currentPanel = panel;
                }
                else {
                    panel.panel.color = new Color(0, 0, 0, targetFade);
                }
            }
        }

        private void OnWeaponChange(WeaponBase.Slot slot) {
            var targetPanel = panels.Find(panel => panel.slot == slot);
            _currSequence?.Kill();
            _currSequence = DOTween.Sequence();

            _currSequence
                .Append(_currentPanel.panel.DOFade(targetFade, fadeDuration))
                .Insert(0, targetPanel.panel.DOFade(0, fadeDuration))
                .SetEase(easeType);

            _currentPanel = targetPanel;
        }
    }
}
