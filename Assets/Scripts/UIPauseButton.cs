using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPauseButton : MonoBehaviour
{
    public GameObject gameplayManagerObj;
    private GameplayManager gameplayManagerScript;

    public GameObject pauseScreen;

    private AudioSource buttonSound;

    private void Start()
    {
        gameplayManagerScript = gameplayManagerObj.GetComponent<GameplayManager>();
        buttonSound = GameObject.Find("ButtonSound").GetComponent<AudioSource>();
    }

    public void PauseButton()
    {
        buttonSound.Play();
        gameplayManagerScript.SetTapControlActive(false);
        pauseScreen.SetActive(true);
        gameObject.SetActive(false);
    }
}
