using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject hud;

    public static bool isPaused = false;

    private void Start()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        hud.SetActive(true);
        Utilities.toggleMouseLock(true);
    }

    private void Update()
    {
        if(Input.GetButtonDown("Pause"))
        {
            togglePause();
        }
    }

    public void togglePause()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        hud.SetActive(!isPaused);
        Utilities.toggleMouseLock(!isPaused);
        //condition ? trueCase : falseCase;
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void onQuitClick()
    {
        SceneManager.LoadScene(0);
    }
}
