using System;
using System.Collections.Generic;
using System.Linq;
using Core.EventDispatcher;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using EventType = Core.EventDispatcher.EventType;

namespace UI {
    public class BarUIColorChange : MonoBehaviour {
        [Serializable]
        public struct BarColorPhase {
            public float valueThreshold;
            public Color color;
        }

        public BarUI.BarType type;
        public Slider slider;
        public Image sliderFill;
        public List<BarColorPhase> colorPhases = new();
        
        [TitleGroup("Tween Config")]
        public float duration;
        public Ease easeType;

        private Tween _currTween;

        private void Start() {
            this.SubscribeListener(EventType.OnBarUIChange, barMess => OnValueChange((BarMessage) barMess)); 
        }

        public void OnValueChange(BarMessage message) {
            if (message.type != type) return;
            
            BarColorPhase targetPhase = new BarColorPhase();
            var sortedPhases = colorPhases.OrderBy(x => x.valueThreshold);
            foreach (var phase in sortedPhases) {
                if (slider.value - phase.valueThreshold > 0) {
                    targetPhase = phase;
                    break;
                }
            }
            if (sliderFill.color == targetPhase.color) return;
            _currTween?.Kill();
            _currTween = sliderFill.DOColor(targetPhase.color, duration).SetEase(easeType);
        }
    }
}
