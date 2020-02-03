using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { Start, Starting, Play, ChangeLevel, End, Ending, ReadyToRestart }
public class GameManager : MonoBehaviour
{
    public float Health = 15;
    [SerializeField] private GameState _gameState;
    public const float LevelTime = 10f;
    public static GameManager Instance;
    [SerializeField] float _points;

    [SerializeField] private float _pointIncreaseRate = 1;
    [SerializeField] private float _pointDecreaseRate = 0.25f;
    [SerializeField] private float _damageDecreaseAmount = 1;
    [SerializeField] private int _level = 0;
    public string Level => _level.ToString();
    public int LevelValue => _level;
    public string Progress => _points.ToString("N");
    public float ProgressValue => _points;

    public GameState State
    {
        get { return _gameState; }
        set { _gameState = value; }
    }

    public float Timer;
    private bool _endGameShown;
    private float _damageCooldown = 0;
    // Start is called before the first frame update
    void Awake()
    {
        _gameState = GameState.Start;
        Instance = this;
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        if (State == GameState.Start)
        {
            if (Input.anyKeyDown)
            {
                StartCoroutine(StartGame());
            }
        }
        else if (State == GameState.End)
        {
            if (Input.anyKeyDown && !_endGameShown)
            {
                _endGameShown = true;
                StartCoroutine(ShowGameEnd());
            }
        }
        else if (State == GameState.ReadyToRestart)
        {
            if (Input.anyKeyDown)
                RestartGame();
        }
        else if (State == GameState.Play)
        {
            Timer += Time.deltaTime;
        }
        _damageCooldown -= Time.deltaTime;
    }

    private void RestartGame()
    {
        Debug.Log("Restart");
        SceneManager.LoadScene(0);
    }

    private IEnumerator ShowGameEnd()
    {
        FindObjectOfType<EndAudioController>().Play();
        Debug.Log("ShowGameEnd");
        yield return StartCoroutine(FindObjectOfType<BackgroundController>().DisableAllSprites());
        yield return StartCoroutine(FindObjectOfType<EndGamePanelFader>().FadeIn());

        State = GameState.ReadyToRestart;

    }

    private IEnumerator StartGame()
    {
        State = GameState.Starting;
        //State = GameState.ChangeLevel;
        yield return StartCoroutine(FindObjectOfType<BubbleController>().StartBubble());
        StartCoroutine(FindObjectOfType<GoalPoint>().StartGoalPoint());
        yield return StartCoroutine(FindObjectOfType<PlayerController>().StartPlayer());
        Timer = 0;
        yield return StartLevel(_level);
    }

    public void SetMeditationPoint(bool isOnPoint)
    {
        if (State != GameState.Play) return;

        if (isOnPoint)
            _points += Time.deltaTime * _pointIncreaseRate;
        else
            _points -= Time.deltaTime * _pointDecreaseRate;

        if (_points < 0) _points = 0;
        if (_points > GameManager.LevelTime)
        {
            _points = 0;
            _level++;
            StartLevel(_level);
        }
    }

    public void SetMeditationPoint(float points)
    {
        if (State != GameState.Play) return;
        _points += points;
        if (_points < 0) _points = 0;
        if (_points > GameManager.LevelTime)
        {
            _points = 0;
            _level++;
            StartCoroutine(StartLevel(_level));
        }
    }

    private IEnumerator StartLevel(int level)
    {

        if (level > 7)
        {
            StartCoroutine(EndGame());
        }
        else if (State != GameState.End && State != GameState.Ending && State != GameState.ReadyToRestart)
        {
            if (level > 3)
                FindObjectOfType<AmbientAudio>().Play();
            State = GameState.ChangeLevel;
            FindObjectOfType<BellAudioController>().PlaySound();
            FindObjectOfType<GoalPoint>().StrongWave();
            FindObjectOfType<BackgroundController>().EnableSprites(level);
            StartCoroutine(FindObjectOfType<EnemySpawner>().StartLevel(level));
            State = GameState.Play;
        }
        return null;
    }

    private IEnumerator EndGame()
    {
        State = GameState.Ending;
        Debug.Log("End Game");
        FindObjectOfType<BackgroundController>().EnableSprites(_level);
        StartCoroutine(FindObjectOfType<EnemySpawner>().EndGame());
        StartCoroutine(FindObjectOfType<GoalPoint>().EndGoalPoint());
        StartCoroutine(FindObjectOfType<PlayerController>().EndPlayer());
        StartCoroutine(FindObjectOfType<BubbleController>().EndBubble());
        yield return new WaitForSeconds(3f);
        State = GameState.End;

    }

    public void DamageMeditationPoint()
    {
        if (State != GameState.Play) return;
        if (_damageCooldown > 0) return;
        _points -= _damageDecreaseAmount;
        Health--;

        if (Health < 0)
            StartCoroutine(EndGame());
        else
            StartCoroutine(FindObjectOfType<BubbleController>().SetBubble(Health / 15));

        if (_points < 0) _points = 0;
        _damageCooldown = 0.5f;

    }
}
