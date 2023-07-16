using Core.EventDispatcher;
using UI;
using UnityEngine;
using EventType = Core.EventDispatcher.EventType;

namespace Manager {
    public class ScoreManager : MonoBehaviour
    {
        private int _currentScore;

        // Start is called before the first frame update
        void Start()
        {
            this.SendMessage(EventType.OnTextUIChange, new TextMessage() {
                type = TextUI.TextType.Score,
                message = _currentScore.ToString("0")
            });
            this.SubscribeListener(EventType.OnSurvivorAdded, _ => UpdateScore(20));
            this.SubscribeListener(EventType.OnSurvivorEnteredBase, _ => UpdateScore(100));
            this.SubscribeListener(EventType.OnEnemyDie, _ => UpdateScore(120));
            this.SubscribeListener(EventType.OnTurnEnd, _ => UpdateScore(1000));
            this.SubscribeListener(EventType.OnEnemyConvert, _ => UpdateScore(50));

        }

        private void UpdateUI(int score)
        {
            this.SendMessage(EventType.OnTextUIChange, new TextMessage() {
                type = TextUI.TextType.Score,
                message = _currentScore.ToString("0")
            });
        }

        public void UpdateScore(int scoreToAdd)
        {
            _currentScore += scoreToAdd;
            UpdateUI(_currentScore);
        }

    
    }
}
