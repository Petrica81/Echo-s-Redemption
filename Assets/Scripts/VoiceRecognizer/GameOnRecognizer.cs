using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOnRecognizer : BaseRecognizer
{
    private string magicWord = "Spell Cast";
    public string newMagicWord = null;
    public static Delegates.PlayAction _onSpellCast;
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
                _onSpellCast?.Invoke();
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
            _onSpellCast?.Invoke();
            this.enabled = false;
        });
    }

    private void Start()
    {

        StartMenuButtons._onPlay += HandleOnPlay;
        CanvasMenuLogic._onMagicWordChanged += HandleOnMagicWordChanged;
        string _sceneName = SceneManager.GetActiveScene().name;
        if(_sceneName.Contains("MainMenu"))
            this.gameObject.SetActive(false);
        Debug.Log(magicWord);

    }
    public void HandleOnMagicWordChanged(string text)
    {
        MagicWord = text;
        PlayerPrefs.SetString("MagicWord", MagicWord);
        Debug.Log("Magic Word = " + MagicWord);
    }
    public void HandleOnPlay()
    {
        gameObject.SetActive(true);
    }

    public new void OnDestroy()
    {
        base.OnDestroy();
        StartMenuButtons._onPlay -= HandleOnPlay;
        CanvasMenuLogic._onMagicWordChanged -= HandleOnMagicWordChanged;
    }
    public Grimoire _grimoire;
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
            _grimoire.CastSpell("FireballLow");
        if (Input.GetKeyUp(KeyCode.Alpha2))
            _grimoire.CastSpell("WaterCascadeLow");
        if (Input.GetKeyUp(KeyCode.Alpha3))
            _grimoire.CastSpell("EarthquakeLow");
        if (Input.GetKeyUp(KeyCode.Alpha4))
            _grimoire.CastSpell("SpaceballLow");

    }
}

