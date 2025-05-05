using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Respawn : MonoBehaviour
{
    private AbilityManager abilityManager;

    private void Start()
    {
        GameObject hero = GameObject.FindGameObjectWithTag("Player");
        abilityManager = hero.GetComponent<AbilityManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("trig");
        if (other.CompareTag("Player"))
        {
            if (abilityManager != null && abilityManager.currentAbility is FireAbility)
            {
                Debug.Log("FireRespawn");
                StartCoroutine(LoadCurLevel()); // Тут реализуй для огненного
            }
            else
                StartCoroutine(LoadCurLevel());

        }
    }

    private IEnumerator LoadCurLevel()
    {
        yield return new WaitForSeconds(0.3f);

        int currentScene = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentScene);
    }
}
