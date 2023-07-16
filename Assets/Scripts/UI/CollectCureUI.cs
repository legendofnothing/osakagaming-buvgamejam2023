using System;
using Core.EventDispatcher;
using DG.Tweening;
using UnityEngine;
using EventType = Core.EventDispatcher.EventType;

namespace UI {
    public class CollectCureUI : MonoBehaviour {
        public CanvasGroup baseGroup;
        public GameObject cureReady;
        public GameObject cureNotReady;

        private Tween _currTween;
        private bool _canPickupCure;

        private void Start() {
            baseGroup.alpha = 0;
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.E) && _canPickupCure) {
                this.SendMessage(EventType.OnMolotovAdded);
                this.SendMessage(EventType.OnCureReset);
                cureReady.SetActive(false);
                cureNotReady.SetActive(true);
            }

            if (Base.Base.instance.currentCureProgress >= 100 && !cureReady.activeInHierarchy) {
                cureNotReady.SetActive(false);
                cureReady.SetActive(true);
                _canPickupCure = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
                _currTween?.Kill();
                
                cureReady.SetActive(false);
                cureNotReady.SetActive(false);
                if (Base.Base.instance.currentCureProgress >= 100) {
                    cureReady.SetActive(true);
                    _canPickupCure = true;
                }
                else {
                    cureNotReady.SetActive(true);
                }

                _currTween = baseGroup.DOFade(1, 0.8f);
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
                _currTween?.Kill();
                _currTween = baseGroup.DOFade(0, 1.2f);
            }
        }
    }
}
