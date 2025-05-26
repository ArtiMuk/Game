using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject levelSelectPanel;
    [SerializeField] private GameObject SettingsPanel;

    public void Play()
    {
        int nextLevel = PlayerPrefs.GetInt("LastLevel", 0) + 1;
        if (nextLevel == SceneManager.sceneCountInBuildSettings)
        {
            nextLevel -= 1;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextLevel);
    }

    public void OpenLevelSelect()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
    }

    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        SettingsPanel.SetActive(true);
    }

    public void BackToMainMenuFromLevelSelect()
    {
        levelSelectPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void BackToMainMenuFromSettings()
    {
        SettingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}