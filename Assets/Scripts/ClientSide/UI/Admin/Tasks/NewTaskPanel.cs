using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewTaskPanel : MonoBehaviour
{
    [SerializeField] private EditTaskPanel _editTaskPanel;

    [SerializeField] private List<TMP_InputField> _inputFields;

    public void SetNewTaskInformation()
    {
        _inputFields.Clear();
        _editTaskPanel.Title.text = "Название задачи";
        _editTaskPanel.Description.text = "Описание";
        _editTaskPanel.Reward.text = "999";
        if (EditTaskPanel.Options.Count <= 0)
        {
            foreach (var taskDifficulty in Task.TaskDifficulty)
            {
                EditTaskPanel.Options.Add(new TMP_Dropdown.OptionData(taskDifficulty.difficulty));
            }
            _editTaskPanel.Dropdown.AddOptions(EditTaskPanel.Options);
            _editTaskPanel.Dropdown.value = EditTaskPanel.Options.FindIndex(option => option.text == "Другое"); //Установка значения по умолчанию
            _editTaskPanel.Dropdown.RefreshShownValue();
        }
        _inputFields.Add(_editTaskPanel.Title);
        _inputFields.Add(_editTaskPanel.Description);
        _inputFields.Add(_editTaskPanel.Reward);
    }


    public void SaveTask()
    {
       if(_inputFields.Count > 0)
       {
            foreach (var item in _inputFields)
            {
                if (!IsPlaceholderOrTextEmpty(item)) { Debug.Log("{Одно из полей пустое!}"); return; }
            }
       }
        _editTaskPanel.NewTask(true);
    }


    private bool IsPlaceholderOrTextEmpty(TMP_InputField inputField)
    {
        if (inputField.placeholder.GetComponent<TextMeshProUGUI>().text.Equals("") && inputField.text.Equals(""))
        {
           return false;
        }
        else return true;
    }
}
