using System;
using Core.EventDispatcher;
using DG.Tweening;
using UnityEngine;
using EventType = Core.EventDispatcher.EventType;

namespace Manager {
    public class LevelManager : MonoBehaviour {
        public enum State {
            Preparing,
            Playing,
            Ending,
        }

        public float beforeTurnDelay = 5f;
        
        private void Start() {
            UpdateTurn(State.Preparing);
            this.SubscribeListener(EventType.OnTurnEnd, _ => UpdateTurn(State.Ending));
        }

        private void UpdateTurn(State state) {
            switch (state) {
                case State.Preparing:
                    DOVirtual.DelayedCall(beforeTurnDelay, () => {
                        UpdateTurn(State.Playing);
                    });
                    break;
                case State.Playing:
                    this.SendMessage(EventType.OnTurnBegin);
                    break;
                case State.Ending:
                    UpdateTurn(State.Preparing);
                    break;
            }
        }
    }
}
