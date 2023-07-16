using System;
using Core.EventDispatcher;
using TMPro;
using UnityEngine;
using EventType = Core.EventDispatcher.EventType;

namespace UI {
    
    public class TextUI : MonoBehaviour
    {
        public enum TextType {
            MolotovCount,
            FollowingCount,
            DefendersCount,
            ResearchersCount,
            
            TotalSurvivorsInBase,
            MaximumSurvivorsWereInBase,
            DefenderLeft,
            CureProgress,
        }

        [Header("Config")] 
        public TextType type;
        public TextMeshProUGUI text;

        private void Awake() {
            this.SubscribeListener(EventType.OnTextUIChange, message => {
                var converted = (TextMessage)message;
                if (converted.type == type) {
                    text.SetText(converted.message);
                }
            });
        }
    }
    
    public struct TextMessage {
        public TextUI.TextType type;
        public string message;
    }
}
