using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Loader : MonoBehaviour
{
    [SerializeField] private GameObject _loader;

    private static bool isLoadComplete = false;
    private static Dictionary<string, bool> _loadedTables = new Dictionary<string, bool>();

    public static Dictionary<string, bool> LoadedTables { get => _loadedTables; set => _loadedTables = value; }
    public static bool IsLoadComplete { get => isLoadComplete; set => isLoadComplete = value; }



    private void FixedUpdate()
    {
        if (Loader.IsLoadComplete && !Loader.LoadedTables.ContainsValue(false))
        {
            Debug.Log("OnTaskSettingsLoad Invoked!");
            Actions.OnTaskSettingsLoad?.Invoke();
            Loader.IsLoadComplete = false;
        }
    }
    private void OnEnable()
    {
        Actions.OnStartLoad += ActivateLoadPanel;
        Actions.OnListCreated += DisableLoadPanel;
    }

    private void OnDisable()
    {
        Actions.OnStartLoad -= ActivateLoadPanel;
        Actions.OnListCreated -= DisableLoadPanel;
    }

    private void ActivateLoadPanel()
    {
        _loader.SetActive(true);
    }

    private void DisableLoadPanel()
    {
        _loader.SetActive(false);
    }
}
