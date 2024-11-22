using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskPanelUI : MonoBehaviour
{
    /*Контент задач*/
    [SerializeField] private RectTransform content;
    [SerializeField] private ScrollRect _scroll;

    /*Префаб задачи*/
    [SerializeField] private RectTransform prefab;
    [SerializeField] private GameTaskPrefab _prefabUIElements;

    [SerializeField] private TaskParse _task;
    [SerializeField] private UserCompletedTask _userCompletedTask;

    [Header("Loader")]
    [SerializeField] private InGameLoader _inGameLoader;
    [SerializeField] private GameObject _loader;
    [SerializeField] private TMP_Text _zeroTaskInformation;

    public void SetUpDailyTask()
    {
        ClearListView();
        _inGameLoader.SetLoader(_loader);
        
        InGameLoader.IsBorryActivate = false;
        Actions.OnStartLoad?.Invoke();       
        _task.GetTaskByTableName(DBTablesName.TasksTable, URLs.TaskSettings); 
    }

    private void StartLoadDailyTask()
    {
        _task.GetTaskByTableName(DBTablesName.Dailytasks, URLs.DailyTask);
    }

    private void StartLoadCompletedTask()
    {
        _userCompletedTask.GetCompletedTask();
    }

    public void SetTasksList()
    {
        ClearListView();
        if (TaskParse.DailyTasks.Length == 0) 
        {
            _zeroTaskInformation.text = "Задач нет!";
            _zeroTaskInformation.gameObject.SetActive(true);
            Actions.OnUserDatasLoad?.Invoke();            
            return; 
        }
        _scroll.vertical = false;
        bool isTaskCompleted = isDailyTaskCompleted();
        foreach (var model in TaskParse.DailyTasks)
        {
            foreach (var item in TaskParse.Tasks)
            {
                if (item.task_id == model.task_id)
                {
                    var instance = GameObject.Instantiate(prefab.gameObject) as GameObject;
                    instance.GetComponent<GameTaskPrefab>().TaskDeactivate(isTaskCompleted);
                    instance.transform.SetParent(content, false);
                    InitializeItemView(instance.GetComponent<GameTaskPrefab>(), item);                    
                    break;
                }
            }
            
        }
        Actions.OnUserDatasLoad?.Invoke();        
    }
    public void SetCompletedTasksList(string response)
    {
        ClearListView();
        if (UserCompletedTask.CompletedTasks == null || UserCompletedTask.CompletedTasks.Length == 0)
        {
            _zeroTaskInformation.text = "Задач нет!";
            _zeroTaskInformation.gameObject.SetActive(true);
            return;
        }               
        _scroll.vertical = true;
        foreach (var model in UserCompletedTask.CompletedTasks)
        {
            foreach (var item in TaskParse.Tasks)
            {
                if (_userCompletedTask.CompletedTaskType(item, model, response))
                {
                    var instance = GameObject.Instantiate(_userCompletedTask.CompletedTaskPrefab.gameObject) as GameObject;
                    instance.GetComponent<GameTaskPrefab>().SetCompletedTaskSettings(response);
                    instance.transform.SetParent(content, false);
                    InitializeItemView(instance.GetComponent<GameTaskPrefab>(), item);
                    break;
                }
            }
        }
    }

    private bool isDailyTaskCompleted()
    {
        if (UserCompletedTask.CompletedTasks == null || UserCompletedTask.CompletedTasks.Length == 0)
        {
            return false;
        }
        foreach (var model in TaskParse.DailyTasks)
        {
            foreach (var item in UserCompletedTask.CompletedTasks)
            {
                if (model.availableonday.Equals(item.complete_day) && model.task_id == item.task_id)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void ClearListView()
    {
        _zeroTaskInformation.gameObject.SetActive(false);
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }

    //Отрисовка префабов и установка данных
    private void InitializeItemView(GameTaskPrefab viewGameObject, Tasks tasks)
    {
        viewGameObject.Title.text = tasks.title;
        viewGameObject.Description.text = tasks.description;
        viewGameObject.RewardCount.text = "+"+tasks.reward.ToString();
        viewGameObject.TaskID.text = "#"+tasks.task_id.ToString();
        viewGameObject.UserCompletedTask = _userCompletedTask;
    }

    private void OnEnable()
    {
        Actions.OnTaskSettingsLoad += SetTasksList;
        Actions.OnTaskListLoaded += StartLoadDailyTask;
        Actions.OnDailyTaskLoad += StartLoadCompletedTask;
    }

    private void OnDisable()
    {
        Actions.OnTaskSettingsLoad -= SetTasksList;
        Actions.OnTaskListLoaded -= StartLoadDailyTask;
        Actions.OnDailyTaskLoad -= StartLoadCompletedTask;
    }
}
