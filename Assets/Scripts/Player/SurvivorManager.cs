using System;
using System.Collections.Generic;
using Core.EventDispatcher;
using Sirenix.OdinInspector;
using Survivor;
using UnityEngine;
using EventType = Core.EventDispatcher.EventType;

namespace PlayerCS {
    public class SurvivorManager : MonoBehaviour {
        [ReadOnly] public List<SurvivorBase> survivors;

        private void Start() {
            this.SubscribeListener(EventType.OnSurvivorAdded, s => AddSurvivor((SurvivorBase) s));
            this.SubscribeListener(EventType.OnSurvivorDecreased, s => DecreaseSurvivor((SurvivorBase) s));
        }

        private void AddSurvivor(SurvivorBase survivor) {
            survivors.Add(survivor);
        }
        
        private void DecreaseSurvivor(SurvivorBase survivor) {
            survivors.Remove(survivor);
        }
    }
}
