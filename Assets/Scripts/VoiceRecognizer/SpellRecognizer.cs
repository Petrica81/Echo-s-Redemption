using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections;
using System.Linq;

public class SpellRecognizer : MonoBehaviour
{
    #region Dictation Recognizer
    private DictationRecognizer dictationRecognizer;
    private float lastSpeechTime;
    private float silenceTime = 3f;
    private string text = string.Empty;
    private string lastHypothesisResult = string.Empty;
    [SerializeField]
    private TypeClassification typeClassification;
    [SerializeField]
    private Grimoire _grimoire;
    public static Delegates.PlayAction _onFinishSpellCast;
    public static Delegates.PlayAction _onStartSpellCast;
    [SerializeField]

    private void CreateDictationRecognizer()
    {
        if (PhraseRecognitionSystem.Status == SpeechSystemStatus.Running)
            PhraseRecognitionSystem.Shutdown();

        dictationRecognizer = new DictationRecognizer(ConfidenceLevel.Medium,DictationTopicConstraint.Dictation);

        dictationRecognizer.DictationResult += (_result, _confidence) =>
        {
            Debug.Log($"Dictation result: {_result}");
            text += " " + _result;//TextHelper.FormatText(_result);
            lastSpeechTime = Time.unscaledTime;
        };

        dictationRecognizer.DictationHypothesis += (_result) =>
        {
            Debug.Log($"Dictation hypothesis: {_result}");
            if (!_result.Split(' ')[0].Equals(lastHypothesisResult.Split(' ')[0]) && text.Split(' ').Last() != lastHypothesisResult.Split(' ').Last())
                text += " " + lastHypothesisResult;
            lastHypothesisResult = _result;
            lastSpeechTime = Time.unscaledTime;
        };

        dictationRecognizer.DictationComplete += (_completion) =>
        {
            if (_completion != DictationCompletionCause.Complete)
            {
                Debug.Log($"Dictation completed unsuccessfully: {_completion}");
                this.enabled = false;
            }
        };

        dictationRecognizer.DictationError += (_error, _hresult) =>
        {
            Debug.Log($"Dictation error: {_error}; \n HResult: {_hresult}.");
            this.enabled = false;
        };
        dictationRecognizer.Start();
        lastSpeechTime = Time.unscaledTime;
        StartCoroutine(CheckSilenceTimeout());
    }

    private void OnEnable()
    {
        if (dictationRecognizer == null)
        {
            _onStartSpellCast?.Invoke();
            text = string.Empty;
            CreateDictationRecognizer();
        }
    }

    private void OnDisable()
    {
        if (dictationRecognizer != null)
        {
            if(dictationRecognizer.Status == SpeechSystemStatus.Running)
                dictationRecognizer.Stop();
            dictationRecognizer.Dispose();
            dictationRecognizer = null;
            lastHypothesisResult = "";
            StopCoroutine(CheckSilenceTimeout());
        }
    }

    private void OnDestroy()
    {
        if (dictationRecognizer != null)
        {
            if (dictationRecognizer.Status == SpeechSystemStatus.Running)
                dictationRecognizer.Stop();
            dictationRecognizer.Dispose();
            dictationRecognizer = null;
        }
        StopAllCoroutines();
    }

    private IEnumerator CheckSilenceTimeout()
    {
        while (true)
        {
            if (Time.unscaledTime - lastSpeechTime > silenceTime && dictationRecognizer != null && dictationRecognizer.Status == SpeechSystemStatus.Running)
            {
                /*if(!text.EndsWith(lastHypothesisResult))
                    text += " " + lastHypothesisResult;*/
                Debug.Log($"Stop Dictation Recognizer! Full text: {text}");
                text = TextHelper.FormatText(text);
                string spell = typeClassification.GetSpell(text);
                Debug.Log($"Spell :{spell}");
                _onFinishSpellCast?.Invoke();
                if(spell != null) _grimoire.CastSpell(spell);
                this.enabled = false;
                yield break;
            }
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
    #endregion
}
