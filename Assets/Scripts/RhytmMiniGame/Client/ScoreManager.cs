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

    private int _comboX5Count = 0;
    private int _comboX5Local = 0;
    private int _comboX10Count = 0;
    private int _comboX10Local = 0;

    [SerializeField] private List<string> _actionTextList;

    public static int ComboScore { get => _comboScore; }
    public int ComboX5Count { get => _comboX5Count; private set => _comboX5Count = value; }
    public int ComboX10Count { get => _comboX10Count; private set => _comboX10Count = value; }

    public void Clear()
    {
        Instance = this;
        _actionScore = 0;
        ComboX5Count = 0;
        ComboX10Count = 0;
        _comboX5Local = 0;
        _comboX10Local = 0;
        _comboScore = 0;
        SetScoreText();
    }
    public void Hit()
    {
        _comboScore += 1;
        ComboHits(ref _comboX5Local, 5, ref _comboX5Count);
        ComboHits(ref _comboX10Local, 10, ref _comboX10Count);
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
        ResetComboHits(ref _comboX5Local);
        ResetComboHits(ref _comboX10Local);
        SetScoreText();
    }


    private void ComboHits(ref int combo, int multiplication, ref int comboCount)
    {
        combo += 1;
        if (combo == multiplication)
        {
            combo = 0;
            comboCount += 1;
        }
    }

    private void ResetComboHits(ref int combo)
    {
        combo = 0;
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
