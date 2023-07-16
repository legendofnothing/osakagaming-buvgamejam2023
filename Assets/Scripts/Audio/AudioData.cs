using System.Collections.Generic;
using UnityEngine;

namespace Audio {
    [CreateAssetMenu(fileName = "AudioData", menuName = "Audio/AudioData", order = 1)]
    public class AudioData : ScriptableObject {
        public List<AudioClip> clips = new();
    }
}
