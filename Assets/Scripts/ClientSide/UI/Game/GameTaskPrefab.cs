using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTaskPrefab : MonoBehaviour
{

    [Header("Text Elements")]
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _rewardCount;

    [Header("Button elements")]
    [SerializeField] private Button _complete;

    public TMP_Text Title { get => _title; set => _title = value; }
    public TMP_Text Description { get => _description; set => _description = value; }
    public TMP_Text RewardCount { get => _rewardCount; set => _rewardCount = value; }
    public Button Complete { get => _complete; set => _complete = value; }
}
