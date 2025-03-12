using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InGameOutfit : MonoBehaviour
{
    private WWWForm _form;
    private UnityWebRequest _www;
    private string _json;
    private Texture2D _texture;

    [SerializeField] private URLs _url;
    [SerializeField] private CurrentOutfit _currentOutfitIDs;
    [SerializeField] private Skins[] _skin;

    [SerializeField] private SpriteRenderer _head;
    [SerializeField] private SpriteRenderer _face;
    [SerializeField] private SpriteRenderer _legs;
    [SerializeField] private SpriteRenderer _body;
    private void Start()
    {
        LoadAndSetOutfit();
    }
    private void LoadAndSetOutfit()
    {
        StartCoroutine(Request("get"));
    }

    private IEnumerator Request(string requestType)
    {
        _form = new WWWForm();
        _form.AddField("user_id", User.Player.user_id);
        _form.AddField("request_type", requestType);
        _www = UnityWebRequest.Post(_url.Outfit, _form);

        _www.timeout = ServerSettings.TimeOut;

        yield return _www.SendWebRequest();

        if (_www.error != null)
        {
            Debug.Log(_www.error);
            yield break;
        }
        _json = _www.downloadHandler.text;
        _currentOutfitIDs = JsonUtility.FromJson<CurrentOutfit>(_json);
        yield return StartCoroutine(GetOutfitSkin(_currentOutfitIDs.hat, _head));
        yield return StartCoroutine(GetOutfitSkin(_currentOutfitIDs.body, _face));
        yield return StartCoroutine(GetOutfitSkin(_currentOutfitIDs.legs, _legs));
        yield return StartCoroutine(GetOutfitSkin(_currentOutfitIDs.skin, _body));
        InGameLoader.Tables[DBTablesName.UserOutfit] = true;
        InGameLoader.IsBorryActivate = true;
        Actions.OnUserDatasLoad?.Invoke();
    }


    public IEnumerator GetOutfitSkin(int ID, SpriteRenderer item)
    {
        if (ID == 0) yield break;
        _form = new WWWForm();
        _form.AddField("user_id", User.Player.user_id);
        _form.AddField("request_type", "getSkinByID");
        _form.AddField("item_id", ID);
        _www = UnityWebRequest.Post(_url.Outfit, _form);
        yield return _www.SendWebRequest();
        if (_www.error != null)
        {
            Debug.Log(_www.error);
            yield break;
        }
        _json = JsonHelper.fixJson(_www.downloadHandler.text);
        
        Debug.Log($"{_json}");
        _skin = JsonHelper.FromJson<Skins>(_json);
        _skin[0].gamePosition = JsonUtility.FromJson<PositionInGame>(_skin[0].position_in_game);
        if (_skin != null)
        {
            _www = UnityWebRequestTexture.GetTexture(_skin[0].skin_url);

            yield return _www.SendWebRequest();
            while (!_www.isDone)
            {
                yield return _www;
            }
            if (_www.isNetworkError || _www.isHttpError)
            {
                Debug.LogError(_www.error);
            }
            else
            {
                _texture = ((DownloadHandlerTexture)_www.downloadHandler).texture;
                item.sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2(_skin[0].gamePosition.pivX, _skin[0].gamePosition.pivY));
            }
        }
    }

    private void OnDisable()
    {
        _currentOutfitIDs = null;
        this.StopAllCoroutines();
    }
}

[Serializable]
public class Skins
{
    public string skin_url;
    public string position_in_game;
    public PositionInGame gamePosition;
}