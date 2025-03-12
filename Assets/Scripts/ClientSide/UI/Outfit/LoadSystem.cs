using UnityEngine;
using UnityEngine.Events;

public class LoadSystem : MonoBehaviour
{
    [SerializeField] private UnityEvent _sceneStart;
    [SerializeField] private UnityEvent _onServerInventoryLoaded;
    [SerializeField] private UnityEvent _onUILoaded;
    [SerializeField] private UnityEvent _onOutfitLoaded;

    public UnityEvent SceneStart { get => _sceneStart; private set => _sceneStart = value; }
    public UnityEvent OnServerInventoryLoaded { get => _onServerInventoryLoaded; private set => _onServerInventoryLoaded = value; }
    public UnityEvent OnUILoaded { get => _onUILoaded; private set => _onUILoaded = value; }
    public UnityEvent OnOutfitLoaded { get => _onOutfitLoaded; private set => _onOutfitLoaded = value; }
}
