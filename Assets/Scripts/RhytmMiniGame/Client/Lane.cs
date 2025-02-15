﻿using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private UnityEvent OnFirstNoteReeady;
    [SerializeField] private AudioSource _hit;
    [SerializeField] private AudioSource _miss;
    [SerializeField] private SpriteRenderer _laneEffect;
    [SerializeField] private List<Sprite> _laneEffects;

    [SerializeField] private ParticleSystem _hitParticle;


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
            if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - 2.5f && spawnIndex == 0 && _isFirstElement)
            {
                OnFirstNoteReeady?.Invoke();
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
                    Pressed();
                    if (Math.Abs(audioTime - timeStamp) < marginOfError)
                    {
                        Hit();
                        //print($"Hit on {inputIndex} note");
                        Destroy(notes[inputIndex].gameObject);
                        inputIndex++;
                    }
                    else
                    {
                    //Paint(58, 58, 58, 255);
                    //print($"Hit inaccurate on {inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
                }
                }
                if (timeStamp + marginOfError <= audioTime)
                {
                Debug.Log(notes[inputIndex].gameObject.transform.position);
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
        _scoreManager.Hit();
        _hit.Play();
        _hitParticle.Play();
        Paint(0, 221, 22, 185);
    }
    private void Miss()
    {
        _hitParticle.Stop();
        _scoreManager.Miss();
        if (!_hit.isPlaying) _miss.Play();
        _laneEffect.gameObject.GetComponent<Animator>().StopPlayback();
        _laneEffect.gameObject.GetComponent<Animator>().Play("HitOut");
        Paint(222, 0, 0, 185);
    }

    private void Paint(byte r, byte g, byte b, byte a)
    {
        _laneEffect.sprite = _laneEffects[UnityEngine.Random.Range(0, _laneEffects.Count - 1)];
        _laneEffect.gameObject.SetActive(true);
        _laneEffect.color = new Color32(r, g, b, a);
    }
}
