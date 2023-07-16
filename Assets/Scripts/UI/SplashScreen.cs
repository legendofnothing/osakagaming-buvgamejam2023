using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour {
    public Image splashScreen;

    private void Start() {
        splashScreen.color = new Color(1, 1, 1, 0);

        var s = DOTween.Sequence();
        s
            .Append(splashScreen.DOFade(1, 3.6f))
            .AppendInterval(2f)
            .Append(splashScreen.DOFade(1, 2.4f))
            .OnComplete(() => {
                SceneManager.LoadScene("Main Menu");
            });
    }
}
