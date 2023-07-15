using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.EventDispatcher;
using DG.Tweening;
using Sirenix.OdinInspector;
using Survivor;
using UI;
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
        [Space] 
        [ReadOnly] public float currentCureProgress;

        private Tween _currentCureTween;
        
        public void Start() {
            currentHp = hp;
            this.SubscribeListener(EventType.OnSurvivorEnteredBase, _ => AddSurvivors());
            FireUIEvent();
            StartTween();
            
            this.SubscribeListener(EventType.OnCureReset, _ => {
                StartTween();
            });
        }
        
        private void FireUIEvent() {
            this.SendMessage(EventType.OnTextUIChange, new TextMessage() {
                type = TextUI.TextType.DefendersCount,
                message = $"x{defenseSurvivors}"
            });
            
            this.SendMessage(EventType.OnTextUIChange, new TextMessage() {
                type = TextUI.TextType.ResearchersCount,
                message = $"x{researchSurvivors}"
            });
            
            this.SendMessage(EventType.OnTextUIChange, new TextMessage() {
                type = TextUI.TextType.TotalSurvivorsInBase,
                message = $"x{survivorCounts}"
            });
            
            this.SendMessage(EventType.OnTextUIChange, new TextMessage() {
                type = TextUI.TextType.DefenderLeft,
                message = $"{defenders.Count}/{defensePoints.Count}"
            });
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
            
            FireUIEvent();
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
            
            FireUIEvent();
        }

        public void AddDefender() {
            defenseSurvivors++;
            if (defensePoints.Count >= defenseSurvivors) {
                var defInst = Instantiate(defender, defensePoints[defenseSurvivors - 1].position, Quaternion.identity);
                defenders.Add(defInst.GetComponent<SurvivorDefend>());
            }
            
            FireUIEvent();
        }

        public void RemoveDefender() {
            defenseSurvivors--;
            if (defensePoints.Count >= defenseSurvivors) {
                var defInst = defenders[defenseSurvivors];
                defenders.Remove(defInst);
                Destroy(defInst.gameObject);
            }
            
            FireUIEvent();
        }

        public void StartTween() {
            currentCureProgress = 0;
            _currentCureTween = DOVirtual.DelayedCall(2f, () => {
                currentCureProgress += 0.1f * researchSurvivors;
                FireUIEvent();

                if (currentCureProgress >= 100) {
                    _currentCureTween.Kill();
                    this.SendMessage(EventType.OnTextUIChange, new TextMessage() {
                        type = TextUI.TextType.CureProgress,
                        message = "COMPLETED"
                    });
                }
                else {
                    this.SendMessage(EventType.OnTextUIChange, new TextMessage() {
                        type = TextUI.TextType.CureProgress,
                        message = currentCureProgress.ToString("0.00") + "%"
                    });
                }
            }).SetLoops(-1, LoopType.Restart);
        }
    }
}
