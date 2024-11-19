using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    void PauseGame()
    {
        if (!isPaused)
        {
            Time.timeScale = 0f;
            isPaused = true;

            if (AudioManager.instance != null)
            {
                AudioManager.instance.PauseAllSounds();
            }

        }
    }

    void ResumeGame()
    {
        if (isPaused)
        {
            Time.timeScale = 1f;
            isPaused = false;

            if (AudioManager.instance != null)
            {
                AudioManager.instance.ResumeAllSounds();
            }
        }
    }
}