using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusiListElement : MonoBehaviour
{
    private Melody _melody;
    private Music _music;

    [SerializeField] private TMP_Text _name, _author, _recomend, _time;

    public Melody Melody { get => _melody; }

    public void SetMelodyInfo(Melody melody, Music music)
    {
        _music = music;
        _melody = melody;
        _name.text = _melody.name;
        _author.text = _melody.author;
        _recomend.text = $"Рекомендация: {_melody.recomend}";
        _time.text = TimeToText(_melody.playtime);
    }

    public void StartPlay()
    {
        _music.DownloadPreview(_melody);
    }

    private string TimeToText(int time) => string.Format((time / 60).ToString("00") + ":" + (time % 60).ToString("00"));
}

