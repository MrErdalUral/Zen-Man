using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientAudio : MonoBehaviour
{
    public void Play()
    {
        GetComponent<AudioSource>().Play();
    }
}
