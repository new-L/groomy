using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class PurchaseOnServer : MonoBehaviour
{
    [SerializeField] private URLs _url;
    private string _json;
    private UnityWebRequest _www;
    private WWWForm _form;

    [SerializeField] private ShopProducts _shop;
    [SerializeField] private ShopProduct[] _purchases;
    
    public ShopProduct[] Purchases { get => _purchases; }

    [SerializeField] private UnityEvent _onPurchasesLoaded;

    public void GetActivePurchases()
    {
        StartCoroutine(nameof(GetPurchases));
    }

    public void LoadPurchaseIcon()
    {
        _shop.LoadPurchasesIcons(_purchases);
    }

    private IEnumerator GetPurchases()
    {
        _form = new WWWForm();
        _form.AddField("user_id", User.Player.user_id);
        UnityWebRequest www = UnityWebRequest.Post(_url.PurchaseProduct, _form);

        www.timeout = ServerSettings.TimeOut;

        yield return www.SendWebRequest();
        if (www.error != null) { Debug.Log("Не удалось связаться с сервером!"); yield break; }
        _json = www.downloadHandler.text;
        if (!_json.Equals("\"\""))
        {
            _json = JsonHelper.fixJson(www.downloadHandler.text);
            _purchases = JsonHelper.FromJson<ShopProduct>(_json);
            if (_purchases.Length != 0) _onPurchasesLoaded?.Invoke();
        }
        else
        {
            _shop.OnIconsLoad?.Invoke();
        }
    }

    private void OnDisable()
    {
        this.StopAllCoroutines();
    }
}


