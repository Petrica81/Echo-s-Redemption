using UnityEngine;
using UnityEngine.SceneManagement;

public class Gate : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Scene name to be loaded.")]
    private string _sceneName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            SceneManager.LoadScene(_sceneName);
    }
}
