using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Outfit : MonoBehaviour
{
    [SerializeField] private OutfitOnServer _outfitOnServer;
    [SerializeField] private InventoryServer _inventoryServer;
    [SerializeField] private InventoryList _inventoryList;
    [SerializeField] private LoadSystem _loadSystem;
    [SerializeField] private Autograph _autograph;

    [SerializeField] private SpriteRenderer hat;
    [SerializeField] private SpriteRenderer body;
    [SerializeField] private SpriteRenderer legs;
    [SerializeField] private SpriteRenderer skin;

    [SerializeField] private GameObject SCREENBLOCK;


    public void SetFromInventory(Items item)
    {
        switch (item.type_id)
        {
            case (int)SkinTypes.HATID: 
                ClearSkin(); 
                StartCoroutine(WaitUntilDownloadSprite(item.item_id, hat)); 
                _outfitOnServer.CurrentOutfit.hat = item.item_id;
                _autograph.Set(item.signature); break;
            case (int)SkinTypes.BODYID: 
                ClearSkin(); 
                StartCoroutine(WaitUntilDownloadSprite(item.item_id, body)); 
                _outfitOnServer.CurrentOutfit.body = item.item_id; 
                _autograph.Set(item.signature);  
                break;
            case (int)SkinTypes.LEGSID: 
                ClearSkin(); 
                StartCoroutine(WaitUntilDownloadSprite(item.item_id, legs)); 
                _outfitOnServer.CurrentOutfit.legs = item.item_id; 
                _autograph.Set(item.signature);  
                break;
            case (int)SkinTypes.SKINID: 
                Clear(true); 
                StartCoroutine(WaitUntilDownloadSprite(item.item_id, skin)); 
                _outfitOnServer.CurrentOutfit.skin = item.item_id; 
                _autograph.Set(item.signature); 
                break;
            default:
                break;
        }
    }

    public void Clear(bool isClearAll)
    {
        if (isClearAll) { _outfitOnServer.CurrentOutfit.SetNull(); SetCurrentOutfit(); return; }
        switch (_inventoryList.SkinType)
        {
            case SkinTypes.HATID: _outfitOnServer.CurrentOutfit.hat = 0; break;
            case SkinTypes.BODYID: _outfitOnServer.CurrentOutfit.body = 0; break;
            case SkinTypes.LEGSID: _outfitOnServer.CurrentOutfit.legs = 0; break;
            case SkinTypes.SKINID: _outfitOnServer.CurrentOutfit.skin = 0; break;
            default:
                break;
        }
        SetCurrentOutfit();
    }

    public void ClearToDefault()
    {
        _outfitOnServer.CurrentOutfit = JsonUtility.FromJson<CurrentOutfit>(_outfitOnServer.Json);
        SetCurrentOutfit();
    }

    private void ClearSkin()
    {
        _outfitOnServer.CurrentOutfit.skin = 0;
        skin.sprite = default;
    }

    public void SaveChanges(bool isNeedToChangeScene)
    {
        SCREENBLOCK.SetActive(isNeedToChangeScene);
        StartCoroutine(_outfitOnServer.UpdateOutfit(isNeedToChangeScene));
    }

    public void SetCurrentOutfit()
    {
        _autograph.SetByType();
        StartCoroutine(nameof(WaitUntilDownload));
    }
    private IEnumerator WaitUntilDownloadSprite(int skinID, SpriteRenderer spriteR)
    {
        SCREENBLOCK.SetActive(true);
        if (skinID == 0) yield break;
        yield return GetOutfitItem(skinID, spriteR);
        SCREENBLOCK.SetActive(false);
    }
    private IEnumerator WaitUntilDownload()
    {
        SCREENBLOCK.SetActive(true);
        yield return GetOutfitItem(_outfitOnServer.CurrentOutfit.hat, hat);
        yield return GetOutfitItem(_outfitOnServer.CurrentOutfit.body, body);
        yield return GetOutfitItem(_outfitOnServer.CurrentOutfit.legs, legs);
        yield return GetOutfitItem(_outfitOnServer.CurrentOutfit.skin, skin);
        SCREENBLOCK.SetActive(false);
        _loadSystem?.OnOutfitLoaded?.Invoke();
    }

    private IEnumerator GetOutfitItem(int skinID, SpriteRenderer spriteR)
    {
        if (skinID == 0){ spriteR.sprite = default; yield break; }
        foreach (var inventoryItem in _inventoryServer.PlayerItems)
        {
            if (skinID == inventoryItem.item_id && inventoryItem.sprite != null)
            {
                spriteR.sprite = inventoryItem.sprite;
                yield break;
            }
            if (skinID == inventoryItem.item_id && inventoryItem.sprite == null)
            {
                yield return _inventoryServer.GetOutfitSkin(inventoryItem);
                spriteR.sprite = inventoryItem.sprite;
                break;
            }
        }
    }

    private void OnDisable()
    {
        this.StopAllCoroutines();
    }
}
