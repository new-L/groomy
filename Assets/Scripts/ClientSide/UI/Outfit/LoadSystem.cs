using UnityEngine;
using UnityEngine.Events;

public class LoadSystem : MonoBehaviour
{
    [SerializeField] private UnityEvent _sceneStart;
    [SerializeField] private UnityEvent _onServerDatasLoaded;
    [SerializeField] private UnityEvent _onUILoaded;

    public UnityEvent SceneStart { get => _sceneStart; private set => _sceneStart = value; }
    public UnityEvent OnServerDatasLoaded { get => _onServerDatasLoaded; private set => _onServerDatasLoaded = value; }
    public UnityEvent OnUILoaded { get => _onUILoaded; private set => _onUILoaded = value; }
}
