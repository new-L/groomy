using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ShopPanel : MonoBehaviour
{
    [Header("Loader")]
    [SerializeField] private InGameLoader _inGameLoader;
    [SerializeField] private GameObject _loader;

    [Header("Scroll View")]
    [SerializeField] private RectTransform _content;
    [SerializeField] private ScrollRect _scroll;
    [SerializeField] private RectTransform _productPrefab;
    [SerializeField] private RectTransform _purchasePrefab;

    [Header("Category")]
    [SerializeField] private List<Button> _types;
    [SerializeField] private TypesButton _typesButton;

    [Header("Server")]
    [SerializeField] private ShopProducts _shop;
    [SerializeField] private PurchaseOnServer _purchase;

    [Header("ProductDetailPanel")]
    [SerializeField] private ProductDetailPanel _detailPanel;

    private string _productType = "real";
    [SerializeField] private bool _isProductsLoading = true, _isPurchasesLoading = false;

    public string ProductType { get => _productType; set => _productType = value; }
    public void StartLoadProducts()
    {
        _isProductsLoading = true;
        _isPurchasesLoading = !_isProductsLoading;
        StartLoad();
        _shop.DownloadProducts(ProductType);
    }

    public void StartLoadPurchases()
    {
        _isPurchasesLoading = true;
        _isProductsLoading = !_isPurchasesLoading;
        StartLoad();
        _purchase.GetActivePurchases();
    }
    private void StartLoad()
    {
        _inGameLoader.SetLoader(_loader);
        _typesButton.PATH = "Art/UI/Shop/";
        _typesButton.TypeButtons = _types;
        InGameLoader.IsBorryActivate = false;
        Actions.OnStartLoad?.Invoke();
        ClearListView();
    }

    public void OnProductsLoaded()
    {
        if (!_isProductsLoading) return;
        if (!CheckArrayNullOrEmpty(_shop.Products))
        {
            OnListCreated();
            return;
        }
        foreach (var item in _shop.Products)
        {
            if (item.type.Equals(_productType))
            {
                InitializeItem(item, _productPrefab);
            }
        }
        OnListCreated();
    }
    public void OnPurchasesLoaded()
    {
        if (!_isPurchasesLoading) return;
        if (!CheckArrayNullOrEmpty(_purchase.Purchases))
        {
            OnListCreated();
            return;
        }
        foreach (var item in _purchase.Purchases)
        {
            InitializeItem(item, _purchasePrefab);
        }
        OnListCreated();
    }

    private void InitializeItem(ShopProduct item, RectTransform prefab)
    {
        var instance = GameObject.Instantiate(prefab.gameObject) as GameObject;
        instance.GetComponent<ShopProductDetail>().SetDetail(item, _detailPanel);
        instance.transform.SetParent(_content, false);
    }

    private bool CheckArrayNullOrEmpty(ShopProduct[] products)
    {
        if (products.Length == 0 || products == null)
            return false;
        return true;
    }

    private void OnListCreated()
    {
        Actions.OnListCreated?.Invoke();
        _typesButton.ActivateButtons();
    }

    private void ClearListView()
    {
        foreach (Transform child in _content)
        {
            Destroy(child.gameObject);
        }
    }
}
