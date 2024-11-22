using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AdminTaskOnReviewPrefab : MonoBehaviour
{


    [Header("Text")]
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _completeTaskInfo;
    [SerializeField] private UsersCompletedTasks _userCompletedTask;
    [SerializeField] private TaskOnReview _userCompletedTaskPanel;
    [SerializeField] private ListType _type;

    [SerializeField] private int userID = 0;

    public TMP_Text Title { get => _title; set => _title = value; }
    public TMP_Text CompleteTaskInfo { get => _completeTaskInfo; set => _completeTaskInfo = value; }
    public UsersCompletedTasks UserCompletedTask { get => _userCompletedTask; set => _userCompletedTask = value; }
    public TaskOnReview UserCompletedTaskPanel { get => _userCompletedTaskPanel; set => _userCompletedTaskPanel = value; }
    public ListType Type { get => _type; set => _type = value; }
    public int UserID { get => userID; set => userID = value; }

    public void OpenTaskInfoPanel()
    {
        switch (_type)
        {
            case ListType.User:
                _userCompletedTaskPanel.SetTasksList(ListType.UserDetailed, userID);
                print("User IDs: " + userID);
                break;
            case ListType.UserDetailed:
                _userCompletedTaskPanel.SetTasksList(ListType.UserDetailed, userID);
                
                break;
            case ListType.Tasks:
                _userCompletedTaskPanel.SetTaskInfo(_userCompletedTask);
                break;
            default:
                break;
        }
    }
}
