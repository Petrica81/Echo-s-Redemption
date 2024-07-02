using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
public class StartMenuButtons : BaseButtons
{
    public static event Delegates.PlayAction _onPlay;
    [SerializeField] private GameObject startMenuFirst;
    [SerializeField] private GameObject settingsMenuFirst;
    private new void Start()
    {
        base.Start();
        EventSystem.current.SetSelectedGameObject(startMenuFirst);
    }
    private void Update()
    {
        if((Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.UpArrow)) && EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(startMenuFirst);
        }
    }
    public void Play()
    {
        StartCoroutine(PlayCoro());
    }

    private IEnumerator PlayCoro()
    {
        ButtonSound();
        yield return new WaitUntil(() => audioSource.isPlaying == false);
        _onPlay?.Invoke();
        Destroy(transform.parent.gameObject);
    }

    public void Settings()
    {
        ButtonSound();
        EventSystem.current.SetSelectedGameObject(settingsMenuFirst);
    }

    public void Quit()
    {
        ButtonSound();
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
