using System;
using Audio;
using Core;
using DG.Tweening;
using Enemy;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Weapons.Variants {
    public class RevivePuddle : MonoBehaviour {
        [TitleGroup("Config")] 
        public AudioClip bottleBreak;
        public LayerMask enemyLayer;
        public float duration = 6f;

        private bool _canConvert = true;
        private PolygonCollider2D _collider2D;

        private void Start() {
            AudioManager.instance.PlayClipAtPoint(transform.position, bottleBreak);
            _collider2D = GetComponent<PolygonCollider2D>();
            _collider2D.enabled = false;
            transform.localScale = Vector3.zero;

            transform.DOScale(1, 0.8f)
                .SetEase(Ease.OutExpo)
                .OnComplete(() => {
                    _collider2D.enabled = true;
                    
                    DOVirtual.DelayedCall(duration, () => {
                        _canConvert = false;
                        GetComponent<SpriteRenderer>().DOFade(0, 2.8f).OnComplete(() => Destroy(gameObject));
                    });
                });
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (!_canConvert) return;
            
            if (CheckLayerMask.IsInLayerMask(other.gameObject, enemyLayer) &&
                other.gameObject.TryGetComponent<EnemyBase>(out var enemy)) {
                enemy.Convert();
            }
        }
    }
}
