using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OutfitItem : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Image _signature;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private Items _item;
    [SerializeField] private Outfit _outfit;
    
    public void SetUI(Items item, GameObject outfit)
    {
        _item = item;
        _outfit = outfit.GetComponent<Outfit>();
        _icon.sprite = item.icon;
        if (item.signature == null) _signature.gameObject.SetActive(false);
        else _signature.sprite = item.signature;
        _name.text = item.title;
    }

    public void SetSprite()
    {
        _outfit.SetFromInventory(_item);
    }
}
