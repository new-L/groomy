using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MusicList : MonoBehaviour
{
    [Header("List")]
    [SerializeField] private Music _music;
    [SerializeField] private Melody _melody;
    [SerializeField] private SongManager _songManager;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private GameObject _taskListView;
    [SerializeField] private GameObject _logo;
    [SerializeField] private GameObject _backButton;
    [SerializeField] private Transform _content;

    [Header("Music Preview Panel")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private Image _previewImage;
    [SerializeField] private TMP_Text _name, _author, _reward, _time;
    [SerializeField] private AudioSource _previewAudioSource;

    [Header("MusicInfo")]
    [SerializeField] private TMP_Text _gameName;
    [SerializeField] private TMP_Text _gameAuthor;
    [SerializeField] private TMP_Text _gameNameAnim;
    [SerializeField] private TMP_Text _gameRecomendAnim;

    [SerializeField] private UnityEvent OnStartLoad;

    private void Start()
    {
        OnStartLoad?.Invoke();
    }

    public void ReloadScene()
    {
        OnStartLoad?.Invoke();
        _gameName.gameObject.SetActive(false);
        _gameAuthor.gameObject.SetActive(false);
        _taskListView.SetActive(true);
        _logo.SetActive(true);
        _backButton.SetActive(true);
    }

    public void SetUpList()
    {
        ClearList(_content);
        if (_music.Melodies.Length == 0 || _music.Melodies == null)
        {
            Actions.OnListCreated?.Invoke();
            return;
        }
        foreach (var item in _music.Melodies)
        {
            var instance = Instantiate(_prefab);
            instance.transform.SetParent(_content, false);
            instance.GetComponent<MusiListElement>().SetMelodyInfo(item, _music);
        }
        Actions.OnListCreated?.Invoke();
    }

    public void SetUpPreview(Sprite sprite, AudioClip clip, Melody melody)
    {
        _melody = melody;
        _previewImage.sprite = sprite;
        _previewAudioSource.clip = clip;
        _name.text = melody.name;
        _author.text = $"{melody.author} | {melody.recomend}";
        _reward.text = $"Награда: +{melody.reward}";
        _time.text = TimeToText(melody.playtime);
        _panel.SetActive(true);
        _previewAudioSource.Play();
        _previewAudioSource.Play();
        Actions.OnListCreated?.Invoke();
    }

    public void StartPlay(int index)
    {
        _gameName.text = _melody.name;
        _gameNameAnim.text = _melody.name;
        _gameRecomendAnim.text = _melody.recomend;
        _gameAuthor.text = _melody.author;
        SongManager.DifficultLevel = GetLevel(index);
        if (_music.LoadedOGGFiles.ContainsKey(_melody.id))
        {
            if (_music.LoadedMIDIFiles.ContainsKey(_melody.id))
            {
                _songManager.SettingsSetup(_melody);
            }
            else
            {
                if (_music.LoadedOGGFiles.Remove(_melody.id))
                {
                    _music.DownloadMelody(_melody);
                    return;
                }
            }
        }
        else
        {
            if (_music.LoadedMIDIFiles.Remove(_melody.id))
            {
                _music.DownloadMelody(_melody);
                return;
            }
            _music.DownloadMelody(_melody);
        }
    }

    public void ReloadGame()
    {
        StartPlay((int)SongManager.DifficultLevel);
    }

    private void ClearList(Transform content)
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }

    private DifficultLevel GetLevel(int index)
    {
        switch ((DifficultLevel)index)
        {
            case (DifficultLevel.Low): return DifficultLevel.Low;
            case (DifficultLevel.Medium): return DifficultLevel.Medium;
            case (DifficultLevel.High): return DifficultLevel.High;
            default: return DifficultLevel.Low;
        }
    }
    private string TimeToText(int time) => string.Format((time / 60).ToString("00") + ":" + (time % 60).ToString("00"));
}
