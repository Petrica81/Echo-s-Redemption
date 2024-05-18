using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Scene name to be loaded.")]
    private string sceneName;

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.gameObject.tag == "Player")
            SceneManager.LoadScene(sceneName);
    }
}
