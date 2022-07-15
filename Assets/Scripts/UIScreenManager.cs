using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class UIScreenManager : MonoBehaviour
{
    public GameObject gameplayManagerObj;
    private GameplayManager gameplayManagerScript;

    private AudioSource buttonSound;

    private void Start()
    {
        gameplayManagerScript = gameplayManagerObj.GetComponent<GameplayManager>();
        buttonSound = GameObject.Find("ButtonSound").GetComponent<AudioSource>();
    }

    public void PlayButton()
    {
        buttonSound.Play();
        gameObject.SetActive(false);
        gameplayManagerScript.InitialSetup();
        gameplayManagerScript.SetTapControlActive(true);
    }

    public async void MainMenuButton()
    {
        buttonSound.Play();
        // Wait till sound ends before loading scene.
        await Task.Delay(System.TimeSpan.FromSeconds(buttonSound.clip.length));
        SceneManager.LoadScene("Main");
    }
}
