using System.Collections;
using UnityEngine;
public class StartMenuButtons : BaseButtons
{
    public static event Delegates.PlayAction _onPlay;

    public void Play()
    {
        StartCoroutine(PlayCoro());
    }

    private IEnumerator PlayCoro()
    {
        ButtonSound();
        yield return new WaitUntil(() => audioSource.isPlaying == false);
        _onPlay?.Invoke();
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
