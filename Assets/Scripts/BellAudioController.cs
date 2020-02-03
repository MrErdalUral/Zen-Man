using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellAudioController : MonoBehaviour
{
    [SerializeField] private Vector2 _playInterval = new Vector2(0, 1);
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _bellAudioClip;

    [SerializeField] private float _radius = 1;

    private float _pitch;
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        _audioSource.pitch = 1+Random.Range(-0.05f,0.05f);
        _audioSource.clip = _bellAudioClip;
        _audioSource.time = _playInterval.x;
        _audioSource.Play();
    }

}
