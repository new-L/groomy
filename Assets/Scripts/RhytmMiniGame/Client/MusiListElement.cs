using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusiListElement : MonoBehaviour
{
    private Melody _melody;
    private Music _music;
    private SongManager _songManager;

    [SerializeField] private TMP_Text _name, _author, _recomend, _time;

    public Melody Melody { get => _melody; }

    public void SetMelodyInfo(Melody melody, SongManager _song, Music music)
    {
        _music = music;
        _songManager = _song;
        _melody = melody;
        _name.text = _melody.name;
        _author.text = "Автор: " + _melody.author;
        _recomend.text = "Рекомендация: " + _melody.recomend;
        _time.text = TimeToText(_melody.playtime);
    }

    public void StartPlay()
    {
        _music.DownloadPreview(_melody);
        //if (Music.LoadedOGGFiles.ContainsKey(_melody.id))
        //{
        //    if (Music.LoadedMIDIFiles.ContainsKey(_melody.id))
        //    {
        //        _songManager.SettingsSetup(_melody);
        //        Debug.Log("Уже есть!");
        //    }
        //    else
        //    {
        //        if (Music.LoadedOGGFiles.Remove(_melody.id))
        //        {
        //            _music.DownloadMelody(_melody);
        //            return;
        //        }
        //    }
        //}
        //else
        //{
        //    if (Music.LoadedMIDIFiles.Remove(_melody.id))
        //    {
        //        _music.DownloadMelody(_melody);
        //        return;
        //    }
        //    Debug.Log("Не было!");
        //    _music.DownloadMelody(_melody);
        //}

    }

    private string TimeToText(int time) => string.Format((time / 60).ToString("00") + ":" + (time % 60).ToString("00"));
}

