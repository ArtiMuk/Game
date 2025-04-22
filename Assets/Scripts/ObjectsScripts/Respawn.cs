using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Respawn : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("trig");
        if (other.CompareTag("Player"))
            StartCoroutine(LoadCurLevel());
    }

    private IEnumerator LoadCurLevel()
    {
        yield return new WaitForSeconds(0.3f);

        int currentScene = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentScene);
    }
}
