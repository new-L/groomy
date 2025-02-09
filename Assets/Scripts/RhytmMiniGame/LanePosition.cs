using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanePosition : MonoBehaviour
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private Transform _lane;
    private void Start()
    {
        SetPosition();
    }
    public void SetPosition()
    {
        _lane.position = new Vector3(_rect.position.x, _lane.position.y, _lane.position.z);
    }

}
