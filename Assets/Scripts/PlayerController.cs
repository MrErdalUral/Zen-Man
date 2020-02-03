using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Movement _movement;
    private bool _isStarted;
    [SerializeField]
    public GameObject _playerTrail;
    // Start is called before the first frame update
    void Awake()
    {
        _isStarted = false;
        transform.localScale = Vector3.zero;
        _movement = GetComponent<Movement>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_isStarted) return;
        var vector2 = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (Input.GetMouseButton(0))
        {
            vector2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            vector2 -= (Vector2)transform.position;
            vector2 = vector2.normalized;
        }
        if (vector2.magnitude > 1)
            vector2 = vector2.normalized;
        var axisInput = vector2;
        _movement.Move(axisInput);
        if (_playerTrail != null)
            GameObject.Instantiate(_playerTrail, transform.position, Quaternion.identity);
    }

    public IEnumerator StartPlayer()
    {
        var time = new WaitForSeconds(1f / 60);
        for (int i = 0; i < 60; i++)
        {
            yield return time;
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, (i + 1) * (1f / 60));
        }
        _isStarted = true;
    }

    public IEnumerator EndPlayer()
    {
        var time = new WaitForSeconds(1f / 60);
        for (int i = 0; i < 60; i++)
        {
            yield return time;
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, (i + 1) * (1f / 60));
        }
        _isStarted = false;
    }
}
