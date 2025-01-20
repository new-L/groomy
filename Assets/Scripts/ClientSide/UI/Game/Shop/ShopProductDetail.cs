using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopProductDetail : MonoBehaviour
{

    [Header("UI")]
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _price;

    private int _productID;

    public void SetDetail(ShopProduct product)
    {
        _productID = product.product_id;
        _icon.sprite = product.icon;
        _title.text = product.title;
        _price.text = product.price.ToString();
    }
}
