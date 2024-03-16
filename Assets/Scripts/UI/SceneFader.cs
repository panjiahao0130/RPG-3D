using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneFader : Singleton<SceneFader>
{
    private CanvasGroup canvasGroup;
    public float fadeOutDuration;
    public float fadeInDuration;
    protected override void Awake()
    {
        base.Awake();
        canvasGroup = GetComponent<CanvasGroup>();
        DontDestroyOnLoad(this);
    }

    public IEnumerator FadeOut(float time)
    {
        while (canvasGroup.alpha<1)
        {
            canvasGroup.alpha += Time.deltaTime / time;
            yield return null;
        }
    }
    public IEnumerator FadeIn(float time)
    {
        while (canvasGroup.alpha!=0)
        {
            canvasGroup.alpha -= Time.deltaTime / time;
            yield return null;
        }
        
    }

    public IEnumerator FadeOutAndIn()
    {
        yield return FadeOut(fadeOutDuration);
        yield return FadeIn(fadeInDuration);
    }
}
