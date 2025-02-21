using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BorrySound : MonoBehaviour
{
    [SerializeField] private GameObject _borry;
    [SerializeField] private List<AudioClip> _clips;
    [SerializeField] private AudioSource _audioSource;

    private void Update()
    {
        if (!SystemInfo.operatingSystem.ToLower().Contains("ios"))
        {
            if (!SystemInfo.operatingSystem.ToLower().Contains("android"))
            {
                if (Input.GetMouseButtonUp(0))
                {
                    Play();
                }
            }
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            Play();
        }
    }

    public void Play()
    {
        var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.transform != null && hit.transform.gameObject == _borry)
        {
            _audioSource.clip = RandomClip();
            _audioSource.Play();
        }
    }

    private AudioClip RandomClip() => _clips[Random.Range(0, _clips.Count)];
}
