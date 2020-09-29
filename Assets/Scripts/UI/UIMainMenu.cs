using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    public AudioSource ButtonSound;
    // Start is called before the first frame update
    public void PlayButton()
    {
        ButtonSound.Play();
        SceneManager.LoadScene("Game");
    }

    public void ExitButton()
    {
        ButtonSound.Play();
        Application.Quit();
    }
}
