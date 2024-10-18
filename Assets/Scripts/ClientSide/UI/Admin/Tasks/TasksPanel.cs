using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TasksPanel : MonoBehaviour
{
    /*������� �����*/
    [SerializeField] private RectTransform content;

    /*������ ������*/
    [SerializeField] private RectTransform prefab;
    [SerializeField] private AdminTaskPrefab _prefabUIElements;

    /*������ �������������� ������*/
    [SerializeField] private GameObject _taskEditPanel;
    [SerializeField] private GameObject _scheduleTaskPanel;



    public void SetTasksList()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        foreach (var model in Task.Tasks)
        {
            var instance = GameObject.Instantiate(prefab.gameObject) as GameObject;
            instance.transform.SetParent(content, false);
            InitializeItemView(instance.GetComponent<AdminTaskPrefab>(), model, TypeOfTask.Typical) ;
        }
        Actions.OnListCreated?.Invoke();
    }

    public void SetPlannedTasks()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        foreach (var plannedItem in Task.ScheduleTasks)
        {
            foreach (var model in Task.Tasks)
            {
                if (plannedItem.task_id == model.task_id)
                {
                    var instance = GameObject.Instantiate(prefab.gameObject) as GameObject;
                    instance.transform.SetParent(content, false);
                    InitializeItemView(instance.GetComponent<AdminTaskPrefab>(), model, TypeOfTask.Scheduled);
                    instance.GetComponent<AdminTaskPrefab>().AvailableOnDay.text = plannedItem.availableonday;
                    break;
                }
            }
        }
        Actions.OnListCreated?.Invoke();
    }


    //��������� �������� � ��������� ������
    private void InitializeItemView(AdminTaskPrefab viewGameObject, Tasks tasks, TypeOfTask typeOfTask)
    {
        viewGameObject.Name.text = tasks.title;
        viewGameObject.Description.text = tasks.description;
        viewGameObject.Difficulty.text = tasks.difficulty;
        viewGameObject.RewardCount.text = tasks.reward.ToString();
        viewGameObject.TaskID = tasks.task_id;
        viewGameObject.TaskEditPanel = _taskEditPanel;
        viewGameObject.DailyTaskPanel = _scheduleTaskPanel;
        viewGameObject.TypeOfTaks = typeOfTask;
    }

    private void OnEnable()
    {
        Actions.OnTaskSettingsLoad += SetPlannedTasks;
        Actions.OnTaskSettingsLoad += SetTasksList;        
    }

    private void OnDisable()
    {
        Actions.OnTaskSettingsLoad -= SetPlannedTasks;
        Actions.OnTaskSettingsLoad -= SetTasksList;
    }
}
