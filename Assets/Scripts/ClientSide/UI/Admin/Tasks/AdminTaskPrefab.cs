using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using JetBrains.Annotations;
using System.Reflection;
using UnityEngine.Apple.ReplayKit;

public class AdminTaskPrefab : MonoBehaviour
{
    [Header("Text Elements")]
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _rewardCount;
    [SerializeField] private TMP_Text _difficulty;
    [SerializeField] private TMP_Text _availableOnDay;

    [Header("Image Elements")]
    [SerializeField] private Image _rewardImage;

    [Header("On Scene Elements")]
    [SerializeField] private GameObject _taskEditPanel;
    [SerializeField] private GameObject _dailyTaskPanel;

    [Header("Prefab Elements")]
    [SerializeField] private GameObject _deleteButton;
    [SerializeField] private GameObject _deScheduleButton;


    private int _taskID;
    private TypeOfTask _typeOfTaks;


    public TMP_Text Name { get => _name; set => _name = value; }
    public TMP_Text Description { get => _description; set => _description = value; }
    public TMP_Text RewardCount { get => _rewardCount; set => _rewardCount = value; }
    public Image RewardImage { get => _rewardImage; set => _rewardImage = value; }
    public TMP_Text Difficulty { get => _difficulty; set => _difficulty = value; }
    public int TaskID { get => _taskID; set => _taskID = value; }
    public GameObject TaskEditPanel { get => _taskEditPanel; set => _taskEditPanel = value; }
    public GameObject DailyTaskPanel { get => _dailyTaskPanel; set => _dailyTaskPanel = value; }
    public TypeOfTask TypeOfTaks { get => _typeOfTaks; set => _typeOfTaks = value; }
    public TMP_Text AvailableOnDay { get => _availableOnDay; set => _availableOnDay = value; }

    public void EditTask()
    {
        _taskEditPanel.SetActive(true);
        _taskEditPanel.GetComponent<EditTaskPanel>().SetPreviosInformation(_taskID);
    }

    public void ScheduleTask()
    {
        DailyTaskPanel.SetActive(true);
        DailyTaskPanel.GetComponent<DailyTaskPanel>().CurrentTaskID = _taskID;
    }

    public void DeleteTask()
    {
        Task serverSide = GameObject.Find("ServerSide").GetComponent<Task>();
        switch (_typeOfTaks)
        {
            case TypeOfTask.Scheduled: serverSide.Delete(_taskID, URLs.DeScheduleTask, DBTablesName.Dailytasks, true); break;
            case TypeOfTask.Typical: serverSide.Delete(_taskID, URLs.DeleteTask, DBTablesName.TasksTable, true); break;
        }
    }
}

public enum TypeOfTask
{
    Scheduled,
    Typical
}

