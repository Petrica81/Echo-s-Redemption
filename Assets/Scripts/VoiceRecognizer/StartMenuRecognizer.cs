using UnityEngine;

public class StartMenuRecognizer : BaseRecognizer
{
    [SerializeField]
    private StartMenuButtons startMenuButtons;

    void Awake()
    {
        base.actions.Add("play", startMenuButtons.Play);
        base.actions.Add("settings", startMenuButtons.Settings);
        base.actions.Add("Quit", startMenuButtons.Quit);
    }
}
