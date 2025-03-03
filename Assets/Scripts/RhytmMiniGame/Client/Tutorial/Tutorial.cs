using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private Music _music;
    [SerializeField] private bool _isPassed;
    [SerializeField] private GameObject _tutorial;
    [SerializeField] private GameObject _hideButton;
    [SerializeField] private Toggle _hideAllTheTime;

    private void Start()
    {
        TutorialPassedInfo(true);
        
    }

    public void TutorialPassedInfo(bool isVisible)
    {
        Actions.OnStartLoad?.Invoke();
        StartCoroutine(CheckTutorialPassedInfo("rhytmgame"));
    }

    public void HidePanel()
    {
        _tutorial.SetActive(false);
        if(_hideAllTheTime.isOn) StartCoroutine(SetTutorialPassed("rhytmgame"));
        if(_isPassed) _music.StartAfterTutorial();
    }

    private IEnumerator CheckTutorialPassedInfo(string name)
    {
        WWWForm form = new WWWForm();
        form.AddField("user_id", User.Player.user_id);
        form.AddField("tutorial_name", name);
        form.AddField("request_type", "get");
        UnityWebRequest www = UnityWebRequest.Post(URLs.Tutorial, form);

        www.timeout = ServerSettings.TimeOut;

        yield return www.SendWebRequest();
        if (www.error != null) { Debug.Log("Не удалось связаться с сервером!"); yield break; }
        _isPassed = Convert.ToBoolean(Convert.ToInt32(www.downloadHandler.text));
        if (_isPassed)
        {
            _tutorial.SetActive(false);
            _music.StartAfterTutorial();
        }
        else
        {
            _tutorial.SetActive(true);
            StartCoroutine(nameof(AnimationWaiter));
        }
    }

    private IEnumerator SetTutorialPassed(string name)
    {
        WWWForm form = new WWWForm();
        form.AddField("user_id", User.Player.user_id);
        form.AddField("tutorial_name", name);
        form.AddField("request_type", "set");
        UnityWebRequest www = UnityWebRequest.Post(URLs.Tutorial, form);

        www.timeout = ServerSettings.TimeOut;

        yield return www.SendWebRequest();
        if (www.error != null) { Debug.Log("Не удалось связаться с сервером!"); yield break; }
    }


    private IEnumerator AnimationWaiter()
    {
        _hideButton.SetActive(false);
        yield return new WaitForSecondsRealtime(3f);
        _hideButton.SetActive(true);
        _isPassed = true;
    }
}
