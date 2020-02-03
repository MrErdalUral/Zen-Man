using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGamePanelFader : MonoBehaviour
{
    public Text _EndGameText;
    public Image _EndGamePanel;

    public IEnumerator FadeIn()
    {
        var levelText = " meditation level" + GameManager.Instance.Level;
        if (GameManager.Instance.LevelValue > 7)
            levelText = " nirvana";
        _EndGameText.text = $"You have reached {levelText} in {GameManager.Instance.Timer:N1} Seconds";
        var time = new WaitForSeconds(1f / 60);
        for (int i = 0; i < 240; i++)
        {
            yield return time;
            var f = (i + 1) * (1f / 240);
            //_EndGamePanel.color = Color.Lerp(new Color(0, 0, 0, 0), Color.white, f * 2);
            _EndGameText.color = Color.Lerp(new Color(1, 1, 1, 0), Color.black, f);
        }

        _EndGameText.text = $"You have reached {levelText} in {(int)GameManager.Instance.Timer} Seconds\n\nPress Any Key To Restart";

    }
}
