using UnityEngine;

public class LevelMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
            ChangeMenuMode();
    }

    public void ChangeMenuMode()
    {
        if (menuPanel == null) return;

        bool isActive = menuPanel.activeSelf;
        menuPanel.SetActive(!isActive);
        Time.timeScale = isActive ? 1.0f : 0.0f;
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
