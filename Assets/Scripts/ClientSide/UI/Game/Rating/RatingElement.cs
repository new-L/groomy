using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RatingElement : MonoBehaviour
{
    [SerializeField] private TMP_Text _place;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _count;
    [SerializeField] private Image _placeIcon;

    private const string _PATH = "Art/UI/Rating/";

    public void SetDetail(Rating rating, int place)
    {
        _place.text = rating.place.ToString();
        _name.text = rating.name;
        _count.text = rating.count.ToString();
        if (place <= 3)
        {
            _placeIcon.gameObject.SetActive(true);
            _placeIcon.sprite = SetLeaderPlaces(place);
        }
        else
            _placeIcon.gameObject.SetActive(false);
    }

    private Sprite SetLeaderPlaces(int place)
    {
        switch (place)
        {
            case 1: return Resources.Load<Sprite>($"{_PATH}Gold");
            case 2: return Resources.Load<Sprite>($"{_PATH}Silver");
            case 3: return Resources.Load<Sprite>($"{_PATH}Bronze");
            default: return Resources.Load<Sprite>($"{_PATH}Bronze");
        }
    }
}
