using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class ShopProducts : MonoBehaviour
{
    [SerializeField] private URLs _url;
    [SerializeField] private string _json;
    private UnityWebRequest _www;
    private Texture2D _texture;


    [SerializeField] private ShopProduct[] _products;

    [SerializeField] private UnityEvent _onProductsLoad;
    [SerializeField] private UnityEvent _onIconsLoad;

    private WWWForm _form;
    public ShopProduct[] Products { get => _products; }

    public void DownloadProducts(string type)
    {
        StartCoroutine(GetProducts(type));
    }

    public void BuyProduct()
    {
        StartCoroutine(PostOrder());
    }

    

    public void LoadProductIcons()
    {
        StartCoroutine(GetProductIcons());
    }
    public void StopCoroutines()
    {
        this.StopAllCoroutines();
    }
    private IEnumerator GetProducts(string type = "real")
    {
        Debug.Log("{GetProducts}: Запрашиваем данные у таблицы");
        _form = new WWWForm();
        _form.AddField("product_type", type);
        _form.AddField("response_type", "get");
        UnityWebRequest www = UnityWebRequest.Post(_url.ShopProduct, _form);

        www.timeout = ServerSettings.TimeOut;

        yield return www.SendWebRequest();
        if (www.error != null) { Debug.Log("Не удалось связаться с сервером!"); yield break; }
        Debug.Log(www.downloadHandler.text);
        _json = JsonHelper.fixJson(www.downloadHandler.text);
        _products = JsonHelper.FromJson<ShopProduct>(_json);
        _onProductsLoad?.Invoke();
    }

    private IEnumerator PostOrder()
    {
        _form = new WWWForm();
        _form.AddField("product_id", "");
        _form.AddField("user_id", User.Player.user_id);
        _form.AddField("response_type", "update");
        UnityWebRequest www = UnityWebRequest.Post(_url.ShopProduct, _form);

        www.timeout = ServerSettings.TimeOut;

        yield return www.SendWebRequest();
        if (www.error != null) { Debug.Log("Не удалось связаться с сервером!"); yield break; }

    }

    private IEnumerator GetProductIcons()
    {
        foreach (var item in Products)
        {
            _www = UnityWebRequestTexture.GetTexture(item.img_url);
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
        _onIconsLoad?.Invoke();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}

[Serializable]
public class ShopProduct
{
    public int product_id;
    public string title;
    public string description;
    public string type;
    public int price;
    public int count;
    public string img_url;
    public Sprite icon;
}
