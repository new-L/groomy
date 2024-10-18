using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;
using static UnityEditor.Progress;

public class EditTaskPanel : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField _title;
    [SerializeField] private TMP_InputField _description;
    [SerializeField] private TMP_InputField _reward;

    [SerializeField] private TMP_Dropdown _dropdown;
    [SerializeField] private static List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

    [Header("Server")]
    [SerializeField] private Task _task;
    private int _currentTaskID;

    #region Get/Set
    public TMP_InputField Title { get => _title; set => _title = value; }
    public TMP_InputField Description { get => _description; set => _description = value; }
    public TMP_InputField Reward { get => _reward; set => _reward = value; }
    public static List<TMP_Dropdown.OptionData> Options { get => options; set => options = value; }
    public TMP_Dropdown Dropdown { get => _dropdown; set => _dropdown = value; }
    #endregion


    public void SetPreviosInformation(int taskID)
    {
        _currentTaskID = taskID;
        foreach (var item in Task.Tasks)
        {
            if(item.task_id == taskID)
            {
                Title.text = item.title;
                Description.text = item.description;
                Reward.text = item.reward.ToString();                
                if (Options.Count <= 0)
                {
                    foreach (var taskDifficulty in Task.TaskDifficulty)
                    {
                        Options.Add(new TMP_Dropdown.OptionData(taskDifficulty.difficulty));
                    }
                    Dropdown.AddOptions(Options);
                    Dropdown.value = Dropdown.options.FindIndex(option => option.text == item.difficulty); //Установка значения по умолчанию
                    Dropdown.RefreshShownValue();
                }
                break;
            }
            
        }
    }


    public void UpdateTask(bool isListUpdateNedeed)
    {
        int pickedEntryIndex = Dropdown.value;
        var newTask = new Tasks
        {
            task_id = _currentTaskID,
            title = UpdatedText(Title),
            description = UpdatedText(Description),
            difficulty = Dropdown.options[pickedEntryIndex].text,
            reward = Int32.Parse(UpdatedText(Reward))
        };
        Actions.OnStartLoad?.Invoke();
        _task.UpdateTask(newTask, isListUpdateNedeed);
    }

    public void NewTask(bool isListUpdateNedeed)
    {
        int pickedEntryIndex = Dropdown.value;
        Debug.Log("Entry position: " + Title.placeholder.GetComponent<TextMeshProUGUI>().text);
        var newTask = new Tasks
        {
            task_id = 999,
            title = UpdatedText(Title),
            description = UpdatedText(Description),
            difficulty = Dropdown.options[pickedEntryIndex].text,
            reward = Int32.Parse(UpdatedText(Reward))
        };
        Actions.OnStartLoad?.Invoke();
        _task.NewTask(newTask, isListUpdateNedeed);
    }

    private string UpdatedText(TMP_InputField inputField)
    {
        if (inputField.text.Equals("")) { return inputField.placeholder.GetComponent<TextMeshProUGUI>().text; }
        else return inputField.text;
    }
    private void OnDestroy()
    {
        Dropdown.ClearOptions();
        Options.Clear();
        StopAllCoroutines();
    }
}
