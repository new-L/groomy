using System.Collections;
using UnityEngine;
using System;
using TMPro;
using System.Text;
using UnityEngine.Networking;

public class DailyTaskPanel : MonoBehaviour
{

    [SerializeField] private TMP_InputField _dateInputField;
    [SerializeField] private Task _task;
    private int _currentTaskID;

    public int CurrentTaskID { get => _currentTaskID; set => _currentTaskID = value; }

    public void Schedule()
    {
        ScheduleTasks scheduledTask = null;
        if(Validate(_dateInputField.text))
        {
            scheduledTask = new ScheduleTasks
            {
                task_id = _currentTaskID,
                availableonday = _dateInputField.text
            };
            StartCoroutine(ScheduleTask(scheduledTask, URLs.ScheduleTask, true));
        }
        else
        {
            Debug.Log("Date: " + _dateInputField.text + " не прошла валидация! Текст должен быть в формате ##.##.####");
        }
    }

    private bool Validate(string date)
    {
        if (date.Equals("")) return false;
        DateTime value;
        if (DateTime.TryParse(date, out value))
        {
            //Чекаем валидацию даты на мать
            return true;
        }
        else
        {
            //TODO: Вызвать ошибку
            return false;
        }
        //Debug.Log(date1.ToShortDateString());
    }

    private IEnumerator ScheduleTask(ScheduleTasks task, string URL, bool isUpdateTaskListNeeded)
    {
        Debug.Log("{NewTask}: Обновляем или добавляем задачу...");
        var bytes = Encoding.UTF8.GetBytes(ParamentrsToJSON(task));//TODO: передать корректные значения
        using (var webRequest = new UnityWebRequest(URL, "POST"))
        {
            webRequest.uploadHandler = new UploadHandlerRaw(bytes);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("accept", "application/json");
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();
            if (isUpdateTaskListNeeded) _task.GetList(DBTablesName.Dailytasks, true);

            Debug.Log(webRequest.downloadHandler.text);
        }
    }


    private string ParamentrsToJSON(ScheduleTasks task)
    {
        return JsonHelper.fixJson("[" + JsonUtility.ToJson(task).ToString() + "]");
    }

    private void OnDisable()
    {
        StopCoroutine(nameof(ScheduleTask));
    }
}
