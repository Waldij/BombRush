using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    #region Singletone
    private static UIController _instance;
    public AudioSource LampSound;
    public static UIController GetInstance()
    {
        if (_instance != null)
        {
            return _instance;
        }
        else
        {
            _instance = FindObjectOfType<UIController>();
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
    [SerializeField] private Text _timerText;
    [SerializeField] private Text _scoreText;
    [SerializeField] private GameObject _defeatMenu;
    [SerializeField] private GameObject _winMenu;
    [SerializeField] private List<GameObject> _lamps;
    public void UpdateTimer(int value)
    {
        _timerText.text = value.ToString();
    }

    public void UpdateScore(int value)
    {
        _scoreText.text = value.ToString();
    }

    public static void ShowDefeatMenu()
    {
        GetInstance()._defeatMenu.gameObject.SetActive(true);
    }
    public static void ShowWinMenu()
    {
        GetInstance()._winMenu.gameObject.SetActive(true);
    }
    public static void SetLamps(int count)
    {
        if (count < 0) { count = 0; }
        else if (count > 3) { count = 3; }
        int oldCount = 0;
        foreach (var lamp in GetInstance()._lamps)
        {
            if (lamp.activeSelf) { oldCount++; }
            lamp.SetActive(false);
        }
        for (int i = 0; i < count; i++)
        {
            GetInstance()._lamps[i].SetActive(true);
        }
        //звук лампы
        if (count > oldCount)
        {
            GetInstance().LampSound.Play();
        }

    }
}
