using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.EventDispatcher;
using Sirenix.OdinInspector;
using Survivor;
using UnityEngine;
using EventType = Core.EventDispatcher.EventType;
using Random = UnityEngine.Random;

namespace Base {
    public class Base : Singleton<Base> {
        public float hp;
        [ReadOnly] public float currentHp;

        [TitleGroup("Config")] 
        public GameObject defender;
        public List<Transform> defensePoints = new();
        [ReadOnly] public List<SurvivorDefend> defenders = new();

        [TitleGroup("Info")] 
        [ReadOnly] public int survivorCounts;
        [ReadOnly] public int peakedSurvivorCounts;
        [Space] 
        [ReadOnly] public int defenseSurvivors;
        [ReadOnly] public int researchSurvivors;
        
        public void Start() {
            currentHp = hp;
            this.SubscribeListener(EventType.OnSurvivorEnteredBase, _ => AddSurvivors());
        }

        public void AddSurvivors() {
            survivorCounts++;
            if (peakedSurvivorCounts < survivorCounts) peakedSurvivorCounts = survivorCounts;

            if (defenseSurvivors > researchSurvivors) {
                researchSurvivors++;
            }
            else if (researchSurvivors > defenseSurvivors) {
                AddDefender();
            }
            else if (researchSurvivors == defenseSurvivors) {
                var i = Random.Range(0, 2);
                if (i == 0) {
                    AddDefender();
                }
                else researchSurvivors++;
            }
        }

        public void TakeDamage(float amount) {
            currentHp -= amount;
            if (survivorCounts > 0) {
                survivorCounts--;
                if (defenseSurvivors > researchSurvivors) {
                    RemoveDefender();
                }
                else if (researchSurvivors > defenseSurvivors) {
                    researchSurvivors--;
                }
                else if (researchSurvivors == defenseSurvivors) {
                    var i = Random.Range(0, 2);
                    if (i == 0) RemoveDefender();
                    else researchSurvivors--;
                }
            }
        }

        public void AddDefender() {
            defenseSurvivors++;
            if (defensePoints.Count >= defenseSurvivors) {
                var defInst = Instantiate(defender, defensePoints[defenseSurvivors - 1].position, Quaternion.identity);
                defenders.Add(defInst.GetComponent<SurvivorDefend>());
            }
        }

        public void RemoveDefender() {
            defenseSurvivors--;
            if (defensePoints.Count >= defenseSurvivors) {
                var defInst = defenders[defenseSurvivors];
                defenders.Remove(defInst);
                Destroy(defInst.gameObject);
            }
        }
    }
}
