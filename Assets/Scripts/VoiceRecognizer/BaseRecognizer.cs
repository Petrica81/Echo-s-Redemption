using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public abstract class BaseRecognizer : MonoBehaviour
{
    [Tooltip("Dictionary containing keywords and associated actions.")]
    protected Dictionary<string, Action> actions = new Dictionary<string, Action>();

    private KeywordRecognizer keywordRecognizer;

    protected void CreateKeywordRecognizer()
    {
        if (keywordRecognizer == null)
        {
            keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray(), ConfidenceLevel.Low);
            keywordRecognizer.OnPhraseRecognized += PhraseRecognized;
            keywordRecognizer.Start();
        }
    }
    private void PhraseRecognized(PhraseRecognizedEventArgs _args)
    {
        actions[_args.text].Invoke();
    }
    private void OnEnable()
    {
        CreateKeywordRecognizer();
    }
    private void OnDisable()
    {
        if (keywordRecognizer != null)
        {
            keywordRecognizer.Stop();
            keywordRecognizer.Dispose();
            keywordRecognizer = null;
        }
    }
    protected void Disable()
    {
        if (keywordRecognizer != null)
        {
            keywordRecognizer.Stop();
            keywordRecognizer.Dispose();
            keywordRecognizer = null;
        }
    }
}
