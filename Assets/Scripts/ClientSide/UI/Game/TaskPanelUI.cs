using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TaskPanelUI : MonoBehaviour
{
    /*Контент задач*/
    [SerializeField] private RectTransform content;

    /*Префаб задачи*/
    [SerializeField] private RectTransform prefab;
    [SerializeField] private GameTaskPrefab _prefabUIElements;

    /*Панель редактирования задачи*/
    //[SerializeField] private GameObject _taskEditPanel;
    //[SerializeField] private GameObject _scheduleTaskPanel;

    [SerializeField] private TaskParse _task;

    [Header("Loader")]
    [SerializeField] private InGameLoader _inGameLoader;
    [SerializeField] private GameObject _loader;
    [SerializeField] private GameObject _zeroTaskInformation;


    public void SetUpDailyTask()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        _inGameLoader.SetLoader(_loader);
        _zeroTaskInformation.SetActive(false);
        InGameLoader.IsBorryActivate = false;
        Actions.OnStartLoad?.Invoke();       
        _task.GetTaskByTableName(DBTablesName.TasksTable, URLs.TaskSettings); 
    }

    private void StartLoadDailyTask()
    {
        _task.GetTaskByTableName(DBTablesName.Dailytasks, URLs.DailyTask);
    }

    private void SetTasksList()
    {
        if (TaskParse.DailyTasks.Length == 0) 
        { 
            _zeroTaskInformation.SetActive(true);
            Actions.OnUserDatasLoad?.Invoke();            
            return; 
        }
        foreach (var model in TaskParse.DailyTasks)
        {
            foreach (var item in TaskParse.Tasks)
            {
                if (item.task_id == model.task_id)
                {
                    var instance = GameObject.Instantiate(prefab.gameObject) as GameObject;
                    instance.transform.SetParent(content, false);
                    InitializeItemView(instance.GetComponent<GameTaskPrefab>(), item);
                    break;
                }
            }
            
        }
        Actions.OnUserDatasLoad?.Invoke();        
    }


    //Отрисовка префабов и установка данных
    private void InitializeItemView(GameTaskPrefab viewGameObject, Tasks tasks)
    {
        viewGameObject.Title.text = tasks.title;
        viewGameObject.Description.text = tasks.description;
        viewGameObject.RewardCount.text = tasks.reward.ToString();
    }

    private void OnEnable()
    {
        Actions.OnTaskSettingsLoad += SetTasksList;
        Actions.OnTaskListLoaded += StartLoadDailyTask;
    }

    private void OnDisable()
    {
        Actions.OnTaskSettingsLoad -= SetTasksList;
        Actions.OnTaskListLoaded -= StartLoadDailyTask;
    }
}
