using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earthquake : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CameraShakeCoroutine());
    }

    private IEnumerator CameraShakeCoroutine()
    {
        Camera camera = Camera.main;
        Vector2Int position; 
        float currentMagnitude = 0.2f;
        while (true)
        {
            position = PlayerController.Instance.GetPlayerPosition();
            float x = (Random.value - 0.5f) * currentMagnitude + position.x;
            float y = (Random.value - 0.5f) * currentMagnitude + position.y;

            camera.transform.localPosition = new Vector3(x, y, -10);

            yield return new WaitForSeconds(0.1f);
        }
    }
    private void OnDestroy()
    {
       StopAllCoroutines();
    }
}
