using UnityEngine;

// Базовый интерфейс для всех способностей героя
public interface IAbility
{
    void Init(Rigidbody2D rb, SpriteRenderer sr);
    void OnUpdate();
    void OnFixedUpdate();
    void OnJump();
    void OnLand();
    void OnExit();
}
