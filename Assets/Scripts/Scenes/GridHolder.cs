using UnityEngine;

public class GridHolder : MonoBehaviour
{
    private Grid _grid = Grid.Instance;
    [SerializeField]
    private bool _showCircles = false;
    private PlayerController _player;
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
