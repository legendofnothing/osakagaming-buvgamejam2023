using System;
using Core;
using Core.EventDispatcher;
using Sirenix.OdinInspector;
using UnityEngine;
using EventType = Core.EventDispatcher.EventType;

namespace Base {
    public class Base : Singleton<Base> {
        public float hp;
        [ReadOnly] public float currentHp;

        [TitleGroup("Info")] 
        [ReadOnly] public int survivorCounts;
        
        public void Start() {
            currentHp = hp;
            this.SubscribeListener(EventType.OnSurvivorEnteredBase, _=>survivorCounts++);
        }

        public void TakeDamage(float amount) {
            currentHp -= amount;
            if (survivorCounts > 0) survivorCounts--;
        }
    }
}
