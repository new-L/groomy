using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryList : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] private InventoryServer _inventoryServer;
    [SerializeField] private GameObject _outfit;

    [Header("Scroll View")]
    [SerializeField] private RectTransform _content;
    [SerializeField] private RectTransform _itemPrefab;

    [Header("Alert")]
    [SerializeField] private TMP_Text _alert;

    [Header("LoadSystem")]
    [SerializeField] private LoadSystem _loadSystem;

    private SkinTypes _skinType = SkinTypes.HATID;

    public SkinTypes SkinType { get => _skinType; private set => _skinType = value; }

    public void Head() { StartLoad(SkinTypes.HATID); }
    public void Body() { StartLoad(SkinTypes.BODYID); }
    public void Legs() { StartLoad(SkinTypes.LEGSID); }
    public void Skin() { StartLoad(SkinTypes.SKINID); }

    public void StartLoad(SkinTypes type)
    {
        ClearListView();
        _alert.gameObject.SetActive(false);
        SkinType = type;
        OnProductsLoaded();
    }
    public void OnProductsLoaded()
    {
        bool isExist = false;
        foreach (var item in _inventoryServer.PlayerItems)
        {
            if(item.type_id == (int)SkinType) { isExist = true; break; }
        }
        if (!isExist)
        {
            _alert.text = "Пусто!";
            _alert.gameObject.SetActive(true);
            return;
        }
        foreach (var item in _inventoryServer.PlayerItems)
        {
            if (item.type_id == (int)SkinType)
            {
                InitializeItem(item, _itemPrefab);
            }
        }
        _loadSystem?.OnUILoaded?.Invoke();
    }
    private void InitializeItem(Items item, RectTransform prefab)
    {
        var instance = GameObject.Instantiate(prefab.gameObject) as GameObject;
        instance.GetComponent<OutfitItem>().SetUI(item, _outfit);
        instance.transform.SetParent(_content, false);
    }

    private bool CheckArrayNullOrEmpty(Items[] products)
    {
        if (products.Length == 0 || products == null)
            return false;
        return true;
    }

    private void ClearListView()
    {
        foreach (Transform child in _content)
        {
            Destroy(child.gameObject);
        }
    }
}
