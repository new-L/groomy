using TMPro;
using UnityEngine;

public class RatingElement : MonoBehaviour
{
    [SerializeField] private TMP_Text _place;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _count;

    public void SetDetail(Rating rating)
    {
        _place.text = rating.place.ToString();
        _name.text = rating.name;
        _count.text = rating.count.ToString();
    }
}
