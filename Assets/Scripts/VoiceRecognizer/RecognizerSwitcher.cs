using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class RecognizerSwitcher : MonoBehaviour
{
    public List<BaseRecognizer> _gameKeywordRecognizers;
    private SpellRecognizer _spellRecognizer;

    private void Start()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            Permission.RequestUserPermission(Permission.Microphone);

        _spellRecognizer = GetComponent<SpellRecognizer>();
        _gameKeywordRecognizers.Add(GetComponent<GameOnRecognizer>());

        GameOnRecognizer._onSpellCast += HandleOnSpellCast;
        SpellRecognizer._onFinishSpellCast += HandleOnFinishSpellCast;
    }

    IEnumerator SwitchSpellToGame()
    {
        _spellRecognizer.enabled = false;
        yield return new WaitForSeconds(0.1f);
        foreach(BaseRecognizer recognizer in _gameKeywordRecognizers)
        {
            recognizer.enabled = true;
        }
    }

    IEnumerator SwitchGameToSpell()
    {
        foreach (BaseRecognizer recognizer in _gameKeywordRecognizers)
        {
            recognizer.enabled = false;
        }
        yield return new WaitForSeconds(0.1f);
        _spellRecognizer.enabled = true;
    }
    private void HandleOnSpellCast()
    {
        StartCoroutine(SwitchGameToSpell());
    }
    private void HandleOnFinishSpellCast()
    {
        StartCoroutine(SwitchSpellToGame());
    }
    private void OnDestroy()
    {
        GameOnRecognizer._onSpellCast -= HandleOnSpellCast;
        SpellRecognizer._onFinishSpellCast -= HandleOnFinishSpellCast;
    }
}
