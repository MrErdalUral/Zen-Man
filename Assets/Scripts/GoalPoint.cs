using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GoalPoint : MonoBehaviour
{
    [SerializeField] private bool _isOnPoint;
    private bool _isStarted;
    public bool IsStarted => _isStarted;
    [SerializeField] private GameObject _wave;
    [SerializeField] private GameObject _strongWave;

    public float angularSpeed = 1;
    private float _currentAngularSpeed;
    public float maxangularSpeed = 250;
    private Vector3 _originalScale;

    [SerializeField]
    Vector3 _minimumScale = new Vector3(0.2f, 0.2f, 1);


    private float _progress;

    private bool _slowingDown;
    // Start is called before the first frame update
    void Awake()
    {
        _originalScale = transform.localScale;
        transform.localScale = Vector3.zero;
        _isStarted = false;

    }

    private IEnumerator Waves()
    {
        var wait = new WaitForSeconds(0.01f);
        while (true)
        {
            yield return wait;
            _progress += 0.01f;
            if (_progress >= (GameManager.LevelTime - GameManager.Instance.ProgressValue) + 0.1f)
            {
                if (_isOnPoint)
                {
                    Instantiate(_wave, transform.position, Quaternion.identity, transform);
                    FindObjectOfType<PointAudioController>().Play(1 + Random.Range(-0.1f, 0.1f));
                    _progress = 0;
                }
            }
        }
    }

    public void StrongWave()
    {
        Instantiate(_strongWave, transform.position, Quaternion.identity);
        StartCoroutine(SlowDown());
    }

    public IEnumerator SlowDown()
    {
        _slowingDown = true;
        var time = new WaitForSeconds(1f / 60);
        for (int i = 0; i < 120; i++)
        {
            yield return time;
            var f = (i + 1) * (1f / 120);
            _currentAngularSpeed = Mathf.Lerp(_currentAngularSpeed, angularSpeed, f);
            transform.localScale = Vector3.Lerp(transform.localScale, _minimumScale, f);


        }
        _slowingDown = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (!_isStarted) return;
        if (!_slowingDown)
        {
            var f = GameManager.Instance.ProgressValue / GameManager.LevelTime;
            _currentAngularSpeed = Mathf.Lerp(angularSpeed, maxangularSpeed, f);
            transform.localScale = Vector3.Lerp(_minimumScale, _originalScale, f);
        }


        var r = transform.eulerAngles.z;
        r += Time.deltaTime * _currentAngularSpeed;
        transform.rotation = Quaternion.AngleAxis(r, Vector3.forward);

        GameManager.Instance.SetMeditationPoint(_isOnPoint);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isStarted) return;
        if (!collision.CompareTag("Player")) return;

        _isOnPoint = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!_isStarted) return;
        if (!collision.CompareTag("Player")) return;

        _isOnPoint = false;
    }

    public IEnumerator StartGoalPoint()
    {
        _slowingDown = true;
        var time = new WaitForSeconds(1f / 60);
        for (int i = 0; i < 60; i++)
        {
            yield return time;
            transform.localScale = Vector3.Lerp(Vector3.zero, _minimumScale, (i + 1) * (1f / 60));
        }

        _isStarted = true;
        if (_wave)
            StartCoroutine(Waves());
        _slowingDown = false;

    }

    public IEnumerator EndGoalPoint()
    {
        _slowingDown = true;
        var time = new WaitForSeconds(1f / 60);
        for (int i = 0; i < 60; i++)
        {
            yield return time;
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, (i + 1) * (1f / 60));
        }

        _isStarted = false;
    }
}
