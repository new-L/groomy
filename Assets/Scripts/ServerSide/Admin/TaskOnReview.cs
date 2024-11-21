using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class TaskOnReview : MonoBehaviour
{
    [SerializeField] private TMP_Text _info;
    /*������� �����*/
    [SerializeField] private RectTransform content;

    /*������ ������*/
    [SerializeField] private RectTransform prefab;
    [SerializeField] private GameObject _completedTaskPanel;
    [SerializeField] private GameObject _completedTaskInfoPanel;
    [SerializeField] private ReviewTaskServer _server;


    private UsersCompletedTasks _currentTask;

    public GameObject CompletedTaskPanel { get => _completedTaskPanel; set => _completedTaskPanel = value; }


    public void SetTasksList()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        foreach (var model in ReviewTaskServer.UserTaskOnReview)
        {
            var instance = GameObject.Instantiate(prefab.gameObject) as GameObject;
            instance.transform.SetParent(content, false);
            InitializeItemView(instance.GetComponent<AdminTaskOnReviewPrefab>(), model);
        }
    }

    public void SetTaskInfo(UsersCompletedTasks completedTask)
    {
        _currentTask = completedTask;
        _completedTaskInfoPanel.SetActive(true);
        _info.text = "id: " + completedTask.task_id +
            "\n\nUserID: " + completedTask.user_id +
            "\n\n�����������: " + completedTask.username +
            "\n\n��������: " + completedTask.title +
            "\n\n��������: " + completedTask.description +
            "\n\n�������: " + completedTask.reward +
            "\n\n���� ����������: " + completedTask.complete_day;
    }

    public void SendUpdatedInfo(string answer)
    {
        _server.UpdateTask(answer, _currentTask);
    }

    //��������� �������� � ��������� ������
    private void InitializeItemView(AdminTaskOnReviewPrefab viewGameObject, UsersCompletedTasks completedTask)
    {
        viewGameObject.Title.text = completedTask.title;
        viewGameObject.CompleteTaskInfo.text = "#" + completedTask.task_id + " | " + completedTask.username;
        viewGameObject.UserCompletedTask = completedTask;
        viewGameObject.UserCompletedTaskPanel = this;
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
