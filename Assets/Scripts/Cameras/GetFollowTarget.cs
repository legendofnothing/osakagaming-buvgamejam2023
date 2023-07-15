using System;
using Cinemachine;
using Player;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Cameras {
    public class GetFollowTarget : MonoBehaviour {
        [TitleGroup("Refs")] 
        public CinemachineVirtualCamera virtualCamera;

        private void Awake() {
            var player = FindObjectOfType<PlayerMovement>();
            virtualCamera.Follow = player.gameObject.transform;
        }
    }
}
