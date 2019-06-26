using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour {

    public AudioMixer audioMixer;

    public void ChangeQuality(int index)
    {
        QualitySettings.SetQualityLevel(index); 
    }

    public void ChangeFullScreen(bool useFullScreen)
    {
        Screen.fullScreen = useFullScreen;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ChangeVolumeAudioMixer(float volume)
    {
        audioMixer.SetFloat("MasterVolume",volume);
    }

    public void ChangeSensibilityMouse(float newSensibility)
    {
        GameManager.Instance.ChangeSensibility(newSensibility);
    }

    public void ResumeGame()
    {
        GameManager.Instance.SetupPauseMenu();
    }

    public void ReloadGame()
    {
        GameManager.Instance.SetupGameMenu();
    }
}
