using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Singletone
    private static GameManager _instance;
    public static GameManager GetInstance()
    {
        if (_instance != null)
        {
            return _instance;
        }
        else
        {
            _instance = FindObjectOfType<GameManager>();
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            return;
        }
        else
        {
            _instance = this;
        }
    }
    #endregion
    [SerializeField] private GameObject _player;
    [SerializeField] private UIController _UIController;
    [SerializeField] private int _sessionTime = 60;
    [SerializeField] private AudioSource DeafeatMenuSound;

    static PlayerPrefs ScoreContainer;
    
    private bool _gameEnd = false;
    private float _timer;
    public event Action TimerEnded;
    public event Action GameLoose;
    public event Action GameWin;
    public static float Timer
    {
        get
        {
            return GetInstance()._timer;
        }
        set
        {

            var instance = GetInstance();
            if (value <= 0) 
            {
                instance._timer = 0;
                if (!instance._gameEnd)
                {
                    instance._gameEnd = true;
                    instance.TimerEnded?.Invoke();
                }
            }
            else
            {
                if (instance._gameEnd) { return; }
                instance._timer = value;
            }
            instance.UpdateVisualTimer();
        }
    }
    public static GameObject Player { get => GetInstance()._player; private set => GetInstance()._player = value; }

    public static void LooseGame()
    {
        var instance = GetInstance();
        if (!instance._gameEnd)
        {
            instance.DeafeatMenuSound.PlayDelayed(1);
            instance._gameEnd = true;
            instance.GameLoose?.Invoke();
        }
        //GetInstance().GameLoose?.Invoke();
    }
    public static void WinGame()
    {
        
        var instance = GetInstance();
        if (!instance._gameEnd)
        {
            instance._gameEnd = true;
            instance.GameWin?.Invoke();
            //Score
            instance.UpdateVisualScore();   
        }
        //GetInstance().GameLoose?.Invoke();
    }
    private void Start()
    {
        _timer = _sessionTime;
    }
    private void Update()
    {
        Timer -= Time.deltaTime;
    }

    private void UpdateVisualTimer()
    {
        _UIController.UpdateTimer(Mathf.RoundToInt(_timer));
    }

    private void UpdateVisualScore()
    {
        _UIController.UpdateScore(Mathf.RoundToInt(_timer * 10));
    }

}
