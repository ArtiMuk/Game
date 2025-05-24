using UnityEngine;

public class LevelMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject settingsPanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
            ChangeMenuMode();
    }

    public void ChangeMenuMode()
    {
        bool isActive = menuPanel.activeSelf;
        menuPanel.SetActive(!isActive);
        Time.timeScale = isActive ? 1.0f : 0.0f;
    }

    public void ChangeSettingsPanelMode()
    {
        menuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void BackToMenuFromSettings()
    {
        settingsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void OpenMainMenu()
    {  
        Time.timeScale = 1.0f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
