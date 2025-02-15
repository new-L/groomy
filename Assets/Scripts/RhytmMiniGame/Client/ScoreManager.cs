using Melanchall.DryWetMidi.Interaction;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public AudioSource hitSFX;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _actionText;
    [SerializeField] private Animator _scoreTextAnimator;
    private static int _comboScore;
    private int _actionScore;
    private bool _isHitted;

    [SerializeField] private List<string> _actionTextList;

    public static int ComboScore { get => _comboScore; }

    public void Clear()
    {
        Instance = this;
        _actionScore = 0;
        _comboScore = 0;
        SetScoreText();
    }
    public void Hit()
    {
   //     _actionScore += 1;
        _comboScore += 1;
        Instance.hitSFX.Play();
        _isHitted = true;
  //      ActionScore();
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

    //private void ActionScore()
    //{
    //    if (_actionTextList.Count == 0 || _actionScore < 10) return;
    //    _actionText.text = _actionTextList[Random.Range(0, _actionTextList.Count)];
    //    _actionText.gameObject.SetActive(true);
    //    _actionScore = 0;
    //}

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
