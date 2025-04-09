using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSoundtrack : MonoBehaviour
{
    [SerializeField] private AudioClip[] _audioClips;
    [SerializeField] private AudioSource _audioSource;
    void Start()
    {
        PlayNext();
    }

    public void PlayNext()
    {
        _audioSource.clip = _audioClips[Random.Range(0, _audioClips.Length - 1)];
        StartCoroutine(Waiter(_audioSource.clip));
        _audioSource.Play();
    }

    public void Pause()
    {
        _audioSource.Pause();
    }

    private IEnumerator Waiter(AudioClip clip)
    {
        yield return new WaitForSeconds(clip.length + Time.deltaTime);
        PlayNext();
    }
}
