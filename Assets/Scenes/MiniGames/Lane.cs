using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    public KeyCode input;
    public GameObject notePrefab;
    public List<Note> notes = new List<Note>();
    public List<double> timeStamps = new List<double>();
    

    int spawnIndex = 0;
    int inputIndex = 0;

    [SerializeField] private bool _isPressed = false;
    [SerializeField] private bool _isLastElement = false;
    [SerializeField] private bool _isFirstElement = false;
    [SerializeField] private SongManager _songManager;


    public void Clear()
    {
        gameObject.SetActive(false);
        Debug.Log("Destroy");
        foreach (Note child in gameObject.GetComponentsInChildren<Note>())
        {
            Destroy(child.gameObject);
        }
        _isLastElement = false;
        _isFirstElement = false;
        notes = new List<Note>();
        timeStamps = new List<double>();
        spawnIndex = 0;
        inputIndex = 0;
    }

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
                if (note == array.Last()) _isLastElement = true;
                if (note == array.First()) _isFirstElement = true;
            }
            
        }
        
    }
    public void Pressed()
    {
        _isPressed = !_isPressed;
    }
    void Update()
    {
        if (spawnIndex < timeStamps.Count)
        {
            if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - 3f && spawnIndex == 0 && _isFirstElement)
            {
                Debug.Log("Ready?");
                _isFirstElement = false;
            }
            if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime)
            {
                var note = Instantiate(notePrefab, transform);
                notes.Add(note.GetComponent<Note>());
                note.GetComponent<Note>().assignedTime = (float)timeStamps[spawnIndex];
                spawnIndex++;
            } 
        }

            if (inputIndex < timeStamps.Count)
            {
                double timeStamp = timeStamps[inputIndex];
                double marginOfError = SongManager.Instance.marginOfError;
                double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

                if (Input.GetKeyDown(input) || _isPressed)
                {
                    _isPressed = !_isPressed;
                    if (Math.Abs(audioTime - timeStamp) < marginOfError)
                    {
                        Hit();
                        //print($"Hit on {inputIndex} note");
                        Destroy(notes[inputIndex].gameObject);
                        inputIndex++;
                    }
                    else
                    {
                        //print($"Hit inaccurate on {inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
                    }
                }
                if (timeStamp + marginOfError <= audioTime)
                {
                    Miss();
                    //print($"Missed {inputIndex} note");
                    inputIndex++;
                }
            
            }
        if (inputIndex == timeStamps.Count && _isLastElement)
        {
            _songManager.EndSong();
            _isLastElement = false;
        }
    }

    
    private void Hit()
    {
        ScoreManager.Hit();
    }
    private void Miss()
    {
        ScoreManager.Miss();
    }
}
