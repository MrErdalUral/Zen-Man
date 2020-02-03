using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsTextManager : MonoBehaviour
{
    public void Trigger()
    {
        StartCoroutine(FadeInFadeOut());
    }

    private IEnumerator FadeInFadeOut()
    {
        var time = new WaitForSeconds(1f / 60);
        for (int i = 0; i < 180; i++)
        {
            yield return time;
            var f = (i + 1) * (1f / 180);
            //_EndGamePanel.color = Color.Lerp(new Color(0, 0, 0, 0), Color.white, f * 2);
            GetComponent<Text>().color = Color.Lerp(new Color(1, 1, 1, 0), Color.black, f);
        }
        for (int i = 0; i < 180; i++)
        {
            yield return time;
            var f = (i + 1) * (1f / 180);
            //_EndGamePanel.color = Color.Lerp(new Color(0, 0, 0, 0), Color.white, f * 2);
            GetComponent<Text>().color = Color.Lerp(Color.black, new Color(1, 1, 1, 0), f);
        }
    }
}
