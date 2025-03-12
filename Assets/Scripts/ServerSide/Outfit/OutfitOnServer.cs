using System;
using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Networking;

public class OutfitOnServer : MonoBehaviour
{

    private WWWForm _form;
    private UnityWebRequest _www;
    private string _json;

    [SerializeField] private URLs _url;
    [SerializeField] private Outfit _outfit;
    [SerializeField] private CurrentOutfit _currentOutfit;
    public CurrentOutfit CurrentOutfit { get => _currentOutfit; set => _currentOutfit = value; }
    public string Json { get => _json; set => _json = value; }

    public void GetOutfit()
    {
        StartCoroutine(Request("get"));
    }

    public IEnumerator UpdateOutfit(bool isNeedToChangeScene)
    {
        yield return StartCoroutine(Request("update"));
        if (isNeedToChangeScene) SceneJump.JumpTo(1);
    }

    private IEnumerator Request(string requestType)
    {
        _form = new WWWForm();
        _form.AddField("user_id", User.Player.user_id);
        _form.AddField("request_type", requestType);
        if(requestType.Equals("update"))
        {
            _form.AddField("hat", _currentOutfit.hat);
            _form.AddField("body", _currentOutfit.body);
            _form.AddField("legs", _currentOutfit.legs);
            _form.AddField("skin", _currentOutfit.skin);
        }
        _www = UnityWebRequest.Post(_url.Outfit, _form);

        _www.timeout = ServerSettings.TimeOut;

        yield return _www.SendWebRequest();

        if (_www.error != null)
        {
            Debug.Log(_www.error);
            yield break;
        }
        Json = _www.downloadHandler.text;
        CurrentOutfit = JsonUtility.FromJson<CurrentOutfit>(Json);
        _outfit.SetCurrentOutfit();
    }
    private void OnDisable()
    {
        this.StopAllCoroutines();
    }
}

[Serializable]
public class CurrentOutfit
{
    public int hat;
    public int body;
    public int legs;
    public int skin;

    public void SetNull()
    {
        hat = 0;
        body = 0;
        legs = 0;
        skin = 0;
    }
}
