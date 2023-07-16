using System;
using UnityEngine;

namespace Audio {
    public class PlayerAudioHelper : MonoBehaviour {
        public AudioClip playerWalk;
        public AudioClip playerShoot;
        public AudioClip playerReload;
        public AudioClip playerHit;
        public AudioClip playerThrow;

        public enum Type {
            Walk,
            Shoot,
            Reload,
            Hit,
            Throw,
        }

        public void Play(Type type) {
            var clip = type switch {
                Type.Walk => playerWalk,
                Type.Shoot => playerShoot,
                Type.Reload => playerReload,
                Type.Hit => playerHit,
                _ => playerThrow
            };
            
            AudioManager.instance.PlayOneShot(clip);
        }
    }
}
