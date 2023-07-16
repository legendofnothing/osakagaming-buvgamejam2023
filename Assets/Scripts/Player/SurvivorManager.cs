using System;
using System.Collections.Generic;
using Core.EventDispatcher;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Survivor;
using UI;
using UnityEngine;
using EventType = Core.EventDispatcher.EventType;

namespace PlayerCS {
    public class SurvivorManager : MonoBehaviour {
        [ReadOnly] public List<SurvivorBase> survivors;

        private void Start() {
            this.SubscribeListener(EventType.OnSurvivorAdded, s => AddSurvivor((SurvivorBase) s));
            this.SubscribeListener(EventType.OnSurvivorDecreased, s => DecreaseSurvivor((SurvivorBase) s));
            this.SubscribeListener(EventType.OnPlayerTakeDamage, _=>OnPlayerHurt());

            FireUIEvent();
        }

        private void AddSurvivor(SurvivorBase survivor) {
            survivors.Add(survivor);
            FireUIEvent();
        }
        
        private void DecreaseSurvivor(SurvivorBase survivor) {
            survivors.Remove(survivor);
            FireUIEvent();
        }

        private void OnPlayerHurt() {
            if (survivors.IsNullOrEmpty()) return;
            var survivorInst = survivors[^1];
            survivors.Remove(survivorInst);
            survivorInst.TakeDamage(9999999f);
            FireUIEvent();
        }

        private void FireUIEvent() {
            this.SendMessage(EventType.OnTextUIChange, new TextMessage() {
                type = TextUI.TextType.FollowingCount,
                message = $"x{survivors.Count}"
            });
        }
    }
}
