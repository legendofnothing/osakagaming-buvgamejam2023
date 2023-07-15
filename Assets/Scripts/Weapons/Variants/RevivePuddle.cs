using System;
using Core;
using DG.Tweening;
using Enemy;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Weapons.Variants {
    public class RevivePuddle : MonoBehaviour {
        [TitleGroup("Config")] 
        public LayerMask enemyLayer;
        public float duration = 6f;

        private void Start() {
            DOVirtual.DelayedCall(duration, () => {
                GetComponent<SpriteRenderer>().DOFade(0, 1.8f).OnComplete(() => Destroy(gameObject));
            });
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (CheckLayerMask.IsInLayerMask(other.gameObject, enemyLayer) &&
                other.gameObject.TryGetComponent<EnemyBase>(out var enemy)) {
                enemy.Convert();
            }
        }
    }
}
