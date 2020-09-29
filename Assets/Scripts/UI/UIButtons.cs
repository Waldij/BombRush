using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour
{
    public AudioSource ButtonSound;
    private bool isPause;

    [SerializeField] private GameObject _gameObject;
    // Start is called before the first frame update
    
    public void RestartGame()
    {
        ButtonSound.Play();
        SceneManager.LoadScene("Game");
        ResumeGame();
    }
    public void PauseGame()
    {
        ButtonSound.Play();
        Time.timeScale = 0f;
        _gameObject.SetActive(true);
        isPause = true;
    }

    public void ResumeGame()
    {
        ButtonSound.Play();
        Time.timeScale = 1f;
        _gameObject.SetActive(false);
        isPause = false;
    }

    public void ExitMainMenu()
    {
        ButtonSound.Play();
        SceneManager.LoadScene("MainMenu");
    }
    
}