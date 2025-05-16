using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectUI : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform buttonParent;
    public static int totalLevels = 3;

    void Start()
    {
        for (int i = 1; i <= totalLevels; i++)
        {          
            if (i <= PlayerPrefs.GetInt("LastLevel", 0) + 1) {
                int levelIndex = i;

                GameObject btn = Instantiate(buttonPrefab, buttonParent);
                btn.GetComponentInChildren<Text>().text = levelIndex.ToString();

                btn.GetComponent<Button>().onClick.AddListener(() =>
                {
                    SceneManager.LoadScene(levelIndex);
                });
            }
        }
    }
}