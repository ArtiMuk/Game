using System.Collections;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    private AbilityManager abilityManager;
    private GameObject hero;
    private Vector3 startPosition;
    private Vector3? lastFireCheckpoint;

    private void Start()
    {
        hero = GameObject.FindGameObjectWithTag("Player");
        abilityManager = hero.GetComponent<AbilityManager>();
        startPosition = hero.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (abilityManager.currentAbility is FireAbility fire)
        {
            // Если активна способность огня, обновляем последний чекпоинт и респавним на нем
            lastFireCheckpoint = fire.GetCheckpoint();
            StartCoroutine(RespawnAtCheckpoint(lastFireCheckpoint.Value));
        }
        else
        {
            // Если активна другая способность, всегда респавним на старте
            StartCoroutine(RespawnAtStart());
        }
    }

    private IEnumerator RespawnAtStart()
    {
        yield return new WaitForSeconds(0.1f);
        hero.transform.position = startPosition;
        var rb = hero.GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;
        Debug.Log($"[Respawn] Teleported to start position {startPosition}");
    }

    private IEnumerator RespawnAtCheckpoint(Vector3 checkpoint)
    {
        yield return new WaitForSeconds(0.1f);
        hero.transform.position = checkpoint;
        var rb = hero.GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;
        Debug.Log($"[Respawn] Teleported to checkpoint {checkpoint}");
    }
}
