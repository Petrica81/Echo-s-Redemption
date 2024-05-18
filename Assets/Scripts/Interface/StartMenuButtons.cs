using System.Collections;
using UnityEditor;
using UnityEngine;

public class StartMenuButtons : BaseButtons
{
    public static event Delegates.PlayAction OnPlay;

    public void Play()
    {
        StartCoroutine(PlayCoro());
    }

    private IEnumerator PlayCoro()
    {
        ButtonSound();
        yield return new WaitUntil(() => audioSource.isPlaying == false);
        OnPlay?.Invoke();
        transform.parent.gameObject.SetActive(false);
    }

    public void Settings()
    {
        ButtonSound();
    }

    public void Quit()
    {
        ButtonSound();
        Application.Quit();
    }
}
