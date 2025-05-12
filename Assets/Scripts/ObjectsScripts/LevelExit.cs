using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("trig");
        if (other.CompareTag("Player"))
            StartCoroutine(LoadNextLevel());
    }

    private IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(1);

        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;
        Debug.Log($"Current: {currentScene}, Next: {nextScene}, Total: {SceneManager.sceneCountInBuildSettings}");
        if (nextScene == SceneManager.sceneCountInBuildSettings)
        {
            nextScene = 0;
        }
        PlayerPrefs.SetInt("LastLevel", currentScene);
        PlayerPrefs.Save();

        SceneManager.LoadScene(nextScene);
    }
}
