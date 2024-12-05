using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MusicList : MonoBehaviour
{
    [SerializeField] private Music _music;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _content;

    [SerializeField] private SongManager _songManager;
    public void SetUpList()
    {
        if (_music.Melodies.Length == 0 || _music.Melodies == null)
        {
            Actions.OnListCreated?.Invoke();
            return;
        }
        foreach (var item in _music.Melodies)
        {
            var instance = Instantiate(_prefab);
            instance.transform.SetParent(_content, false);
            instance.GetComponent<MusiListElement>().SetMelodyInfo(item, _songManager, _music);
            break;

        }
        Actions.OnListCreated?.Invoke();
    }
}
