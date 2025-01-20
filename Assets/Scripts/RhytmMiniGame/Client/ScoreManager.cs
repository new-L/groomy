using Melanchall.DryWetMidi.Interaction;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public AudioSource hitSFX;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Animator _scoreTextAnimator;
    private static int _comboScore;
    private bool _isHitted;

    public static int ComboScore { get => _comboScore; }

    public void Clear()
    {
        Instance = this;
        _comboScore = 0;
        SetScoreText();
    }
    public void Hit()
    {
        _comboScore += 1;
        Instance.hitSFX.Play();
        _isHitted = true;
        Animation();
        SetScoreText();
    }
    public void ResetAnimation()
    {
        _isHitted = false;
        Animation();
    }
    public void Miss()
    {
        _isHitted = false;
        _comboScore -= CheckScoreUnderNull(1);
        SetScoreText();
    }
    private void Animation()
    {
        _scoreTextAnimator.SetBool("isHitted", _isHitted);
        _isHitted = false;
    }

    private void Start()
    {
        Clear();
    }

    private int CheckScoreUnderNull(int score) => _comboScore - score  < 0 ?  0 : score;

    private void SetScoreText()
    {
        _scoreText.text = _comboScore.ToString();
    }
}
