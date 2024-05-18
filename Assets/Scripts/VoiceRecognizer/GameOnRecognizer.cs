using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOnRecognizer : BaseRecognizer
{
    private string magicWord = "Spell Cast";
    public string newMagicWord = null;
    public static Delegates.PlayAction onSpellCast;
    public string MagicWord
    {
        get { return magicWord; }
        set
        {
            Disable();
            actions.Remove(magicWord);
            magicWord = value;
            actions.Add(magicWord, () => { 
                Debug.Log($"Magic sequence detected: {magicWord}");
                onSpellCast?.Invoke();
                this.enabled = false;
            });
            CreateKeywordRecognizer();
        }
    }
    private void Awake()
    {
        if (PlayerPrefs.GetString("MagicWord") != string.Empty)
        {
            magicWord = PlayerPrefs.GetString("MagicWord");
        }

        base.actions.Add(MagicWord, () => { 
            Debug.Log($"Magic sequence detected: {MagicWord}");
            onSpellCast?.Invoke();
            this.enabled = false;
        });
    }

    private void Start()
    {

        StartMenuButtons.OnPlay += () => this.gameObject.SetActive(true);
        CanvasMenuLogic._onMagicWordChanged += (string text) =>
        {
            MagicWord = text;
            PlayerPrefs.SetString("MagicWord", MagicWord);
            Debug.Log("Magic Word = " +  MagicWord);
        };
        string _sceneName = SceneManager.GetActiveScene().name;
        if(_sceneName.Contains("MainMenu"))
            this.gameObject.SetActive(false);
        Debug.Log(magicWord);

    }

    /*private void Update()
    {
        if (!string.IsNullOrEmpty(newMagicWord) && newMagicWord.EndsWith("."))
        {
            newMagicWord = newMagicWord.TrimEnd('.');
            Debug.Log($"Input changed from '{MagicWord}' to '{newMagicWord}'.");
            MagicWord = newMagicWord;
            newMagicWord = null;
            PlayerPrefs.SetString("MagicWord", MagicWord);
        }
    }*/
}

