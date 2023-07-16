using System;
using Core.EventDispatcher;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using EventType = Core.EventDispatcher.EventType;

namespace UI {
    public struct BarMessage {
        public BarUI.BarType type;
        public float value;
    }
    
    public class BarUI : MonoBehaviour {
        public enum BarType {
            Health,
            Faith,
            Base,
        }

        [Header("Config")] 
        public BarType type;
        public Slider slider;

        [Header("Tween Config")] 
        public float duration;
        public Ease easeType;

        private void Awake() {
            this.SubscribeListener(EventType.OnBarUIChange, barMess => OnBarChange((BarMessage) barMess));    
        }

        private void OnBarChange(BarMessage message) {
            if (message.type != type) return;
            slider.DOValue(message.value, duration).SetEase(easeType);
        }
    }
}
