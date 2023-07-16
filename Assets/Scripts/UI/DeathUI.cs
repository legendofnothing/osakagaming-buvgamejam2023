using System;
using Audio;
using Core.EventDispatcher;
using DG.Tweening;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using EventType = Core.EventDispatcher.EventType;

namespace UI {
    public class DeathUI : MonoBehaviour {
        public CanvasGroup main;
        public TextMeshProUGUI text;

        private bool _canReturn;

        private void Start() {
            this.SubscribeListener(EventType.OnEndGame, _ => {
                Time.timeScale = 0;
                AudioManager.instance.PauseMusic();
                text.SetText(ScoreManager.instance._currentScore.ToString());
                main.DOFade(0, 1.2f).OnComplete(() => {
                    gameObject.SetActive(true);
                    _canReturn = true;
                }).SetUpdate(true);
            });
            
            gameObject.SetActive(false);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Return) && _canReturn) {
                SceneManager.LoadScene("Scenes/Main Menu");
            }
        }
    }
}
