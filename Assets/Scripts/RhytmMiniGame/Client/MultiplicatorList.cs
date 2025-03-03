using UnityEngine;

public class MultiplicatorList : MonoBehaviour
{

    private int _evenIndex = 0;
    [Header("List")]
    [SerializeField] private GameObject _prefab;
    [SerializeField] private GameObject _taskListView;
    [SerializeField] private Transform _content;
    [SerializeField] private AudioSource _audioSource;


    public void ResetEvenIndex()
    {
        _evenIndex = 0;
        ClearList(_content);
    }

    public void SetUpListElement(string title, string multiplicator, string coinsReward, string ratingReward)
    {
        _evenIndex += 1;
        _audioSource.Play();
        var instance = Instantiate(_prefab);
        instance.transform.SetParent(_content, false);
        if(_evenIndex%2 == 0)
        instance.GetComponent<MultiplicatorPrefab>().Set(true, title, multiplicator, coinsReward, ratingReward);
        else
            instance.GetComponent<MultiplicatorPrefab>().Set(false, title, multiplicator, coinsReward, ratingReward);
        //instance.GetComponent<MusiListElement>().SetMelodyInfo(item, _music);
        //OnListLoaded?.Invoke();
    }


    private void ClearList(Transform content)
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }
}
