using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Task : MonoBehaviour
{
    private static Tasks[] _tasks;
    private static TaskType[] _taskType;
    private static ScheduleTasks[] _scheduleTasks;
    private static TaskDifficulty[] _taskDifficulty;
    
    [SerializeField] private List<string> _tablesNames;

    private string _json = "";
    

    #region Get/Set
    public static Tasks[] Tasks { get => _tasks; set => _tasks = value; }
    public static TaskDifficulty[] TaskDifficulty { get => _taskDifficulty; set => _taskDifficulty = value; }
    public static TaskType[] TaskType { get => _taskType; set => _taskType = value; }
    public static ScheduleTasks[] ScheduleTasks { get => _scheduleTasks; set => _scheduleTasks = value; }
    #endregion

    public void StartLoad()
    {
        Loader.LoadedTables.Clear();
        GetTablesData();
    }
    private void GetTablesData()
    {
        Actions.OnStartLoad?.Invoke();
        foreach (var item in _tablesNames)
        {
            Loader.LoadedTables.Add(item, false);
            if (item.Equals(_tablesNames.Last())) GetList(item, true);
            else GetList(item, false);
        }
    }

    //Подтягиваем все задачи
    public void GetList(string tableName, bool isLastTable)
    {
        StartCoroutine(GetTaskSettings(tableName, isLastTable));
    }

    public void UpdateTask(Tasks newTask, bool isUpdateTaskListNeeded)
    {
        Actions.OnStartLoad?.Invoke();
        StartCoroutine(NewTask(newTask, URLs.UpdateTask, isUpdateTaskListNeeded));
    }

    public void NewTask(Tasks newTask, bool isUpdateTaskListNeeded)
    {
        Actions.OnStartLoad?.Invoke();
        StartCoroutine(NewTask(newTask, URLs.NewTask, isUpdateTaskListNeeded));
    }

    public void Delete(int taskID, string URL, string tableName, bool isUpdateTaskListNeeded)
    {
        Actions.OnStartLoad?.Invoke();
        StartCoroutine(DeleteTask(taskID, URL, tableName, isUpdateTaskListNeeded));
    }


    private IEnumerator GetTaskSettings(string tableName, bool isLastTable)
    {
        Loader.IsLoadComplete = false;
        Debug.Log("{GetTaskSettings}: Запрашиваем данные у таблицы: " + tableName);
        WWWForm form = new WWWForm();
        form.AddField("table_name", tableName);
        form.AddField("response_type", "get");
        UnityWebRequest www = UnityWebRequest.Post(URLs.TaskSettings, form);

        www.timeout = ServerSettings.TimeOut;

        yield return www.SendWebRequest();
        if (www.error != null) { Debug.Log("Не удалось связаться с сервером!"); yield break; }
        Debug.Log(isLastTable + " " +www.downloadHandler.text);

        _json = JsonHelper.fixJson(www.downloadHandler.text);

        if (tableName.Equals(DBTablesName.TasksTypeTable))
            TaskType = JsonHelper.FromJson<TaskType>(_json, tableName);
        else if (tableName.Equals(DBTablesName.TasksDifficultyTable))
            TaskDifficulty = JsonHelper.FromJson<TaskDifficulty>(_json, tableName);
        else if (tableName.Equals(DBTablesName.TasksTable))
            Tasks = JsonHelper.FromJson<Tasks>(_json, tableName);
        else if (tableName.Equals(DBTablesName.Dailytasks))
            ScheduleTasks = JsonHelper.FromJson<ScheduleTasks>(_json, tableName);
        if (isLastTable)
        {
            Loader.IsLoadComplete = true;
        }
    }

    private IEnumerator NewTask(Tasks newTask, string URL, bool isUpdateTaskListNeeded)
    {
        Loader.IsLoadComplete = false;
        Debug.Log("{NewTask}: Обновляем или добавляем задачу...");
        var bytes = Encoding.UTF8.GetBytes(ParamentrsToJSON(newTask));//TODO: передать корректные значения
        using (var webRequest = new UnityWebRequest(URL, "POST"))
        {
            webRequest.uploadHandler = new UploadHandlerRaw(bytes);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("accept", "application/json");
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();
            if (isUpdateTaskListNeeded) StartCoroutine(GetTaskSettings(DBTablesName.TasksTable, true));

            Debug.Log(webRequest.downloadHandler.text);
        }
    }

    private IEnumerator DeleteTask(int taskID, string URL, string tableName, bool isUpdateTaskListNeeded)
    {
        Loader.IsLoadComplete = false;
        WWWForm form = new WWWForm();
        
        form.AddField("task_id", taskID);
        UnityWebRequest www = UnityWebRequest.Post(URL, form);

        www.timeout = ServerSettings.TimeOut;

        yield return www.SendWebRequest();
        if (www.error != null) { Debug.Log("Не удалось связаться с сервером!"); yield break; }
        Debug.Log(www.downloadHandler.text);
        if (isUpdateTaskListNeeded) StartCoroutine(GetTaskSettings(tableName, true));
        Debug.Log("Удаление завершено");
    }
    private string ParamentrsToJSON(Tasks newTask)
    {
        return JsonHelper.fixJson("[" + JsonUtility.ToJson(newTask).ToString() + "]");
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}

#region Classes
[Serializable]
public class TaskType
{
    public string type;
}

[Serializable]
public class TaskDifficulty
{
    public string difficulty;
}

[Serializable]
public class Tasks
{
    public int task_id;
    public string title;
    public string description;
    public string difficulty;
    public string type;
    public int reward;
}

[Serializable]
public class ScheduleTasks
{
    public int task_id;
    public string availableonday;
}
#endregion
