using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPointMovement : MonoBehaviour
{
    private float _time;

    private float _angleSeed;
    [SerializeField] private float _speed = 10;
    [SerializeField] private float _maxDistance = 4;

    private float _distanceSeed;
    // Start is called before the first frame update
    void Start()
    {
        _angleSeed = Random.Range(0f, 1000f);
        _distanceSeed = Random.Range(0f, 1000f);
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime * _speed / 1000;
        var time2 = _time + Time.deltaTime;
        var angle = 720 * Mathf.PerlinNoise(_time / 3, _angleSeed);

        var distance = Mathf.PerlinNoise(_time, _distanceSeed);
        angle += 180 * Mathf.PerlinNoise(time2 / 3, _angleSeed);
        distance += Mathf.PerlinNoise(_time, _distanceSeed) * 0.5f;
        distance /= 1.5f;
        var pos = DegreeToVector2(angle).normalized * distance * _maxDistance;
        pos.x *= 1.25f;
        transform.position = pos;
    }
    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }
}
