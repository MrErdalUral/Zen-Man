using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubbleController : MonoBehaviour
{
    [SerializeField] private Image _bubbleBackgroundImage;
    public SpriteMask Mask;
    public Color Color;

    public IEnumerator StartBubble()
    {
        var renderer = GetComponentInChildren<SpriteRenderer>();
        transform.localScale = Vector3.one;
        if (!_bubbleBackgroundImage) yield break;
        var time = new WaitForSeconds(1f / 60);
        for (int i = 0; i < 60; i++)
        {
            yield return time;
            var f = (i + 1) / 60f;
            _bubbleBackgroundImage.fillAmount = f;
        }
        renderer.color = Color;
    }

    public IEnumerator EndBubble()
    {
        GetComponentInChildren<SpriteMask>().enabled = false;
        GetComponentInChildren<Canvas>().sortingLayerName = "Front";
        var renderer = GetComponentInChildren<SpriteRenderer>();

        if (!_bubbleBackgroundImage) yield break;
        var time = new WaitForSeconds(1f / 60);
        for (int i = 0; i < 600; i++)
        {
            yield return time;
            var f = (i + 1) * (1f / 600);
            _bubbleBackgroundImage.color = Color.Lerp(_bubbleBackgroundImage.color, new Color(0, 0, 0, 0), f);
            var c = Color;
            c.a = 0;
            renderer.color = Color.Lerp(renderer.color, c, f);

        }
        _bubbleBackgroundImage.fillAmount = 0;
        _bubbleBackgroundImage.color = Color.white;

    }

    public IEnumerator SetBubble(float f)
    {
        var time = new WaitForSeconds(1f / 60);
        for (int i = 0; i < 30; i++)
        {
            yield return time;
            _bubbleBackgroundImage.fillAmount = Mathf.Lerp(_bubbleBackgroundImage.fillAmount, f, (i + 1) / 30f);
        }

    }
}
