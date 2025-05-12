using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;

    public void Play()
    {
        int nextLevel = 0;//PlayerPrefs.GetInt("LastLevel", 1) != 1 ? PlayerPrefs.GetInt("LastLevel", 1) + 1 : 0;
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