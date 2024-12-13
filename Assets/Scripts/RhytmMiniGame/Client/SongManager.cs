using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine.Events;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Lane[] _lanes;
    [SerializeField] private Music _music;

    [Header("NotesSettings")]
    public double marginOfError; // in seconds
    public float inputDelayInMilliseconds;
    public float noteTime;
    public float noteSpawnY;
    public float noteTapY;
    [SerializeField] private float _songDelayInSeconds;


    public static MidiFile midiFile;
    [SerializeField] private EndGame _endGame;
    private Melody _melody;
    private int _totalNotes;
    private static DifficultLevel _difficultLevel = DifficultLevel.Low;


    public UnityEvent _datasDownload;

    #region Properties
    public float noteDespawnY
    {
        get
        {
            return noteTapY - (noteSpawnY - noteTapY);
        }
    }

    public static DifficultLevel DifficultLevel { get => _difficultLevel; set => _difficultLevel = value; }
    #endregion


    private void Start()
    {
        Instance = this;
    }

    public void SettingsSetup(Melody melody)
    {
        _melody = melody;
        _audioSource.clip = _music.LoadedOGGFiles[melody.id];
        midiFile = _music.LoadedMIDIFiles[melody.id];
        GetDataFromMidi();
        SetupDifficult();
        _datasDownload?.Invoke();
        Actions.OnListCreated?.Invoke();        
    }
    public void Clear()
    {
        _audioSource.Stop();
    }
    public void StartSong()
    {
        _audioSource.Play();
    }

    public void GetDataFromMidi()
    {
            var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        _totalNotes = notes.Count;
        notes.CopyTo(array, 0);

        foreach (var lane in _lanes)
        {
            lane.Clear();
            lane.SetTimeStamps(array);
            lane.gameObject.SetActive(true);
        }
        Invoke(nameof(StartSong), inputDelayInMilliseconds);
    }
    
    public static double GetAudioSourceTime()
    {
        return (double)Instance._audioSource.timeSamples / Instance._audioSource.clip.frequency;
    }

    public void EndSong()
    {
        StartCoroutine(_endGame.Waiter(_totalNotes, _melody));
    }

    private void SetupDifficult()
    {
        switch (DifficultLevel)
        {
            case DifficultLevel.Low:
                noteTime = 2;
                marginOfError = .13d;
                break;
            case DifficultLevel.Medium:
                noteTime = 1.2f;
                marginOfError = .085d;
                break;
            case DifficultLevel.High:
                noteTime = .8f;
                marginOfError = .055d;
                break;
            default:
                noteTime = 2;
                marginOfError = .13d;
                break;
        }
    }
    private void OnDisable()
    {
        this.StopAllCoroutines();
    }
}
