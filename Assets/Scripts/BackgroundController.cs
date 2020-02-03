using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField]
    public LevelSprites[] LevelSprites;

    public void EnableSprites(int level)
    {
        if (level >= LevelSprites.Length) return;
        var levelSprites = LevelSprites[level];
        foreach (var levelSprite in levelSprites.BackgroundSprites)
        {
            StartCoroutine(FadeIn(levelSprite));
        }

        foreach (var disableSprite in levelSprites.DisableSprites)
        {
            StartCoroutine(FadeOut(disableSprite));
        }
    }

    public IEnumerator DisableAllSprites()
    {
        for (var i = LevelSprites.Length - 1; i > -1; i--)
        {
            var levelSprite = LevelSprites[i];
            foreach (var sprite in levelSprite.BackgroundSprites)
            {
                if (sprite.enabled)
                    yield return FadeOut(sprite);
            }
        }
    }


    public void DisableAllSpritesExceptLastOnes()
    {
        for (var index = 0; index < LevelSprites.Length - 1; index++)
        {
            var levelSprite = LevelSprites[index];
            foreach (var sprite in levelSprite.BackgroundSprites)
            {
                StartCoroutine(FadeOut(sprite));
            }
        }
    }

    private IEnumerator FadeOut(SpriteRenderer sprite)
    {
        sprite.color = new Color(0, 0, 0, 0);
        var wait = new WaitForSeconds(1f / 60);
        for (int i = 0; i < 30; i++)
        {
            sprite.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), (i + 1) * (1f / 30));
            yield return wait;
        }
        sprite.gameObject.SetActive(false);
    }

    private IEnumerator FadeIn(SpriteRenderer sprite)
    {
        sprite.gameObject.SetActive(true);
        sprite.color = new Color(0, 0, 0, 0);
        var wait = new WaitForSeconds(1f / 60);
        for (int i = 0; i < 180; i++)
        {
            sprite.color = Color.Lerp(new Color(0, 0, 0, 0), Color.white, (i + 1) * (1f / 180));
            yield return wait;
        }
    }
}

[System.Serializable]
public class LevelSprites
{
    public SpriteRenderer[] BackgroundSprites;
    public SpriteRenderer[] DisableSprites;
}