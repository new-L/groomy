using Melanchall.DryWetMidi.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Networking;

public class Music : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private MusicList _list;
    [SerializeField] private SongManager _songManager;
    private static Dictionary<int, AudioClip> _loadedOGGFiles = new Dictionary<int, AudioClip>();
    private static Dictionary<int, MidiFile> _loadedMIDIFiles = new Dictionary<int, MidiFile>();

    private Melody[] _melodies;
    private UnityWebRequest _www;
    private string _json;
    private byte[] _result;

    private Texture2D _texture;
    private Sprite _sprite;
    

[SerializeField] private UnityEvent _onStartLoad;
    
    #region Properties
    private MidiFile _midiFile;
    public Melody[] Melodies { get => _melodies; }
    public byte[] Result { get => _result; set => _result = value; }
    public static Dictionary<int, AudioClip> LoadedOGGFiles { get => _loadedOGGFiles; set => _loadedOGGFiles = value; }
    public static Dictionary<int, MidiFile> LoadedMIDIFiles { get => _loadedMIDIFiles; set => _loadedMIDIFiles = value; }
    #endregion


    private void Start()
    {
        LoadedOGGFiles.Clear();
        LoadedMIDIFiles.Clear();
        Actions.OnStartLoad?.Invoke();
        StartCoroutine(DownloadMusicList());
    }
    public void DownloadMelody(Melody melody)
    {
        Actions.OnStartLoad?.Invoke();
        StartCoroutine(DownloadMelodyOGG(melody));
    }

    public void DownloadPreview(Melody melody)
    {
        Actions.OnStartLoad?.Invoke();
        StartCoroutine(DownloadPreviewFromServer(melody));
    }

    private IEnumerator DownloadMusicList()
    {
        _www = UnityWebRequest.Get(URLs.MelodiesList);
        _www.timeout = ServerSettings.TimeOut;
        yield return _www.SendWebRequest();
        if (_www.error != null) { Debug.Log("Не удалось связаться с сервером!"); yield break; }

        _json = JsonHelper.fixJson(_www.downloadHandler.text);
        Debug.Log(_json);
        _melodies = JsonHelper.FromJson<Melody>(_json);
        if (_melodies != null) _list.SetUpList();
    }


    private IEnumerator ReadMIDIFromSite(Melody melody)
    {
        _www = UnityWebRequest.Get(melody.midi_url);
        yield return _www.SendWebRequest();

        if (_www.isNetworkError || _www.isHttpError)
        {
            Debug.LogError(_www.error);
        }
        else
        {
            Result = _www.downloadHandler.data;
            using (var stream = new MemoryStream(Result))
            {
                _midiFile = MidiFile.Read(stream);
            }
            LoadedMIDIFiles.Add(melody.id, _midiFile);
            _songManager.SettingsSetup(melody);
        }
    }

    private IEnumerator DownloadMelodyOGG(Melody melody)
    {
        _www = UnityWebRequestMultimedia.GetAudioClip(melody.ogg_url, AudioType.OGGVORBIS);
        yield return _www.SendWebRequest();
        if (_www.isNetworkError || _www.isHttpError)
        {
            Debug.LogError(_www.error);
        }
        else
        {
            _audioSource.clip = DownloadHandlerAudioClip.GetContent(_www) as AudioClip;
            LoadedOGGFiles.Add(melody.id, _audioSource.clip);
            StartCoroutine(ReadMIDIFromSite(melody));
            //_audioSource.clip = DownloadHandlerAudioClip.GetContent(_www);
            //_audioSource.Play();
        }
    }

    private IEnumerator DownloadPreviewFromServer(Melody melody)
    {
        _www = UnityWebRequestTexture.GetTexture(melody.preview_img_url);
        yield return _www.SendWebRequest();
        if (_www.isNetworkError || _www.isHttpError)
        {
            Debug.LogError(_www.error);
        }
        else
        {
            _texture = ((DownloadHandlerTexture)_www.downloadHandler).texture;
            _sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2());
            _www = UnityWebRequestMultimedia.GetAudioClip(melody.preview_ogg_url, AudioType.OGGVORBIS);
            yield return _www.SendWebRequest();
            if (_www.isNetworkError || _www.isHttpError)
            {
                Debug.LogError(_www.error);
            }
            else
            {
                _list.SetUpPreview(_sprite, DownloadHandlerAudioClip.GetContent(_www), melody);
            }
        }
        
    }

    private void OnDisable()
    {
        this.StopAllCoroutines();
        LoadedOGGFiles.Clear();
        LoadedMIDIFiles.Clear();
    }

}

[Serializable]
public class Melody
{
    public int id;
    public string name;
    public string author;
    public string recomend;
    public string midi_url;
    public string ogg_url;
    public string preview_img_url;
    public string preview_ogg_url;
    public int playtime;
    public int reward;
}
