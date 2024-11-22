using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ReviewTaskServer : MonoBehaviour
{
    [SerializeField] TaskOnReview _taskOnReview;
    private string _json;
    private static UsersCompletedTasks[] _userTaskOnReview;
    public static UsersCompletedTasks[] UserTaskOnReview { get => _userTaskOnReview; set => _userTaskOnReview = value; }
    
    public void GetTask()
    {
        StartCoroutine(nameof(GetTaskOnReview));
    }

    public void UpdateTask(string responseType, UsersCompletedTasks completedTask)
    {
        StartCoroutine(UpdateCompletedTaskInfo(responseType, completedTask));
    }

    private IEnumerator GetTaskOnReview()
    {
        WWWForm form = new WWWForm();
        form.AddField("response_type", "get");
        UnityWebRequest www = UnityWebRequest.Post(URLs.TaskOnReview, form);

        www.timeout = ServerSettings.TimeOut;

        yield return www.SendWebRequest();
        if (www.error != null) { Debug.Log("Не удалось связаться с сервером! " + www.error); yield break; }
        else
        {
            Debug.Log(www.downloadHandler.text);
            
            _json = JsonHelper.fixJson(www.downloadHandler.text);
            UserTaskOnReview = JsonHelper.FromJson<UsersCompletedTasks>(_json);
            _taskOnReview.CompletedTaskPanel.SetActive(true);
            _taskOnReview.SetTasksList(ListType.Tasks);
        }
    }

    private IEnumerator UpdateCompletedTaskInfo(string responseType, UsersCompletedTasks completedTask)
    {
        WWWForm form = new WWWForm();
        form.AddField("response_type", responseType);
        form.AddField("id", completedTask.id);
        if (responseType.ToLower().Equals("accept"))
        {
            form.AddField("user_id", completedTask.user_id);
            form.AddField("task_id", completedTask.task_id);
        }
        UnityWebRequest www = UnityWebRequest.Post(URLs.TaskOnReview, form);

        www.timeout = ServerSettings.TimeOut;

        yield return www.SendWebRequest();
        if (www.error != null) { Debug.Log("Не удалось связаться с сервером! " + www.error); yield break; }
        else
        {
            GetTask();
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
