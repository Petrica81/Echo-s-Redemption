using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonsScript : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The sound played when a button click event occurs.")]
    private AudioClip clickSound;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Debug.Log("MainMenu");
    }
    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    public void ButttonSound()
    {
        audioSource.PlayOneShot(clickSound);
    }
}
