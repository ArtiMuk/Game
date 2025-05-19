using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour
{
    private AbilityManager abilityManager;
    private GameObject hero;

    private void Start()
    {
        hero = GameObject.FindGameObjectWithTag("Player");
        abilityManager = hero.GetComponent<AbilityManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (abilityManager.currentAbility is FireAbility fire)
            StartCoroutine(RespawnAtCheckpoint(fire));
        else
            StartCoroutine(ReloadScene());
    }

    private IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator RespawnAtCheckpoint(FireAbility fire)
    {
        yield return new WaitForSeconds(0.1f);

        Vector3 cp = fire.GetCheckpoint();
        hero.transform.position = cp;
        var rb = hero.GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        Debug.Log($"[Respawn] Teleported to checkpoint {cp}");
    }
}
