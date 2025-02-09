using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private Music _music;
    [SerializeField] private bool _isPassed;
    [SerializeField] private GameObject _tutorial;

    private void Start()
    {
        TutorialPassedInfo(true);
    }

    public void TutorialPassedInfo(bool isVisible)
    {
        _tutorial.SetActive(isVisible);
        _isPassed = isVisible;
        if (_isPassed) _music.StartAfterTutorial();
        //Actions.OnStartLoad?.Invoke();
        //StartCoroutine(CheckTutorialPassedInfo("rhytmgame"));
    }

    public void HidePanel()
    {
        if(_isPassed)
            _tutorial.SetActive(false);
    }

    private IEnumerator CheckTutorialPassedInfo(string name)
    {
        WWWForm form = new WWWForm();
        form.AddField("user_id", User.Player.user_id);
        form.AddField("tutorial_name", name);
        UnityWebRequest www = UnityWebRequest.Post(URLs.Tutorial, form);

        www.timeout = ServerSettings.TimeOut;

        yield return www.SendWebRequest();
        if (www.error != null) { Debug.Log("Не удалось связаться с сервером!"); yield break; }
        _isPassed = Convert.ToBoolean(Convert.ToInt32(www.downloadHandler.text));
        if (_isPassed) _music.StartAfterTutorial();
        else
        {
            _tutorial.SetActive(true);
        }
    }
    private void OnEnable()
    {
        //Actions.OnListCreated += HidePanel;
    }

    private void OnDisable()
    {
        //Actions.OnListCreated -= HidePanel;
    }
}
