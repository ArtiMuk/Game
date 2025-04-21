using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform hero;
    [SerializeField] private float camSpeed = 5f; // Скорость, с которой камера следует за героем
    [SerializeField] private Vector3 offset = new Vector3(0, -0.5f, -11); // Смещение камеры относительно позиции героя (x, y, z)

    private Vector3 position;

    private void Awake()
    {
        if (!hero)
            hero = FindFirstObjectByType<Hero>().transform;
    }

    void Update()
    {
        CamMove();
    }

    void CamMove() // Метод следования камеры за героем
    {
        position = hero.position + offset; // Позиция героя + смещение камеры для вычисления необходимой позиции

        // Перемешаем камеру в position со скоростью camSpeed
        transform.position = Vector3.Lerp(
            transform.position, 
            position, 
            camSpeed * Time.deltaTime); 
    }
}
