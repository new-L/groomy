using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

public class TaskParse : MonoBehaviour
{
    private string _json = "";
    [SerializeField] private Task _task;

    private static Tasks[] _tasks;
    private static ScheduleTasks[] _dailyTasks;


    #region Get/Set
    public static Tasks[] Tasks { get => _tasks; set => _tasks = value; }
    public static ScheduleTasks[] DailyTasks { get => _dailyTasks; set => _dailyTasks = value; }
    #endregion
    public void GetTaskByTableName(string name, string URL)
    {
        StartCoroutine(GetTaskSettings(name, URL));
    }

    private IEnumerator GetTaskSettings(string tableName, string URL)
    {
        Debug.Log("{GetTaskSettings}: Запрашиваем данные у таблицы: " + tableName);
        WWWForm form = new WWWForm();
        form.AddField("table_name", tableName);
        form.AddField("response_type", "get");
        UnityWebRequest www = UnityWebRequest.Post(URL, form);

        www.timeout = ServerSettings.TimeOut;

        yield return www.SendWebRequest();
        if (www.error != null) { Debug.Log("Не удалось связаться с сервером!"); yield break; }
        Debug.Log(www.downloadHandler.text);
        _json = JsonHelper.fixJson(www.downloadHandler.text);


        if (tableName.Equals(DBTablesName.TasksTable))
        {
            Tasks = JsonHelper.FromJson<Tasks>(_json, tableName);
            Actions.OnTaskListLoaded?.Invoke();
        }
        else if (tableName.Equals(DBTablesName.Dailytasks))
        {
            DailyTasks = JsonHelper.FromJson<ScheduleTasks>(_json, tableName);
            InGameLoader.IsBorryActivate = false;
            Actions.OnTaskSettingsLoad?.Invoke();
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
