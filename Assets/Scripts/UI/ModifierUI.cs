using System;
using System.Collections.Generic;
using Base;
using Core.EventDispatcher;
using UnityEngine;
using EventType = Core.EventDispatcher.EventType;

namespace UI {

    public class ModifierUI : MonoBehaviour {
        [Serializable]
        public struct ModifierPanel {
            public ModifierType type;
            public GameObject panel;
        }

        public List<ModifierPanel> panels = new();

        private void Start() {
            foreach (var panel in panels) {
                panel.panel.SetActive(false);
            }
            
            this.SubscribeListener(EventType.OnModifierActivated, modifier => {
                var converted = (ModifierType)modifier;
                panels.Find(x => x.type == converted).panel.SetActive(true);
            });
            
            this.SubscribeListener(EventType.OnModifierDeactivated, modifier => {
                var converted = (ModifierType)modifier;
                panels.Find(x => x.type == converted).panel.SetActive(false);
            });
        }
    }
}
