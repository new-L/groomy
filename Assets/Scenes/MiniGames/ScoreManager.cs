using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public AudioSource hitSFX;
    public AudioSource missSFX;
    public TMPro.TextMeshPro scoreText;
    private static int _comboScore;

    public static int ComboScore { get => _comboScore; }

    void Start()
    {
        Instance = this;
        _comboScore = 0;
    }
    public static void Hit()
    {
        _comboScore += 1;
        Instance.hitSFX.Play();
    }
    public static void Miss()
    {
        _comboScore -= 1;
        Instance.missSFX.Play();    
    }
    private void Update()
    {
        scoreText.text = ComboScore.ToString();
    }
}
