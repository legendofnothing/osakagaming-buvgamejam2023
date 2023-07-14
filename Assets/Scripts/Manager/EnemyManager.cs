using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Manager {
    public class EnemyManager : MonoBehaviour {
        [Header("Config")] 
        public GameObject enemyPrefab;
        public List<Transform> spawnPoints;
        [Space] 
        public int minSpawnAmount;
        public int maxSpawnAmount;

        public void SpawnEnemy() {
            var amount = Random.Range(minSpawnAmount, maxSpawnAmount + 1);
            var rnd = new System.Random();
            for (var i = 0; i < amount; i++) {
                var randomSpawnPoint = spawnPoints.OrderBy(_ => rnd.Next()).FirstOrDefault();
                if (randomSpawnPoint != null) {
                    Instantiate(enemyPrefab, randomSpawnPoint.position * Random.insideUnitCircle * 0.2f, Quaternion.identity);
                }
            }
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                SpawnEnemy();
            }
        }
    }
}
