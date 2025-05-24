using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    private AudioSource musicSource;

    private const string VolumePrefKey = "MusicVolume";

    void Start()
    {
        // Найти MusicPlayer и получить AudioSource
        var musicPlayer = GameObject.Find("MusicPlayer");
        if (musicPlayer != null)
            musicSource = musicPlayer.GetComponent<AudioSource>();

        // Загрузить сохранённую громкость или установить дефолт
        float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, 0.7f);
        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
        SetVolume(savedVolume);
    }

    public void SetVolume(float value)
    {
        if (musicSource != null)
            musicSource.volume = value;
        PlayerPrefs.SetFloat(VolumePrefKey, value);
        PlayerPrefs.Save();
    }
}
