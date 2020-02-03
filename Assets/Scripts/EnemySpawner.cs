using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyController[] EnemyPrefabs;
    private List<EnemyController> _currentEnemyList;
    private List<Vector2> _spawnPositions;
    private int _spawnCount;

    // Start is called before the first frame update
    void Start()
    {
        _spawnCount = 0;
        _currentEnemyList = new List<EnemyController>();
        _spawnPositions = new List<Vector2>();
        var coliders = GetComponentsInChildren<EdgeCollider2D>();
        foreach (var colider in coliders)
        {
            _spawnPositions.AddRange(colider.points);
        }
    }

    public IEnumerator SpawnEnemies(int count)
    {
        var time = GameManager.LevelTime / (count * 2);
        for (int i = 0; i < count; i++)
        {
            yield return SpawnEnemyRoutine(time);
        }
        yield return null;
    }

    public IEnumerator SpawnEnemyRoutine(float time)
    {
        if (GameManager.Instance.State == GameState.Ending || GameManager.Instance.State == GameState.End || GameManager.Instance.State == GameState.ReadyToRestart) yield break;
        yield return new WaitForSeconds(time);
        if (_spawnCount == 0)
        {
            FindObjectOfType<InstructionsTextManager>().Trigger();
        }
        _spawnCount++;
        _currentEnemyList.Add(Instantiate(EnemyPrefabs[Random.Range(0,EnemyPrefabs.Length)],
            _spawnPositions[Random.Range(0, _spawnPositions.Count)] * 0.9f, Quaternion.identity));
    }

    public void SpawnEnemy(float time)
    {
        StartCoroutine(SpawnEnemyRoutine(time));
    }


    public IEnumerator StartLevel(int level)
    {
        yield return new WaitForSeconds(0.5f);
        while (_currentEnemyList.Count > 0)
        {
            if (_currentEnemyList[0] != null)
                _currentEnemyList[0].JustDie();
            _currentEnemyList.RemoveAt(0);
        }
        yield return SpawnEnemies(level);
    }

    public IEnumerator EndGame()
    {
        yield return new WaitForSeconds(0.1f);
        while (_currentEnemyList.Count > 0)
        {
            if (_currentEnemyList[0] != null)
                _currentEnemyList[0].JustDie();
            _currentEnemyList.RemoveAt(0);
        }
    }
}
