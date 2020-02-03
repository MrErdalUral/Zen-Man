using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject _redWave;
    [SerializeField] private GameObject _particle;
    private Transform _playerTransform;
    private bool _cancelSpawn;
    // Start is called before the first frame update
    void Start()
    {
        _cancelSpawn = false;
        StartCoroutine(Spawn());
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        //GetComponent<Movement>().SetVelocity(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized);
    }

    private IEnumerator Spawn()
    {
        GetComponentInChildren<BoxCollider2D>().enabled = false;
        var wait = new WaitForSeconds(1f / 60);
        for (int i = 0; i < 60; i++)
        {
            if (_cancelSpawn) yield break;
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, (i + 1) * (1f / 60));
            yield return wait;
        }
        GetComponentInChildren<BoxCollider2D>().enabled = true;
    }

    public IEnumerator Die()
    {
        GetComponentInChildren<BoxCollider2D>().enabled = false;
        _cancelSpawn = true;
        var wait = new WaitForSeconds(1f / 60);
        for (int i = 0; i < 60; i++)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, (i + 1) * (1f / 60));
            yield return wait;
        }

        FindObjectOfType<EnemySpawner>().SpawnEnemy(2);

        Destroy(gameObject);
    }
    void FixedUpdate()
    {
        if (GameManager.Instance.State == GameState.Ending || GameManager.Instance.State == GameState.End || GameManager.Instance.State == GameState.ReadyToRestart)
            Destroy(gameObject);
        var v = (_playerTransform.position - transform.position).normalized;
        GetComponent<Movement>().Move(v);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.DamageMeditationPoint();
            //Instantiate(_redWave, transform.position, Quaternion.identity);
            Instantiate(_particle, transform.position, Quaternion.identity);
            StartCoroutine(Die());
        }
        //else if (ShockwavePrefab != null && collision.gameObject.CompareTag("Wall"))
        //{
        //    Instantiate(ShockwavePrefab, collision.GetContact(0).point, Quaternion.identity);

        //}
    }


    public void JustDie()
    {
        StartCoroutine(JustDieRoutine());
    }

    public IEnumerator JustDieRoutine()
    {
        GetComponentInChildren<BoxCollider2D>().enabled = false;
        _cancelSpawn = true;
        var wait = new WaitForSeconds(1f / 60);
        for (int i = 0; i < 60; i++)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, (i + 1) * (1f / 60));
            yield return wait;
        }
        Destroy(gameObject);
    }
}
