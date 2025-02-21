using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelAnimation : MonoBehaviour
{

    [SerializeField] private Animator _animator;


    public void ShopPanelPlay(Animator panel)
    {
        if (panel == null) return;
        _animator = panel;
        SetAnimatorParametrs("isShopPanel", true);
    }

    public void RatingPanelPlay(Animator panel)
    {
        if (panel == null) return;
        _animator = panel;
        SetAnimatorParametrs("isRatingPanel", true);
    }
    
    public void TaskPanelPlay(Animator panel)
    {
        if (panel == null) return;
        _animator = panel;
        SetAnimatorParametrs("isTaskPanel", true);
    }

    private void SetAnimatorParametrs(string parametrName, bool value)
    {
        foreach (var parametr in _animator.parameters)
        {
            parametr.defaultBool = false;
        }
        _animator.SetBool(parametrName, value);
    }
}
