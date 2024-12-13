using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameLoader : MonoBehaviour
{


    [SerializeField]
    private GameObject _borry;
    private static bool _isBorryActivate = false;
    [SerializeField]
    private static Dictionary<string, bool> _tables = new Dictionary<string, bool>();
    [SerializeField]
    private GameObject _loader;

    public static Dictionary<string, bool> Tables { get => _tables; set => _tables = value; }
    public static bool IsBorryActivate { get => _isBorryActivate; set => _isBorryActivate = value; }

    private void Awake()
    {
        Tables.Clear();
        Tables.Add(DBTablesName.UserCurrency, false);
    }

    private void Start()
    {
        Actions.OnStartLoad?.Invoke();
        _borry.SetActive(false);
        IsBorryActivate = false;
    }

    public void SetLoader(GameObject loader)
    {
        _loader = loader;
    }

    public void SetBorryActive(bool isActive)
    {
        IsBorryActivate = isActive;
    }
    //private void FixedUpdate()
    //{
    //  Debug.Log("IsBorryActivate: " + IsBorryActivate);
    //}
    private void OnEnable()
    {
        Actions.OnStartLoad += ActivateLoadPanel;
        Actions.OnListCreated += DisableLoadPanel;
        Actions.OnUserDatasLoad += IsTablesLoaded;
    }

    private void OnDisable()
    {
        Actions.OnStartLoad -= ActivateLoadPanel;
        Actions.OnListCreated -= DisableLoadPanel;
        Actions.OnUserDatasLoad -= IsTablesLoaded;
    }

    private void ActivateLoadPanel()
    {
        if (_loader == null) return;
        _loader.SetActive(true);
        _borry.SetActive(IsBorryActivate);
               
    }

    private void DisableLoadPanel()
    {
        if (_loader == null) return;
        _borry.SetActive(IsBorryActivate);
        _loader.SetActive(false);
    }

    private void IsTablesLoaded()
    {
        foreach (var item in Tables)
        {
            if (!item.Value) return;
        }
        Actions.OnListCreated?.Invoke();
    }
}
