using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CanvasMenuLogic : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The panel which will pop up on pausing.")]
    private GameObject _pausePanel;

    [SerializeField]
    [Tooltip("The panel which contains the settings of the game")]
    private GameObject _settingsPanel;

    private bool _gameIsPaused;

    public static Delegates.PlayActionOneArg _onMagicWordChanged;

    [SerializeField] private GameObject pauseMenuFirst;
    [SerializeField] private GameObject settingsMenuFirst;
    private void Start()
    {
        if (SceneManager.GetActiveScene().name.Contains("MainMenu"))
        {
            gameObject.SetActive(false);
            StartMenuButtons._onPlay += HandleOnPlay;
        }
        _gameIsPaused = false;
        _pausePanel.SetActive(false);
        _settingsPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            if (_gameIsPaused) ResumeGame();
            else PauseGame();
        if ((Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.UpArrow)) && EventSystem.current.currentSelectedGameObject == null)
        {
            if(_pausePanel.gameObject.active == true)
                EventSystem.current.SetSelectedGameObject(pauseMenuFirst);
            if (_settingsPanel.gameObject.active == true)
                EventSystem.current.SetSelectedGameObject(settingsMenuFirst);
        }
    }

    public void ResumeGame()
    {
        _gameIsPaused = false;
        Time.timeScale = 1f;
        _pausePanel.SetActive(false);
        _settingsPanel.SetActive(false);
    }
    public void PauseGame()
    {
        _gameIsPaused = true;
        Time.timeScale = 0f;
        _pausePanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(pauseMenuFirst);
    }
    public void Settings()
    {
        _pausePanel.SetActive(false);
        _settingsPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(settingsMenuFirst);
    }
    public void BackToPause()
    {
        _settingsPanel.SetActive(false);
        _pausePanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(pauseMenuFirst);
    }
    public void Quit()
    {
        EditorApplication.isPlaying = false;
        Application.Quit();
    }
    public void Exit()
    {
        _gameIsPaused = false;
        Time.timeScale = 1f;
        _pausePanel.SetActive(false);
        _settingsPanel.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }
    public void OnInputFieldValueChanged(string inputText)
    {
        if(inputText.Length > 0)
        {
            _onMagicWordChanged?.Invoke(inputText);
        }
    }
    private void HandleOnPlay()
    {
        gameObject.SetActive(true);
    }
    private void OnDestroy()
    {
        StartMenuButtons._onPlay -= HandleOnPlay;
    }
}
