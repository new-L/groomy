using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine.Events;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    public AudioSource audioSource;
    public Lane[] lanes;
    public float songDelayInSeconds;
    public double marginOfError; // in seconds

    public float inputDelayInMilliseconds;
    
    public float noteTime;
    public float noteSpawnY;
    public float noteTapY;


    public UnityEvent _datasDownload;

    [SerializeField] private EndGame _endGame;
    private int _totalNotes;
    private Melody _melody;    
    private static DifficultLevel _difficultLevel = DifficultLevel.Low;
    public float noteDespawnY
    {
        get
        {
            return noteTapY - (noteSpawnY - noteTapY);
        }
    }

    public static DifficultLevel DifficultLevel { get => _difficultLevel; set => _difficultLevel = value; }

    public static MidiFile midiFile;


    private void Start()
    {
        Instance = this;
    }

    public void SettingsSetup(Melody melody)
    {
        _melody = melody;
        audioSource.clip = Music.LoadedOGGFiles[melody.id];
        midiFile = Music.LoadedMIDIFiles[melody.id];
        GetDataFromMidi();
        _datasDownload?.Invoke();
        Actions.OnListCreated?.Invoke();        
    }

    public void StartSong()
    {
        audioSource.Play();
    }

    public void GetDataFromMidi()
    {
            var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        _totalNotes = notes.Count;
        notes.CopyTo(array, 0);

        foreach (var lane in lanes)
        {
            lane.Clear();
            lane.SetTimeStamps(array);
            lane.gameObject.SetActive(true);
        }

        Invoke(nameof(StartSong), inputDelayInMilliseconds);
    }
    
    public static double GetAudioSourceTime()
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }

    public void EndSong()
    {
        _endGame.RewardCalculation(_totalNotes, _melody);
    }
}
