using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAudioController : MonoBehaviour
{
    public float Offset = 0;
    public float[] PitchArray = new[] { 0.74915f, 0.84088f, 0.89088f, 1.12242f, 1.25979f };
    public void Play(float p)
    {
        GetComponent<AudioSource>().pitch = PitchArray[Random.Range(0, PitchArray.Length)] + Offset;
        GetComponent<AudioSource>().Play();
    }
}
