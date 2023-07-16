using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.EventDispatcher;
using DG.Tweening;
using Player;
using Sirenix.OdinInspector;
using Survivor;
using UI;
using UnityEngine;
using EventType = Core.EventDispatcher.EventType;
using Random = UnityEngine.Random;

namespace Base {
    public enum ModifierType {
        PlayerBuff,
        PlayerDebuff,
        DefenderBuff,
        DefenderDebuff,
    }
    
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
        [ReadOnly] public float faith;
        [Space] 
        [ReadOnly] public List<ModifierType> currentModifiers = new();

        private Tween _currentCureTween;
        
        public void Start() {
            faith = 1;
            currentHp = hp;
            this.SubscribeListener(EventType.OnSurvivorEnteredBase, _ => AddSurvivors());
            FireUIEvent();
            StartTween();
            
            this.SubscribeListener(EventType.OnCureReset, _ => {
                StartTween();
            });
            
            this.SubscribeListener(EventType.OnTransferResearchersToDefenders, _ => {
                if (researchSurvivors > 0) {
                    researchSurvivors--;
                    AddDefender();
                }
            });
            
            this.SubscribeListener(EventType.OnTransferDefendersToResearchers, _ => {
                if (defenseSurvivors > 0) {
                    RemoveDefender();
                    researchSurvivors++;
                }
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
            
            this.SendMessage(EventType.OnTextUIChange, new TextMessage() {
                type = TextUI.TextType.MaximumSurvivorsWereInBase,
                message = $"x{peakedSurvivorCounts}"
            });
            
            this.SendMessage(EventType.OnBarUIChange, new BarMessage() {
                type = BarUI.BarType.Base,
                value = currentHp / hp
            });
            
            this.SendMessage(EventType.OnBarUIChange, new BarMessage() {
                type = BarUI.BarType.Faith,
                value = faith
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

            faith = (float) survivorCounts / peakedSurvivorCounts;
            
            HandleModifier();
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
            
            faith = (float) survivorCounts / peakedSurvivorCounts;
            
            HandleModifier();
            FireUIEvent();
        }

        public void AddDefender() {
            defenseSurvivors++;
            if (defensePoints.Count >= defenseSurvivors) {
                var defInst = Instantiate(defender, defensePoints[defenseSurvivors - 1].position, Quaternion.identity);
                defenders.Add(defInst.GetComponent<SurvivorDefend>());
            }
            
            HandleModifier();
            FireUIEvent();
        }

        public void RemoveDefender() {
            defenseSurvivors--;
            if (defensePoints.Count > defenseSurvivors) {
                var defInst = defenders[defenseSurvivors];
                defenders.Remove(defInst);
                this.SendMessage(EventType.OnSurvivorDecreased);

                StartCoroutine(defInst.Death());
                //Destroy(defInst.gameObject);
            }
            
            HandleModifier();
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

        public void HandleModifier() {
            if (faith < 0.4f) {
                if (!currentModifiers.Contains(ModifierType.PlayerDebuff)) currentModifiers.Add(ModifierType.PlayerDebuff);
                
                if (currentModifiers.Contains(ModifierType.PlayerBuff)) {
                    currentModifiers.Remove(ModifierType.PlayerBuff);
                    this.SendMessage(EventType.OnModifierDeactivated,ModifierType.PlayerBuff);
                }
            } 
            
            else if (faith > 0.6f) {
                if (!currentModifiers.Contains(ModifierType.PlayerBuff)) currentModifiers.Add(ModifierType.PlayerBuff);
                
                if (currentModifiers.Contains(ModifierType.PlayerDebuff)) {
                    currentModifiers.Remove(ModifierType.PlayerDebuff);
                    this.SendMessage(EventType.OnModifierDeactivated,ModifierType.PlayerDebuff);
                }
            }

            else {
                if (currentModifiers.Contains(ModifierType.PlayerBuff)) {
                    currentModifiers.Remove(ModifierType.PlayerBuff);
                    this.SendMessage(EventType.OnModifierDeactivated,ModifierType.PlayerBuff);
                }
                
                if (currentModifiers.Contains(ModifierType.PlayerDebuff)) {
                    currentModifiers.Remove(ModifierType.PlayerDebuff);
                    this.SendMessage(EventType.OnModifierDeactivated,ModifierType.PlayerDebuff);
                }
            }

            if (defenseSurvivors / (float) survivorCounts > 0.65f) {
                if (!currentModifiers.Contains(ModifierType.DefenderBuff)) currentModifiers.Add(ModifierType.DefenderBuff);
                
                if (currentModifiers.Contains(ModifierType.DefenderDebuff)) {
                    currentModifiers.Remove(ModifierType.DefenderDebuff);
                    this.SendMessage(EventType.OnModifierDeactivated,ModifierType.DefenderDebuff);
                }
            }
            else if (faith < 0.3f) {
                if (!currentModifiers.Contains(ModifierType.DefenderDebuff)) currentModifiers.Add(ModifierType.DefenderDebuff);
                
                if (currentModifiers.Contains(ModifierType.DefenderBuff)) {
                    currentModifiers.Remove(ModifierType.DefenderBuff);
                    this.SendMessage(EventType.OnModifierDeactivated,ModifierType.DefenderBuff);
                }
            }
            else {
                if (currentModifiers.Contains(ModifierType.DefenderBuff)) {
                    currentModifiers.Remove(ModifierType.DefenderBuff);
                    this.SendMessage(EventType.OnModifierDeactivated,ModifierType.DefenderBuff);
                }
                
                if (currentModifiers.Contains(ModifierType.DefenderDebuff)) {
                    currentModifiers.Remove(ModifierType.DefenderDebuff);
                    this.SendMessage(EventType.OnModifierDeactivated,ModifierType.DefenderDebuff);
                }
            }

            foreach (var modifier in currentModifiers) {
                this.SendMessage(EventType.OnModifierActivated, modifier);
                
                switch (modifier) {
                    case ModifierType.PlayerBuff:
                        CombatManager.instance.damageModifier = 1.5f;
                        CombatManager.instance.speedModifier = 1.15f;
                        break;
                    case ModifierType.PlayerDebuff:
                        CombatManager.instance.damageModifier = 0.8f;
                        CombatManager.instance.speedModifier = 0.8f;
                        break;
                    case ModifierType.DefenderBuff:
                        CombatManager.instance.damageModifier = 1.5f;
                        CombatManager.instance.speedModifier = 1.5f;
                        break;
                    case ModifierType.DefenderDebuff:
                        CombatManager.instance.damageModifier = 0.8f;
                        CombatManager.instance.speedModifier = 0.8f;
                        break;
                }
            }
        }
    }
}
