using System.Collections;
using UnityEngine;
using UnityEngine.Android;

public class RecognizerSwitcher : MonoBehaviour
{
    private GameOnRecognizer gameOnRecognizer;
    private SpellRecognizer spellRecognizer;

    private void Start()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            Permission.RequestUserPermission(Permission.Microphone);

        spellRecognizer = GetComponent<SpellRecognizer>();
        gameOnRecognizer = GetComponent<GameOnRecognizer>();

        GameOnRecognizer._onSpellCast += HandleOnSpellCast;
        SpellRecognizer._onFinishSpellCast += HandleOnFinishSpellCast;
    }

    IEnumerator SwitchSpellToGame()
    {
        spellRecognizer.enabled = false;
        yield return new WaitForSeconds(0.1f);
        gameOnRecognizer.enabled = true;
    }

    IEnumerator SwitchGameToSpell()
    {
        gameOnRecognizer.enabled = false;
        yield return new WaitForSeconds(0.1f);
        spellRecognizer.enabled = true;
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
