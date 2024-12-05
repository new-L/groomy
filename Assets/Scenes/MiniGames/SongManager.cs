using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using UnityEngine.Networking;
using System;
using UnityEngine.Events;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    public AudioSource audioSource;
    public Lane[] lanes;
    public float songDelayInSeconds;
    public double marginOfError; // in seconds

    public int inputDelayInMilliseconds;
    

    public string fileLocation;
    public float noteTime;
    public float noteSpawnY;
    public float noteTapY;


    public UnityEvent _datasDownload;
    public float noteDespawnY
    {
        get
        {
            return noteTapY - (noteSpawnY - noteTapY);
        }
    }

    public static MidiFile midiFile;

    private void Start()
    {
        Instance = this;
    }

    public void SettingsSetup(Melody melody)
    {
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
        notes.CopyTo(array, 0);

        foreach (var lane in lanes)
        {
            lane.Clear();
            lane.SetTimeStamps(array);
            lane.gameObject.SetActive(true);
        }

        Invoke(nameof(StartSong), 0f);
    }
    
    public static double GetAudioSourceTime()
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }
}
