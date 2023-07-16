using Core;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace Audio {
    public class AudioManager : Singleton<AudioManager> {
        [TitleGroup("Audio Sources")] 
        public AudioSource musicSource;
        public AudioSource sfxSource;
        [Space] 
        public GameObject audioSourceInstance;

        public void PlayClipAtPoint(Vector2 position, AudioClip clip) {
            var inst = Instantiate(audioSourceInstance, position, quaternion.identity);
            var audioSource = inst.GetComponent<AudioSource>();

            audioSource.clip = clip;
            audioSource.Play();

            DOVirtual.DelayedCall(clip.length, () => Destroy(inst));
        }

        public void PlayOneShot(AudioClip clip) {
            sfxSource.PlayOneShot(clip);
        }
    }
}
