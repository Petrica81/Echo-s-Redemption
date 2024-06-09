using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinish : MonoBehaviour
{
    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.childCount == 0)
            SceneManager.LoadScene("MainMenu");
    }
}
