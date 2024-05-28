using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Canvas pauseMenu;
    [SerializeField] private bool shouldPlayMusic = false;
    [SerializeField] private AudioSource[] audioSourcesToMute;

    //public bool AutoLockCursor { get; set; } = true;

    private AudioSource audioSource;

    private void Awake()
    {
        if (shouldPlayMusic)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void Start()
    {
        pauseMenu.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EnablePauseMenu();
        }

        /*
        if (AutoLockCursor)
        {
            Cursor.lockState = pauseMenu.enabled ? CursorLockMode.None : CursorLockMode.Locked;
        }
        */
    }

    public void EnablePauseMenu()
    {
        pauseMenu.enabled = !pauseMenu.enabled;
        Time.timeScale = Time.timeScale == 1 ? 0 : 1;
        Cursor.lockState = Time.timeScale == 1 ? CursorLockMode.Locked : CursorLockMode.None;
        if (shouldPlayMusic)
        {
            audioSource.Play();
            PlayOtherAudios(true);
            if (!pauseMenu.enabled)
            {
                audioSource.Stop();
                PlayOtherAudios(false);
            }
        }
    }

    private void PlayOtherAudios(bool shouldPlay)
    {
        for (int i = 0; i < audioSourcesToMute.Length; i++)
        {
            if (shouldPlay)
            {
                audioSourcesToMute[i].Pause();
            }
            else
            {
                audioSourcesToMute[i].Play();
            }
        }
    }

    public void Resume()
    {
        pauseMenu.enabled = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        if (shouldPlayMusic)
        {
            audioSource.Stop();
            PlayOtherAudios(false);
        }
    }

    public void Quit()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
