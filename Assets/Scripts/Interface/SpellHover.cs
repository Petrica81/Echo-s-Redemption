using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellHover : MonoBehaviour
{
    private void Awake()
    {
        SpellRecognizer._onStartSpellCast += Show;
        SpellRecognizer._onFinishSpellCast += Hide;
        this.gameObject.SetActive(false);
    }
    private void Show()
    {
        this.gameObject.SetActive(true);
        Time.timeScale = 0.0f;
    }
    private void Hide()
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }
    private void OnDestroy()
    {
        Time.timeScale = 1.0f;
        SpellRecognizer._onStartSpellCast -= Show;
        SpellRecognizer._onFinishSpellCast -= Hide;
    }
}
