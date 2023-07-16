using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI {
    public class InGameUI : MonoBehaviour {
        public GameObject mainMenu;
        public GameObject pauseMenu;

        private bool _isPaused;

        private void Start() {
            pauseMenu.SetActive(false);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                switch (_isPaused) {
                    case true:
                        mainMenu.SetActive(true);
                        pauseMenu.SetActive(false);
                        Time.timeScale = 1;
                        _isPaused = false;
                        break;
                    
                    default:
                        mainMenu.SetActive(false);
                        pauseMenu.SetActive(true);
                        Time.timeScale = 0;
                        _isPaused = true;
                        break;
                }
            }
        }

        public void UnPause() {
            mainMenu.SetActive(false);
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
            _isPaused = true;
        }

        public void Restart() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void Quit() {
            Application.Quit();
        }
    }
}
