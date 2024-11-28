using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTaskPrefab : MonoBehaviour
{
    [Header("Image Elements")]
    [SerializeField] private Image _background;
    [SerializeField] private Image _completeStatus;

    [Header("Text Elements")]
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _rewardCount;
    [SerializeField] private TMP_Text _description;

    [Header("Button Elements")]
    [SerializeField] private Button _detail;

    [Header("InGameComponents")]
    [SerializeField] private UserCompletedTask _userCompletedTask;
    [SerializeField] private TaskDetail _taskDetail;
    [SerializeField] private Tasks _task;

    private bool isCompletedTaskExist;
    private string _taskStatusImagePATH = "Art/UI/CompletedTaskStatus/";

    public TMP_Text Title { get => _title; set => _title = value; }
    public TMP_Text RewardCount { get => _rewardCount; set => _rewardCount = value; }
    public UserCompletedTask UserCompletedTask { get => _userCompletedTask; set => _userCompletedTask = value; }
    public TaskDetail TaskDetail { get => _taskDetail; set => _taskDetail = value; }
    public Tasks Task { get => _task; set => _task = value; }
    public TMP_Text Description { get => _description; set => _description = value; }
    public bool IsCompletedTaskExist { get => isCompletedTaskExist; set => isCompletedTaskExist = value; }

    public void TaskDetails()
    {
        //_userCompletedTask.TaskComplete(RemoveCharacter(_taskID.text, 0, 1));
        _taskDetail.SetTaskDetail(_task, isCompletedTaskExist);
    }

    public void TaskDeactivate(bool isCompletedTaskExist)
    {
        //if (isCompletedTaskExist)
        //{
        //    _complete.interactable = false;
        //    _background.color = new Color32(255, 255, 255, 128);
        //}
        //else
        //{
        //    _complete.interactable = true;
        //    _background.color = new Color32(255, 255, 255, 255);
        //}
        _detail.interactable = !isCompletedTaskExist;

    }

    public void SetCompletedTaskSettings(string response)
    {
        _completeStatus.sprite = Resources.Load<Sprite>(_taskStatusImagePATH + response);
    }

    //private int RemoveCharacter(string text, int index, int length) => Int32.Parse(text.Remove(index, length));
}