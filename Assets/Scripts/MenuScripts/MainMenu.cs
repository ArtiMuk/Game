using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;

    public void Play()
    {
        int nextLevel = PlayerPrefs.GetInt("LastLevel", 0) + 1;
        if (nextLevel == SceneManager.sceneCountInBuildSettings)
        {
            nextLevel = 1;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextLevel);
    }

    public void OpenLevelSelect()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        levelSelectPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}