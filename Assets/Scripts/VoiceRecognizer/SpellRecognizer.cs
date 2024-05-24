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
    public static Delegates.PlayAction _onFinishSpellCast;

    private void CreateDictationRecognizer()
    {
        if (PhraseRecognitionSystem.Status == SpeechSystemStatus.Running)
            PhraseRecognitionSystem.Shutdown();

        dictationRecognizer = new DictationRecognizer(ConfidenceLevel.Low,DictationTopicConstraint.Dictation);

        dictationRecognizer.DictationResult += (_result, _confidence) =>
        {
            Debug.Log($"Dictation result: {_result}");
            text += " " + _result;//TextHelper.FormatText(_result);
            lastSpeechTime = Time.time;
        };

        dictationRecognizer.DictationHypothesis += (_result) =>
        {
            Debug.Log($"Dictation hypothesis: {_result}");
            if (!_result.Split(' ')[0].Equals(lastHypothesisResult.Split(' ')[0]) && text.Split(' ').Last() != lastHypothesisResult.Split(' ').Last())
                text += " " + lastHypothesisResult;
            lastHypothesisResult = _result;
            lastSpeechTime = Time.time;
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
        lastSpeechTime = Time.time;
        StartCoroutine(CheckSilenceTimeout());
    }

    private void OnEnable()
    {
        if (dictationRecognizer == null)
        {
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
            _onFinishSpellCast?.Invoke();
            StopCoroutine(CheckSilenceTimeout());
        }
    }

    private void OnDestroy()
    {
        this.enabled = false;
    }

    private IEnumerator CheckSilenceTimeout()
    {
        while (true)
        {
            if (Time.time - lastSpeechTime > silenceTime && dictationRecognizer != null && dictationRecognizer.Status == SpeechSystemStatus.Running)
            {
                /*if(!text.EndsWith(lastHypothesisResult))
                    text += " " + lastHypothesisResult;*/
                Debug.Log($"Stop Dictation Recognizer! Full text: {text}");
                text = TextHelper.FormatText(text);
                Debug.Log($"Spell Type:{typeClassification.GetType(text)}");
                this.enabled = false;
                break;
            }
            yield return null;
        }
    }
    #endregion
}
