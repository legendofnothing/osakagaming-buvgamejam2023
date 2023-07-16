using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.EventDispatcher;
using Enemy;
using UnityEngine;
using EventType = Core.EventDispatcher.EventType;
using Random = UnityEngine.Random;

namespace Manager {
    public class EnemyManager : Singleton<EnemyManager> {
        [Header("Config")] 
        public GameObject enemyPrefab;
        public List<EnemyBase> enemies;
        public List<Transform> spawnPoints;
        [Space] 
        public int minSpawnAmount;
        public int maxSpawnAmount;
        
        
        private void Start() {
            this.SubscribeListener(EventType.OnEnemyDie, e => OnEnemyDie((EnemyBase) e));
            this.SubscribeListener(EventType.OnTurnBegin, _=>SpawnEnemy());
        }

        public void SpawnEnemy() {
            var amount = Random.Range(minSpawnAmount, maxSpawnAmount + 1);
            var rnd = new System.Random();
            for (var i = 0; i < amount; i++) {
                var randomSpawnPoint = spawnPoints.OrderBy(_ => rnd.Next()).FirstOrDefault();
                if (randomSpawnPoint != null) {
                    var enemyInst = Instantiate(enemyPrefab, randomSpawnPoint.position, Quaternion.identity);
                    enemies.Add(enemyInst.GetComponent<EnemyBase>());
                }
            }
        }

        private void OnEnemyDie(EnemyBase enemy) {
            enemies.Remove(enemy);
            if (enemies.Count == 0) {
                this.SendMessage(EventType.OnTurnEnd);
                minSpawnAmount++;
                minSpawnAmount++;
            }
        }
    }
}
