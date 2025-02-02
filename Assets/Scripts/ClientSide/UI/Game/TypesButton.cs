using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TypesButton : MonoBehaviour
{
    [SerializeField] private List<Button> _typeButtons;

    private string _PATH = "Art/UI/";

    public List<Button> TypeButtons { get => _typeButtons; set => _typeButtons = value; }
    public string PATH { get => _PATH; set => _PATH = value; }

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
    public void DeactivateButton(Button button)
    {
        button.interactable = false;
    }
    public void SetActiveButton(GameObject button)
    {
        foreach (var item in TypeButtons)
        {
            if(item.gameObject == button)
            {
                item.GetComponent<Image>().sprite = Resources.Load<Sprite>(PATH + item.name);
                if (PATH.Contains("Shop")) SetTextColor(item, new Color32(255, 242, 230, 255));
            }
            else
            {
                item.GetComponent<Image>().sprite = Resources.Load<Sprite>(PATH + item.name + "Unactive");
                if (PATH.Contains("Shop")) SetTextColor(item, new Color32(71, 39, 11, 255));
            }
        }
    }


    public void SetAllButtonsActive()
    {
        foreach (var item in TypeButtons)
        {
            item.GetComponent<Image>().sprite = Resources.Load<Sprite>(PATH + item.name);
            if (PATH.Contains("Shop")) SetTextColor(item, new Color32(255, 242, 230, 255));
        }
    }

    private void SetTextColor(Button button, Color32 color)
    {
        button.GetComponentInChildren<TMP_Text>().color = color;
    }
}
