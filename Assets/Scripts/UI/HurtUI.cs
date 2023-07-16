using System;
using Core.EventDispatcher;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using EventType = Core.EventDispatcher.EventType;

namespace UI {
    public class HurtUI : MonoBehaviour {
        public Image hurtImage;
        public float targetFade;
        private Sequence _currTween;

        private void Start() {
            hurtImage.color = new Color(hurtImage.color.r, hurtImage.color.g, hurtImage.color.b, 0);
            this.SubscribeListener(EventType.OnPlayerTakeDamage, _ => {
                _currTween?.Kill();
                _currTween = DOTween.Sequence();
                _currTween
                    .Append(hurtImage.DOFade(targetFade, 0.4f))
                    .AppendInterval(0.2f)
                    .Append(hurtImage.DOFade(0, 0.2f));
            });
        }
    }
}
