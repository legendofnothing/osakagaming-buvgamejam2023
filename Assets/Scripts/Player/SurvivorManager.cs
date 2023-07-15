using System;
using System.Collections.Generic;
using Core.EventDispatcher;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Survivor;
using UnityEngine;
using EventType = Core.EventDispatcher.EventType;

namespace PlayerCS {
    public class SurvivorManager : MonoBehaviour {
        [ReadOnly] public List<SurvivorBase> survivors;

        private void Start() {
            this.SubscribeListener(EventType.OnSurvivorAdded, s => AddSurvivor((SurvivorBase) s));
            this.SubscribeListener(EventType.OnSurvivorDecreased, s => DecreaseSurvivor((SurvivorBase) s));
            this.SubscribeListener(EventType.OnPlayerTakeDamage, _=>OnPlayerHurt());
        }

        private void AddSurvivor(SurvivorBase survivor) {
            survivors.Add(survivor);
        }
        
        private void DecreaseSurvivor(SurvivorBase survivor) {
            survivors.Remove(survivor);
        }

        private void OnPlayerHurt() {
            if (survivors.IsNullOrEmpty()) return;
            var survivorInst = survivors[^1];
            survivors.Remove(survivorInst);
            survivorInst.TakeDamage(9999999f);
        }
    }
}
