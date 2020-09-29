using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICountdown : MonoBehaviour
{
    [SerializeField] private GameObject _lockScreen;
    [SerializeField] private int _countDownTime;
    private Text _text;
    public AudioSource CountDownSound;
    
    void Awake()
    {
        _text = GetComponent<Text>();
        StartCoroutine(CountDown());
    }
    
    IEnumerator CountDown()
    {

        Time.timeScale = 0f;
        for (int i = _countDownTime; i > 0; i--)
        {
            //тик
            CountDownSound.Play();
            _text.text = i.ToString();
            yield return new WaitForSecondsRealtime(1f);
        }
        Destroy(_lockScreen);
        CountDownSound.Play();
        _text.text = "Go!";
        Time.timeScale = 1f;
        yield return new WaitForSecondsRealtime(1f);
        Destroy(gameObject);
    }
}
