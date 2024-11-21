using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AdminTaskOnReviewPrefab : MonoBehaviour
{


    [Header("Text")]
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _completeTaskInfo;
    [SerializeField] private UsersCompletedTasks _userCompletedTask;
    [SerializeField] private TaskOnReview _userCompletedTaskPanel;

    public TMP_Text Title { get => _title; set => _title = value; }
    public TMP_Text CompleteTaskInfo { get => _completeTaskInfo; set => _completeTaskInfo = value; }
    public UsersCompletedTasks UserCompletedTask { get => _userCompletedTask; set => _userCompletedTask = value; }
    public TaskOnReview UserCompletedTaskPanel { get => _userCompletedTaskPanel; set => _userCompletedTaskPanel = value; }

    public void OpenTaskInfoPanel()
    {
        _userCompletedTaskPanel.SetTaskInfo(_userCompletedTask);
    }
}
