using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopProductDetail : MonoBehaviour
{

    [Header("UI")]
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _price;
    [SerializeField] private ProductDetailPanel _detailPanel;

    private ShopProduct _product;

    public void SetDetail(ShopProduct product, ProductDetailPanel _productPanel)
    {
        _product = product;
        _icon.sprite = product.icon;
        _title.text = product.title;
        _price.text = product.price.ToString();
        _detailPanel = _productPanel;
    }

    public void OnItemClick()
    {
        _detailPanel.OnProductClick(_product);
    }

}
