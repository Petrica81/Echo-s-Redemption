using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The target object the camera will follow.")]
    private Transform target;

    public Transform Target { get => target; set => target = value; }

    private void Start()
    {
        Target = Target?Target:FindObjectOfType<PlayerController>().transform;
        transform.position = new Vector3(target.position.x, target.position.y, -10);
        StartMenuButtons._onPlay += HandleOnPlay;
    }

    void LateUpdate()
    {
        Vector3 newPosition = new Vector3(target.position.x, target.position.y, -10);
        transform.position = Vector3.Lerp(transform.position, newPosition, 3f * Time.deltaTime);
    }
    private void HandleOnPlay()
    {
        Target = FindObjectOfType<PlayerController>().transform;
    }
    private void OnDestroy()
    {
        StartMenuButtons._onPlay -= HandleOnPlay;
    }
}
