using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Notification : MonoBehaviour
{
    [Header("Notification Settings")]
    [SerializeField] private int _time = 0;

    [SerializeField] private TMP_Text _notificationText;
    [SerializeField] private Animator _notificationAnimator;

    public void NotificationIn(string _text)
    {
        StopAnimation();
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
        _notificationText.text = text;
        _notificationAnimator.SetBool("isNotification", true);
        yield return new WaitForSeconds(time);
        _notificationAnimator.SetBool("isNotification", false);
    }

}
