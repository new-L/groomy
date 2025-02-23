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
    [SerializeField] private GameObject _canvasTop;
    [SerializeField] private Transform _content;

    [Header("Music Preview Panel")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private Image _previewImage;
    [SerializeField] private GameObject _goldIcon;
    [SerializeField] private TMP_Text _name, _author, _reward, _time;
    [SerializeField] private AudioSource _previewAudioSource;

    [Header("MusicInfo")]
    [SerializeField] private TMP_Text _gameName;
    [SerializeField] private TMP_Text _gameAuthor;
    [SerializeField] private TMP_Text _gameNameAnim;
    [SerializeField] private TMP_Text _gameRecomendAnim;

    [SerializeField] private GameLimit _limit;
    [SerializeField] private UnityEvent OnStartLoad;
    [SerializeField] private UnityEvent OnListLoaded;

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
        _canvasTop.SetActive(true);
    }

    public void SetUpList()
    {
        ClearList(_content);
        if (_music.Melodies.Length == 0 || _music.Melodies == null)
        {
            OnListLoaded?.Invoke();
            return;
        }
        foreach (var item in _music.Melodies)
        {
            var instance = Instantiate(_prefab);
            instance.transform.SetParent(_content, false);
            instance.GetComponent<MusiListElement>().SetMelodyInfo(item, _music);
        }
        OnListLoaded?.Invoke();
    }

    public void SetUpPreview(Sprite sprite, AudioClip clip, Melody melody)
    {
        _melody = melody;
        _previewImage.sprite = sprite;
        _previewAudioSource.clip = clip;
        _name.text = melody.name;
        _author.text = $"{melody.author} | {melody.recomend}";
        if (_limit.Limit.current <= 0)
        {
            _reward.text = $"Только рейтинг!";
            _goldIcon.SetActive(false);
        }
        else
        {
            _reward.text = $"Награда: +{melody.reward}";
            _goldIcon.SetActive(true);
        }
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

    private DifficultLevel GetLevel(int index) => (DifficultLevel)index;
    private string TimeToText(int time) => string.Format((time / 60).ToString("00") + ":" + (time % 60).ToString("00"));
}
