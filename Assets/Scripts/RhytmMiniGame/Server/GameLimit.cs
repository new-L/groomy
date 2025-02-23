using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class GameLimit : MonoBehaviour
{
    #region request
    private WWWForm _form;
    private string _json;
    private UnityWebRequest _www;
    #endregion

    private bool _isSended;
    private Limit _limit;
    [SerializeField] private UnityEvent OnLimitLoaded;
    [SerializeField] private EndGame _endGame;

    public Limit Limit { get => _limit; private set => _limit = value; }
    public bool IsSended { get => _isSended; private set => _isSended = value; }

    public void Get()
    {
        StartCoroutine(SendToServer("get"));
    }

    public void Reduce()
    {
        StartCoroutine(SendToServer("reduce"));
    }
    private IEnumerator SendToServer(string request)
    {
        IsSended = false;
        _form = new WWWForm();
        _form.AddField("user_id", User.Player.user_id);
        _form.AddField("request_type", request);
        _www = UnityWebRequest.Post(URLs.Rhytmgamelimit, _form);

        _www.timeout = ServerSettings.TimeOut;

        yield return _www.SendWebRequest();
        if (_www.error != null)
        {
            Debug.Log("Не удалось связаться с сервером!");
            StartCoroutine(SendToServer(request));
            yield break;
        }
        else
        {
            _limit = JsonUtility.FromJson<Limit>(_www.downloadHandler.text);
            Actions.OnListCreated?.Invoke();
            OnLimitLoaded?.Invoke();
            if (request.Equals("reduce")) _endGame.CheckDatasSendedStatus();
        }
    }



    private void OnDisable()
    {
        this.StopAllCoroutines();
    }
    private void OnApplicationQuit()
    {
        this.StopAllCoroutines();
    }

}

[Serializable]
public class Limit
{
    public int current;
    public int maximum;
}