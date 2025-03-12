using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class InventoryServer : MonoBehaviour
{

    private WWWForm _form;
    private UnityWebRequest _www;
    private string _json;
    private Texture2D _texture;

    [SerializeField] private Items[] _playerItems;
    [SerializeField] private URLs _url;
    [SerializeField] private InventoryList _inventoryList;
    [SerializeField] private LoadSystem _loadSystem;

    public Items[] PlayerItems { get => _playerItems; private set => _playerItems = value; }

    private void Start()
    {
        _loadSystem?.SceneStart?.Invoke();
        StartCoroutine(Waiter(1f));
    }

    public void GetType(SkinTypes type)
    {
        StartCoroutine(SendRequest((int)type));
    }

    public void GetAllItems()
    {
        StartCoroutine(SendRequest((int)SkinTypes.ALL));
    }

    public void GetOutfitSkinByID(Items item)
    {
        StartCoroutine(GetOutfitSkin(item));
    }

    private IEnumerator SendRequest(int id)
    {
        _form = new WWWForm();
        _form.AddField("user_id", User.Player.user_id);
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
        PlayerItems = JsonHelper.FromJson<Items>(_json);

        foreach (var item in PlayerItems)
        {
            item.gamePosition = JsonUtility.FromJson<PositionInGame>(item.position_in_game);
        }
        StartCoroutine(nameof(GetIcons));
    }

    private IEnumerator GetIcons()
    {
        foreach (var item in PlayerItems)
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
        StartCoroutine(nameof(GetSignatures));
    }

    private IEnumerator GetSignatures()
    {
        foreach (var item in PlayerItems)
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
        _loadSystem.OnServerInventoryLoaded?.Invoke();
    }

    public IEnumerator GetOutfitSkin(Items currentItem)
    {
        //_www.timeout = ServerSettings.TimeOut;
        foreach (var item in PlayerItems)
        {
            if(item == currentItem)
            {
                _www = UnityWebRequestTexture.GetTexture(currentItem.skin_url);

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
                    item.sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2(item.gamePosition.pivX, item.gamePosition.pivY));
                }
                break;
            }
        }
    }

    private IEnumerator Waiter(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        GetAllItems();
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
    public Sprite sprite = null;
    public Sprite signature = null;
}

[Serializable]
public class PositionInGame
{
    public float pivX;
    public float pivY;
    public float posZ;
}
