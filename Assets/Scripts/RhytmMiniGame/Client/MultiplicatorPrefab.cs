using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MultiplicatorPrefab : MonoBehaviour
{

    [SerializeField] private Image _panelBackground;
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _multiplicator;
    [SerializeField] private TMP_Text _coinsRewardText;
    [SerializeField] private TMP_Text _ratingRewardText;


    public void Set(bool isEven, string title, string multiplicatior, string coinsReward, string ratingReward)
    {
        if (isEven) _panelBackground.color = Black();
        else _panelBackground.color = White();
        _title.text = title;
        _multiplicator.text = multiplicatior;
        _coinsRewardText.text = $"+{coinsReward}";
        _ratingRewardText.text = $"+{ratingReward}";
    }

    private Color32 White() => new Color32(255, 255, 255, 20);
    private Color32 Black() => new Color32(0, 0, 0, 80);
}
