using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    [Header("Notification Settings")]
    [SerializeField] private int _time = 0;
    [SerializeField] private string _text = "";
    private string PATH = "Art/Notification/";

    [SerializeField] private Image _type;
    [SerializeField] private TMP_Text _textUI;
    [SerializeField] private Animator _notificationAnimator;


    public void Set(NotificationType type, string text)
    {
        _type.sprite = Resources.Load<Sprite>($"{PATH}{type}");
        _textUI.text = text;
    }

    public void Play()
    {
        if (_type == null || _textUI == null) return;
        //StopAnimation();
        StartCoroutine(SendNotification(_text, _time));
    }

    private void StopAnimation()
    {
        _notificationAnimator.Rebind();
        _notificationAnimator.Update(0f);
        this.StopAllCoroutines();
    }

    private IEnumerator SendNotification(string text, int time)
    {
        _notificationAnimator.SetBool("isNotification", true);
        yield return new WaitForSeconds(time);
        _notificationAnimator.SetBool("isNotification", false);
    }
}
