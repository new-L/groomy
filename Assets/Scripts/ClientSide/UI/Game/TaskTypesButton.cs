using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskTypesButton : MonoBehaviour
{
    [SerializeField] private List<Button> _typeButtons;

    private string _PATH = "Art/UI/";

    public List<Button> TypeButtons { get => _typeButtons; set => _typeButtons = value; }
    public void ActivateButtons()
    {
        foreach (var item in _typeButtons)
        {
            item.interactable = true;
        }
    }

    public void DeactivateButtons()
    {
        foreach (var item in _typeButtons)
        {
            item.interactable = false;
        }
    }

    public void SetActiveButton(GameObject button)
    {
        foreach (var item in TypeButtons)
        {
            if(item.gameObject == button)
            {
                item.GetComponent<Image>().sprite = Resources.Load<Sprite>(_PATH + item.name);
            }
            else
            {
                item.GetComponent<Image>().sprite = Resources.Load<Sprite>(_PATH + item.name + "Unactive");
            }
        }
    }
}
