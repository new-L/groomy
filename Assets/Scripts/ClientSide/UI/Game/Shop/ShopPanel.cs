using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{
    [Header("Loader")]
    [SerializeField] private InGameLoader _inGameLoader;
    [SerializeField] private GameObject _loader;

    [Header("Scroll View")]
    [SerializeField] private RectTransform _content;
    [SerializeField] private ScrollRect _scroll;
    [SerializeField] private RectTransform _prefab;

    [Header("Category")]
    [SerializeField] private List<Button> _types;
    [SerializeField] private TypesButton _typesButton;

    [Header("Server")]
    [SerializeField] private ShopProducts _shop;

    [Header("ProductDetailPanel")]
    [SerializeField] private ProductDetailPanel _detailPanel;

    private string _productType = "real";

    public string ProductType { get => _productType; set => _productType = value; }

    public void StartLoad()
    {
        _inGameLoader.SetLoader(_loader);
        _typesButton.PATH = "Art/UI/Shop/";
        _typesButton.TypeButtons = _types;
        InGameLoader.IsBorryActivate = false;
        Actions.OnStartLoad?.Invoke();
        ClearListView();
        _shop.DownloadProducts(ProductType);
    }
    public void SetProductList()
    {
        if (_shop.Products.Length == 0 || _shop.Products == null)
        {
            Actions.OnListCreated?.Invoke();
            _typesButton.ActivateButtons();
            return;
        }

        foreach (var item in _shop.Products)
        {
            if (item.type.Equals(_productType))
            {
                var instance = GameObject.Instantiate(_prefab.gameObject) as GameObject;
                instance.GetComponent<ShopProductDetail>().SetDetail(item, _detailPanel);
                instance.transform.SetParent(_content, false);
            }
        }
        _typesButton.ActivateButtons();
        Actions.OnListCreated?.Invoke();
    }

    private void ClearListView()
    {
        foreach (Transform child in _content)
        {
            Destroy(child.gameObject);
        }
    }
}
