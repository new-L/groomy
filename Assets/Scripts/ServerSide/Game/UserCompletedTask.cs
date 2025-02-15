using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class UserCompletedTask : MonoBehaviour
{
    [SerializeField] private UnityEvent _taskCreated;
    [SerializeField] private Notification _notification;
    [SerializeField] private RectTransform _completedTaskPrefab;
    [SerializeField] private TaskPanelUI _taskPanel; 
    private static CompletedTasks[] _completedTasks;

    private string _json = "";

    public static CompletedTasks[] CompletedTasks { get => _completedTasks; set => _completedTasks = value; }
    public RectTransform CompletedTaskPrefab { get => _completedTaskPrefab; set => _completedTaskPrefab = value; }

    public void TaskComplete(int taskID)
    {
        foreach (var task in TaskParse.DailyTasks)
        {
            if(taskID == task.task_id)
            {
                StartCoroutine(SendCompletedTask(taskID, task.availableonday, "create"));
                break;
            }
        }
    }

    public void GetCompletedTask()
    {
        StartCoroutine(SendCompletedTask(0, "null", "get"));
    }

    public bool CompletedTaskType(Tasks tasks, CompletedTasks completed, string response)
    {
        if (tasks.task_id == completed.task_id) {
            switch (response)
            {
                case "completed":
                    {
                        if (completed.is_complete == true && completed.is_on_check == false)
                            return true;
                        break;
                    }
                case "rejected":
                    {
                        if (completed.is_complete == false && completed.is_on_check == false)
                            return true;
                        break;
                    }
                case "review":
                    {
                        if (completed.is_complete == false && completed.is_on_check == true)
                            return true;
                        break;
                    }
                default:
                    return false;
            }
        }
        return false;
    }

    private IEnumerator SendCompletedTask(int taskID, string completedDay, string responseType)
    {
        WWWForm form = new WWWForm();
        form.AddField("response_type", responseType);
        form.AddField("user_id", User.Player.user_id);
        form.AddField("task_id", taskID);
        form.AddField("complete_day", completedDay);
        UnityWebRequest www = UnityWebRequest.Post(URLs.UserCompletedTask, form);

        www.timeout = ServerSettings.TimeOut;

        yield return www.SendWebRequest();
        if (www.error != null) { Debug.Log("Не удалось связаться с сервером!"); yield break; }

        
        print("Response Type: " + responseType);
        if (responseType.Equals("get"))
        {
            _json = JsonHelper.fixJson(www.downloadHandler.text);
            _completedTasks = JsonHelper.FromJson<CompletedTasks>(_json);
            Actions.OnTaskSettingsLoad?.Invoke();
        }
        else if(responseType.Equals("create")) 
        {
            _taskPanel.SetUpDailyTask();
            _notification.Set(NotificationType.Accept, "Задание отправлено на проверку");
            _notification.Play();
            _taskCreated?.Invoke();
        }
        
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}

[Serializable]
public class CompletedTasks
{
    public int task_id;
    public bool is_complete;
    public bool is_on_check;
    public bool is_reward_received;
    public string complete_day;
}
