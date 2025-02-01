using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class ProductDetailPanel : MonoBehaviour
{
    [SerializeField] private int _productID;

    [Header("UI")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _count;
    [SerializeField] private Button _buyButton;
    [SerializeField] private Button _hidePanelButton;

    [Header("Server")]
    [SerializeField] private ShopProducts _productsOnServer;
    public void OnProductClick(ShopProduct _product)
    {
        _productID = _product.product_id;
        _icon.sprite = _product.icon;
        _title.text = _product.title;
        _description.text = $"Описание:\n{_product.description}";
        _count.text = $"Осталось: {_product.count} шт.";
        _buyButton.interactable = true;
        _hidePanelButton.interactable = true;
        _panel.SetActive(true);
        _productsOnServer.LoadProductCount(_productID);
    }
    public void CountUpdate()
    {
        if (!_panel.activeSelf) return;
        foreach (var item in _productsOnServer.Products)
        {
            if(_productID == item.product_id)
            {
                _count.text = $"Осталось: {item.count} шт.";
                break;
            }
        }
        
    }

    public void Buy()
    {
        _buyButton.interactable = false;
        _hidePanelButton.interactable = false;
        _productsOnServer.StopLoadProductCount();
        _productsOnServer.BuyProduct(_productID);
    }

    public void OnProductPanelClose()
    {
        _productsOnServer.StopLoadProductCount();
        _productID = 0;
        _panel.SetActive(false);
    }
}
