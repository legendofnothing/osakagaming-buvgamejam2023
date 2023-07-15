using System;
using System.Collections.Generic;
using System.Linq;
using Core.EventDispatcher;
using Enemy;
using UnityEngine;
using EventType = Core.EventDispatcher.EventType;
using Random = UnityEngine.Random;

namespace Manager {
    public class SurvivorManager : MonoBehaviour
    {
        [Header("Config")] 
        public GameObject survivorPrefab;
        public List<Transform> spawnPoints;
        [Space] 
        public int minSpawnAmount;
        public int maxSpawnAmount;

        private void Start() {
            this.SubscribeListener(EventType.OnTurnBegin, _=>SpawnSurvivor());
        }

        public void SpawnSurvivor() {
            var amount = Random.Range(minSpawnAmount, maxSpawnAmount + 1);
            var rnd = new System.Random();
            for (var i = 0; i < amount; i++) {
                var randomSpawnPoint = spawnPoints.OrderBy(_ => rnd.Next()).FirstOrDefault();
                if (randomSpawnPoint != null) {
                    Instantiate(survivorPrefab, randomSpawnPoint.position , Quaternion.identity);
                }
            }
        }
    }
}
