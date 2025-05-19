using UnityEngine;

public class PersistentSound : MonoBehaviour
{
    private static PersistentSound instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Не уничтожать этот объект при загрузке новой сцены
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Если другой экземпляр уже существует, уничтожить этот
        }
    }
}
