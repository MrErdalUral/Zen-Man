using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextSetter : MonoBehaviour
{
    [SerializeField] private Text _levelText;
    [SerializeField] private Text _progressText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_levelText != null)
            _levelText.text = $"Meditation Level: {GameManager.Instance.Level}";
        if (_progressText != null)
            _progressText.text = $"Meditation Progress: {GameManager.Instance.Progress} / {GameManager.LevelTime}";
    }
}
