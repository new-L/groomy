using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskDetail : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _rewardCount;
    [SerializeField] private TMP_Text _taskID;
    [SerializeField] private TMP_Text _completedTaskAlert;
    [SerializeField] private Button _sendCompletedTask;

    [Header("Task")]
    [SerializeField] private UserCompletedTask _completedTask;
    [SerializeField] private GameObject _panel;

    private int _taskIDs;

    public void SetTaskDetail(Tasks task, bool isCompletedTaskExist)
    {
        _title.text = task.title;
        _description.text = task.description;
        _rewardCount.text = "+" + task.reward;
        _taskIDs = task.task_id;
        _taskID.text = "#" + _taskIDs.ToString();
        SetCompletedTaskInfo(isCompletedTaskExist);
        _panel.SetActive(true);
    }

    public void TaskComplete()
    {
        _completedTask.TaskComplete(_taskIDs);
        _panel.SetActive(false);
    }

    private void SetCompletedTaskInfo(bool isCompletedTaskExist)
    {
        _sendCompletedTask.interactable = !isCompletedTaskExist;
        _completedTaskAlert.gameObject.SetActive(isCompletedTaskExist);
    }
}
