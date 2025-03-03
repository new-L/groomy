using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEditor.Progress;

public class InventoryServer : MonoBehaviour
{
    #region Constants
    private const int HATID = 2001;
    private const int BODYID = 2002;
    private const int LEGSID = 2003;
    private const int SKINID = 2004;
    private const int ALL = 2994;
    #endregion

    private WWWForm _form;
    private UnityWebRequest _www;
    private string _json;
    private Texture2D _texture;

    [SerializeField] private Items[] _playerItems;
    [SerializeField] private URLs _url;
    private void Start()
    {
        GetHats();
    }

    public void GetHats()
    {
        StartCoroutine(SendRequest(HATID));
    }

    private IEnumerator SendRequest(int id)
    {
        _form = new WWWForm();
        _form.AddField("user_id", 406788);//User.Player.user_id);
        _form.AddField("items_type", id);
        _www = UnityWebRequest.Post(_url.Inventory, _form);

        _www.timeout = ServerSettings.TimeOut;

        yield return _www.SendWebRequest();

        if(_www.error != null)
        {
            Debug.Log(_www.error);
            yield break;
        }
        _json = JsonHelper.fixJson(_www.downloadHandler.text);
        _playerItems = JsonHelper.FromJson<Items>(_json);

        foreach (var item in _playerItems)
        {
            item.gamePosition = JsonUtility.FromJson<PositionInGame>(item.position_in_game); ;
        }
        StartCoroutine(nameof(GetIcons));
    }

    private IEnumerator GetIcons()
    {
        foreach (var item in _playerItems)
        {
            _www = UnityWebRequestTexture.GetTexture(item.icon_url);
            yield return _www.SendWebRequest();
            if (_www.isNetworkError || _www.isHttpError)
            {
                Debug.LogError(_www.error);
            }
            else
            {
                _texture = ((DownloadHandlerTexture)_www.downloadHandler).texture;
                item.icon = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2());
            }
        }
    }

    private IEnumerator GetSignatures()
    {
        foreach (var item in _playerItems)
        {
            if (!item.signature_url.Equals(""))
            {
                _www = UnityWebRequestTexture.GetTexture(item.signature_url);
                yield return _www.SendWebRequest();
                if (_www.isNetworkError || _www.isHttpError)
                {
                    Debug.LogError(_www.error);
                }
                else
                {
                    _texture = ((DownloadHandlerTexture)_www.downloadHandler).texture;
                    item.signature = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2());
                }
            }
        }
    }

    private IEnumerator GetOutfitSkin(Items currentItem)
    {
        foreach (var item in _playerItems)
        {
            if(item.item_id == currentItem.item_id)
            {

                _www.timeout = ServerSettings.TimeOut;
                _www = UnityWebRequestTexture.GetTexture(currentItem.signature_url);

                yield return _www.SendWebRequest();
                if (_www.isNetworkError || _www.isHttpError)
                {
                    Debug.LogError(_www.error);
                }
                else
                {
                    _texture = ((DownloadHandlerTexture)_www.downloadHandler).texture;
                    item.signature = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2());
                }
                break;
            }
        }
    }
}
[Serializable]
public class Items
{
    public int item_id;
    public string title;
    public string author;
    public int type_id;
    public string icon_url;
    public string skin_url;
    public int fit;
    public string position_in_game;
    public string signature_url;


    public PositionInGame gamePosition;
    public Sprite icon;
    public Sprite sprite;
    public Sprite signature = null;
}

[Serializable]
public class PositionInGame
{
    public float pivX;
    public float pivY;
    public float posZ;
}
