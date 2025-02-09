using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RatingPanel : MonoBehaviour
{

    [Header("Loader")]
    [SerializeField] private InGameLoader _inGameLoader;
    [SerializeField] private GameObject _loader;

    [Header("Scroll View")]
    [SerializeField] private RectTransform _content;
    [SerializeField] private ScrollRect _scroll;
    [SerializeField] private RectTransform _ratingPrefab;
    [SerializeField] private TMP_Text _selfRatingCount;
    [SerializeField] private TMP_Text _selfRatingPlace;
    [SerializeField] private TMP_Text _selfName;

    [Header("Server")]
    [SerializeField] private RatingServer _rating;

    public void StartLoadRating()
    {
        _inGameLoader.SetLoader(_loader);
        InGameLoader.IsBorryActivate = false;
        Actions.OnStartLoad?.Invoke();
        ClearListView();
        _rating.StartLoadRatingList();
    }

    public void SetRatingPanel()
    {
        if (!CheckArrayNullOrEmpty(_rating.Ratings))
        {
            Actions.OnListCreated?.Invoke();
            return;
        }
        
        for (int i = 0; i < _rating.Ratings.Length; i++)
        {
            if(i != _rating.Ratings.Length - 1)
             InitializeItem(_rating.Ratings[i], _ratingPrefab, i+1);
            if (_rating.Ratings[i].userID == User.Player.user_id)
            {
                if (_rating.Ratings[i].place == 0 && _rating.Ratings[i].count == 0)
                    _selfRatingPlace.text = "?";
                else
                    _selfRatingPlace.text = _rating.Ratings[i].place.ToString();
                _selfRatingCount.text = _rating.Ratings[i].count.ToString();
                _selfName.text = _rating.Ratings[i].name;
            }

        }
        Actions.OnListCreated?.Invoke();
    }

    private void InitializeItem(Rating item, RectTransform prefab, int place)
    {
        var instance = GameObject.Instantiate(prefab.gameObject) as GameObject;
        instance.GetComponent<RatingElement>().SetDetail(item, place);
        instance.transform.SetParent(_content, false);
    }
    private bool CheckArrayNullOrEmpty(Rating[] rating)
    {
        if (rating.Length == 0 || rating == null)
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
