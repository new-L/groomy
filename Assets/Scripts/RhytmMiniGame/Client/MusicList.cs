using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusicList : MonoBehaviour
{
    [SerializeField] private Music _music;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _content;

    [SerializeField] private SongManager _songManager;

    [Header("Music Preview Panel")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private Image _previewImage;
    [SerializeField] private TMP_Text _name, _author, _reward;
    [SerializeField] private AudioSource _previewAudioSource;
    public void SetUpList()
    {
        if (_music.Melodies.Length == 0 || _music.Melodies == null)
        {
            Actions.OnListCreated?.Invoke();
            return;
        }
        foreach (var item in _music.Melodies)
        {
            var instance = Instantiate(_prefab);
            instance.transform.SetParent(_content, false);
            instance.GetComponent<MusiListElement>().SetMelodyInfo(item, _songManager, _music);
        }
        Actions.OnListCreated?.Invoke();
    }

    public void SetUpPreview(Sprite sprite, AudioClip clip, Melody melody)
    {
        _previewImage.sprite = sprite;
        _previewAudioSource.clip = clip;
        _name.text = melody.name;
        _author.text = $"{melody.author} | {melody.recomend}";
        _reward.text = $"Стандартная награда: +{melody.reward}";
        _panel.SetActive(true);
        _previewAudioSource.Play();
        Actions.OnListCreated?.Invoke();
    }
}
