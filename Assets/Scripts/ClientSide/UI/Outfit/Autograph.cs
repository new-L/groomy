using UnityEngine;
using UnityEngine.UI;

public class Autograph : MonoBehaviour
{

    [SerializeField] private Image _signature;
    [SerializeField] private GameObject _signaturePanel;
    [SerializeField] private InventoryList _skinType;
    [SerializeField] private InventoryServer _inventoryServer;
    [SerializeField] private OutfitOnServer _outfitOnServer;

    public void Set(Sprite image)
    {
        _signature.sprite = image;
        if (image == null) _signaturePanel.gameObject.SetActive(false);
        else _signaturePanel.gameObject.SetActive(true);
    }

    public void Set(int itemID)
    {
        if (itemID == 0) { Set(null); return; }
        foreach (var item in _inventoryServer.PlayerItems)
        {
            if(itemID == item.item_id)
            {
                Set(item.signature);
                break;
            }
        }
    }
    public void SetByType()
    {
        switch (_skinType.SkinType)
        {
            case SkinTypes.HATID:
                Set(_outfitOnServer.CurrentOutfit.hat);
                break;
            case SkinTypes.BODYID:
                Set(_outfitOnServer.CurrentOutfit.body);
                break;
            case SkinTypes.LEGSID:
                Set(_outfitOnServer.CurrentOutfit.legs);
                break;
            case SkinTypes.SKINID:
                Set(_outfitOnServer.CurrentOutfit.skin);
                break;
            case SkinTypes.ALL:
                break;
            default:
                break;
        }
    }
    public void Clear()
    {
        _signaturePanel.SetActive(false);
    }
}
