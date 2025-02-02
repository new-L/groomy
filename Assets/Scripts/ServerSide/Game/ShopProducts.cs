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
    [SerializeField] private Notification _notification;

    [SerializeField] private UnityEvent _onProductsLoad;
    [SerializeField] private UnityEvent _onIconsLoad;
    [SerializeField] private UnityEvent _onProductCountLoad;
    [SerializeField] private UnityEvent _onProductPurchased;
    [SerializeField] private UnityEvent _onPurchaseBreak;

    private WWWForm _form;
    public ShopProduct[] Products { get => _products; }

    //Методы для для взаимодействия с внешними скриптами
    //Загрузка всех товаров определенного типа
    public void DownloadProducts(string type)
    {
        StartCoroutine(GetProducts(type));
    }
    //Покупка товара
    public void BuyProduct(int ID)
    {
        StartCoroutine(PostOrder(ID));
    }
    //Загрузка кол-во продукта (загружается по кулдауну во время активной панели конкретного айтема
    public void LoadProductCount(int productID)
    {
        StartCoroutine(GetProductCountByID(productID));
    }
    //Загрузка спрайтов продуктов
    public void LoadProductIcons()
    {
        StartCoroutine(GetProductIcons(Products));
    }
    public void LoadPurchasesIcons(ShopProduct[] purchases)
    {
        StartCoroutine(GetProductIcons(purchases));
    }

    //Запросы на сервер
    //Подтягиваем все продукты определенного типа. По умолчания - существующие в реальности
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
        _json = JsonHelper.fixJson(www.downloadHandler.text);
        _products = JsonHelper.FromJson<ShopProduct>(_json);
        _onProductsLoad?.Invoke();
    }
    //Апдейтим данные таблицы
    private IEnumerator PostOrder(int ID)
    {
        StopCoroutine(nameof(Waiter));
        _form = new WWWForm();
        _form.AddField("product_id", ID);
        _form.AddField("user_id", User.Player.user_id);
        _form.AddField("response_type", "update");
        UnityWebRequest www = UnityWebRequest.Post(_url.ShopProduct, _form);

        www.timeout = ServerSettings.TimeOut;

        yield return www.SendWebRequest();
        if (www.error != null) { Debug.Log("Не удалось связаться с сервером!"); yield break; }
        else
        {
            string _text = www.downloadHandler.text.ToString();
            print(_text);
            if (_text.Contains("{gold_error}")) { _notification.NotificationIn("Недостаточно монет!"); _onPurchaseBreak?.Invoke(); }
            else if (_text.Contains("{product_error}")) { _notification.NotificationIn("Товар закончился!"); _onPurchaseBreak?.Invoke(); }
            else _onProductPurchased?.Invoke();
        }
    }
    //Загружаем спрайты продуктов
    private IEnumerator GetProductIcons(ShopProduct[] products)
    {
        foreach (var item in products)
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
    //Загружаем конкретное кол-во продуктов
    private IEnumerator GetProductCountByID(int productID)
    {
        StopCoroutine(nameof(Waiter));
        _form = new WWWForm();
        _form.AddField("response_type", "getcount");
        _form.AddField("productID", productID);
        UnityWebRequest www = UnityWebRequest.Post(_url.ShopProduct, _form);

        www.timeout = ServerSettings.TimeOut;

        yield return www.SendWebRequest();
        if (www.error != null) { Debug.Log("Не удалось связаться с сервером!"); StartCoroutine(Waiter("getcount", productID)); yield break; }
        else
        {
            foreach (var item in _products)
            {
                if (item.product_id == productID)
                {
                    item.count = Int32.Parse(www.downloadHandler.text);
                    StartCoroutine(Waiter("getcount", productID));
                    _onProductCountLoad?.Invoke();
                    break;
                }
            }
        }
    }

    //Кулдаун, по которому работает запросы для обновления в реалтайме (пока не придумал альтернативы для загрузки в реальном времени, кроме как отправлять такой же запрос по времени)
    private IEnumerator Waiter(string request, int ID)
    {
        yield return new WaitForSecondsRealtime(ServerSettings.Cooldown / 2);
        if (request.Equals("getcount")) StartCoroutine(GetProductCountByID(ID));
    }


    public void StopLoadProductCount()
    {
        StopCoroutine(nameof(GetProductCountByID));
        StopCoroutine(nameof(Waiter));
    }

    public void StopCoroutines()
    {
        this.StopAllCoroutines();
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
