using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform hero;
    [SerializeField] private float camSpeed = 5f; // ��������, � ������� ������ ������� �� ������
    [SerializeField] private Vector3 offset = new Vector3(0, -0.5f, -11); // �������� ������ ������������ ������� ����� (x, y, z)

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

    void CamMove() // ����� ���������� ������ �� ������
    {
        position = hero.position + offset; // ������� ����� + �������� ������ ��� ���������� ����������� �������

        // ���������� ������ � position �� ��������� camSpeed
        transform.position = Vector3.Lerp(
            transform.position, 
            position, 
            camSpeed * Time.deltaTime); 
    }
}
