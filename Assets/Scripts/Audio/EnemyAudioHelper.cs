using UnityEngine;
using UnityEngine.Serialization;

namespace Audio {
    public class EnemyAudioHelper : MonoBehaviour {
        public AudioData enemyWalkData;
        public AudioData enemyHitData;

        public enum Type {
            Walk,
            Hit
        }
        public void Play(Type type) {
            if (type == Type.Walk) {
                AudioManager.instance
                    .PlayClipAtPoint(transform.position, enemyWalkData.clips[Random.Range(0, enemyWalkData.clips.Count)]);
            }
            else {
                AudioManager.instance
                    .PlayClipAtPoint(transform.position, enemyHitData.clips[Random.Range(0, enemyHitData.clips.Count)]);
            }
        }
    }
}
