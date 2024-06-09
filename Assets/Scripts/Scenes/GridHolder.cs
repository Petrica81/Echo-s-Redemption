using UnityEngine;

public class GridHolder : MonoBehaviour
{
    private Grid _grid;
    [SerializeField]
    private bool _showCircles = false;
    private PlayerController _player;
    private void Awake()
    {
        _grid = Grid.Instance;
    }
    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
    }
    // Update is called once per frame
    void Update()
    {
        if (_showCircles)
        {
            _grid.DrawDebugCircles(_player.GetPlayerPosition(), 10);
        }
    }
}
