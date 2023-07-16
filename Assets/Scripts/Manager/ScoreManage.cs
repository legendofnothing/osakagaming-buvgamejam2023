using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
using UnityEngine.PlayerLoop;
using UnityEngine.SocialPlatforms.Impl;
using EventType = Core.EventDispatcher.EventType;
using Core.EventDispatcher;

public class ScoreManage : MonoBehaviour
{
    private int _currentScore;
    [SerializeField] private TextMeshPro _scoreText;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = _currentScore.ToString();
        this.SubscribeListener(Core.EventDispatcher.EventType.OnSurvivorAdded, _ => UpdateScore(20));
        this.SubscribeListener(Core.EventDispatcher.EventType.OnSurvivorEnteredBase, _ => UpdateScore(100));
        this.SubscribeListener(Core.EventDispatcher.EventType.OnEnemyDie, _ => UpdateScore(120));
        this.SubscribeListener(Core.EventDispatcher.EventType.OnTurnEnd, _ => UpdateScore(1000));
        this.SubscribeListener(Core.EventDispatcher.EventType.OnEnemyConvert, _ => UpdateScore(50));

    }

    private void UpdateUI(int score)
    {
        _scoreText.text = score.ToString();
    }

    public void UpdateScore(int scoreToAdd)
    {
        _currentScore += scoreToAdd;
        UpdateUI(_currentScore);
    }

    
}
