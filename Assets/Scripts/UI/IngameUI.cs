using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI {
    public class InGameUI : MonoBehaviour {
        public GameObject pauseMenu;

        private bool _isPaused;

        private void Start() {
            pauseMenu.SetActive(false);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                switch (_isPaused) {
                    case true:
                        pauseMenu.SetActive(false);
                        Time.timeScale = 1;
                        _isPaused = false;
                        break;
                    
                    default:
                        pauseMenu.SetActive(true);
                        Time.timeScale = 0;
                        _isPaused = true;
                        break;
                }
            }
        }
    }
}
