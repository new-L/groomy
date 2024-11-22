using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class TaskOnReview : MonoBehaviour
{
    [SerializeField] private TMP_Text _info;
    /*Контент задач*/
    [SerializeField] private RectTransform content;

    /*Префаб задачи*/
    [SerializeField] private RectTransform _prefab;
    [SerializeField] private GameObject _completedTaskPanel;
    [SerializeField] private GameObject _completedTaskInfoPanel;
    [SerializeField] private ReviewTaskServer _server;
    [SerializeField] private ListType _listType;
    [SerializeField] private List<int> userIDs = new List<int>();

    private UsersCompletedTasks _currentTask;

    public GameObject CompletedTaskPanel { get => _completedTaskPanel; set => _completedTaskPanel = value; }
    public ListType ListType { get => _listType; set => _listType = value; }


    public void SetTasksList(ListType type, int ID = 0)
    {
        _listType = type;
        Clear();
        
        foreach (var model in ReviewTaskServer.UserTaskOnReview)
        {
            if (type == ListType.UserDetailed && ID == model.user_id)
            {
                var instance = GameObject.Instantiate(_prefab.gameObject) as GameObject;
                instance.transform.SetParent(content, false);
                InitializeItemView(instance.GetComponent<AdminTaskOnReviewPrefab>(), model);
            }
            else if(type == ListType.Tasks)
            {
                var instance = GameObject.Instantiate(_prefab.gameObject) as GameObject;
                instance.transform.SetParent(content, false);
                InitializeItemView(instance.GetComponent<AdminTaskOnReviewPrefab>(), model);
            }
        }
    }

    public void SetUsersList()
    {
        _listType = ListType.User;
        Clear();
        GetUniqueUserIDs();
        foreach (var item in userIDs)
        {
            foreach (var model in ReviewTaskServer.UserTaskOnReview)
            {
                if (model.user_id == item)
                {
                    var instance = GameObject.Instantiate(_prefab.gameObject) as GameObject;
                    instance.transform.SetParent(content, false);
                    InitializeItemView(instance.GetComponent<AdminTaskOnReviewPrefab>(), model);
                    break;
                }
            }
        }
    }

    public void SetTaskInfo(UsersCompletedTasks completedTask)
    {
        _currentTask = completedTask;
        _completedTaskInfoPanel.SetActive(true);
        _info.text = "id: " + completedTask.task_id +
            "\n\nUserID: " + completedTask.user_id +
            "\n\nВыполнивший: " + completedTask.username +
            "\n\nНазвание: " + completedTask.title +
            "\n\nОписание: " + completedTask.description +
            "\n\nНаграда: " + completedTask.reward +
            "\n\nДень выполнения: " + completedTask.complete_day;
    }

    public void SendUpdatedInfo(string answer)
    {
        _server.UpdateTask(answer, _currentTask);
    }

    //Отрисовка префабов и установка данных
    private void InitializeItemView(AdminTaskOnReviewPrefab viewGameObject, UsersCompletedTasks completedTask)
    {
        if (_listType == ListType.Tasks || _listType == ListType.UserDetailed)
        {
            viewGameObject.Title.text = completedTask.title;
            viewGameObject.CompleteTaskInfo.text = "#" + completedTask.task_id + " | " + completedTask.username;  
        }
        else if(_listType == ListType.User)
        {
            viewGameObject.Title.text = completedTask.username;
            viewGameObject.CompleteTaskInfo.text = "#" + completedTask.user_id;
        }
        viewGameObject.UserID = completedTask.user_id;
        viewGameObject.Type = _listType;
        viewGameObject.UserCompletedTask = completedTask;
        viewGameObject.UserCompletedTaskPanel = this;
        
    }
    private void GetUniqueUserIDs()
    {
        foreach (var model in ReviewTaskServer.UserTaskOnReview)
        {
            if(!userIDs.Contains(model.user_id))
                userIDs.Add(model.user_id);
        }
    }
    private void Clear()
    {
        userIDs.Clear();
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }

}

[Serializable]
public class UsersCompletedTasks
{
    public int id;
    public int user_id;
    public string username;
    public int task_id;
    public string title;
    public string description;
    public int reward;
    public string complete_day;
}

public enum ListType
{
    User,
    UserDetailed,
    Tasks
}
