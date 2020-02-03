using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAudioController : MonoBehaviour
{

    public void Play()
    {
        GetComponent<AudioSource>().Play();
    }

}
