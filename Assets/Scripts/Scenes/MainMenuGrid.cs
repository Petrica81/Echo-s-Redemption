using UnityEngine;
using UnityEngine.Tilemaps;

public class MainMenuGrid : MonoBehaviour
{
    private Grid _grid;

    [SerializeField]
    private Tilemap _ground;
    [SerializeField]
    private Tilemap _collision;

    private void Awake()
    {
        _grid = Grid.Instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        // Ini?ializeaz? grid-ul pe baza dimensiunii Tilemap-urilor
        Vector3Int gridSize = _ground.size;
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3Int localPlace = new Vector3Int(x, y, 0);
                Vector3 worldPosition = _ground.CellToWorld(localPlace);

                if (_ground.HasTile(localPlace))
                {
                    _grid.UpdateGrid(worldPosition, 0);
                }

                if (_collision.HasTile(localPlace))
                {
                    _grid.UpdateGrid(worldPosition, 1);
                }
            }
        }
    }
}
