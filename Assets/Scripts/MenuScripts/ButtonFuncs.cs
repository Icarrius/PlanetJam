using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFuncs : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuHolder;
    [SerializeField] private GameObject creditHolder;

    public void ExitButton()
    {
        Application.Quit();
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void SoundButton()
    {
        SoundHandler.singleton.TurnSound();
    }

    public void CreditButton()
    {
        creditHolder.SetActive(true);
        mainMenuHolder.SetActive(false);
    }

    public void BackButton()
    {
        mainMenuHolder.SetActive(true);
        creditHolder.SetActive(false);
    }
}
