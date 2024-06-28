using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class TilemapGenerator : MonoBehaviour
{
    #region Data
    private Grid grid;

    public static Delegates.PlayActionCoro _onTilemapGenerated;

    [SerializeField]
    [Tooltip("Characters and players can move on this tilemap.")]
    private Tilemap groundTilemap;
    [SerializeField]
    [Tooltip("Characters and players can't move on this tilemap.")]
    private Tilemap collisionTileMap;

    [SerializeField]
    private TileBase grassTile; // 1
    [SerializeField]
    private TileBase grassDecoTile; 
    [SerializeField]
    private TileBase groundTile; // 2
    [SerializeField]
    private TileBase groundDecoTile;
    [SerializeField]
    private TileBase treeTile; // 3

    [SerializeField]
    private int groundChance;

    [SerializeField]
    private GameObject _enemy;
    [SerializeField]
    private GameObject _enemy2;
    [SerializeField]
    private Transform enemies;

    [SerializeField]
    private int mapWidth;

    [SerializeField]
    private int mapHeight;
    private int[,] groundMap;
    private int[,] collisionMap;
    #endregion

    private void Awake()
    {
        grid = Grid.Instance;
    }
    void Start()
    {
        GenerateMap();

        PopulateMapWithHoles(groundChance);
        SmoothTilesMooreN(5, ref groundMap, 2);
        CutHoles();
        Outline(groundMap, ref groundTilemap, 2, 2, -1);

        PopulateMapWithTrees(30);
        SmoothTilesMooreN(5, ref collisionMap, 1);
        CutTrees();


        UpdateTilemap(groundMap,ref groundTilemap, 0);
        UpdateTilemap(collisionMap, ref collisionTileMap, 0);

        UpdateTilemapWithDeco(groundMap, 1, 0, grassDecoTile, 6);
        UpdateTilemapWithDeco(groundMap, 2, 0, groundDecoTile, 4);

        SpawnEnemies();

        UpdateGrid();

        if (_onTilemapGenerated != null)
        {
            foreach (var dele in _onTilemapGenerated.GetInvocationList())
            {
                StartCoroutine((IEnumerator)dele.DynamicInvoke());
            }
        }
    }

    private void FixedUpdate()
    {
        if (enemies.childCount <= 0)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
    private void GenerateMap()
    {
        groundMap = new int[mapWidth, mapHeight];
        collisionMap = new int[mapWidth, mapHeight];
        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                groundMap[i, j] = 1;
            }
        } 
        for (int i = 0; i <= 5; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                collisionMap[i, j] = 1;
                collisionMap[mapHeight - i - 1, j] = 1;
                collisionMap[j, i] = 1;
                collisionMap[j, mapWidth - i - 1] = 1;
            }

        }
    }
    private void PopulateMapWithHoles(int _percent)
    {
        for (int i = 6; i < mapHeight - 7; i++)
        {
            for (int j = 6; j < mapWidth - 7; j++)
            {
                if (groundMap[i, j] == 1 && collisionMap[i, j] == 0 && Random.Range(0, 100) < _percent)
                {
                    groundMap[i, j] = 2;
                }
            }
        }
    }
    private void CutHoles()
    {
        for (int i = 5; i < mapWidth - 6; i++)
        {
            for (int j = 5; j < mapHeight - 6; j++)
            {
                int _numberOfTiles = GetMooreNeighbourhoodTiles(i, j, groundMap, 2);

                if (_numberOfTiles < 4 && groundMap[i, j] == 2)
                    groundMap[i, j] = 1;
            }
        }
    }
    private void Outline(int[,] _map,ref Tilemap _tilemap, int _tileOutlined, int _tileToOutline, int _z)
    {
        int[,] _tempMap = new int[mapWidth, mapHeight];
        for (int i = 5; i < mapHeight - 6; i++)
        {
            for (int j = 5; j < mapWidth - 6; j++)
            {
                if (GetMooreNeighbourhoodTiles(i, j, _map, _tileOutlined) != 0)
                    _tempMap[i, j] = _tileToOutline;
            }
        }
        UpdateTilemap(_tempMap,ref _tilemap,_z);
    }
    private void PopulateMapWithTrees(int _percent)
    {
        for (int i = 5; i < mapWidth - 6; i++)
        {
            for (int j = 5; j < mapHeight - 6; j++)
            {
                if (groundMap[i, j] == 1 &&
                    collisionMap[i, j] == 0 &&
                    Random.Range(0, 100) < _percent &&
                    GetMooreNeighbourhoodTiles(i, j, groundMap, 2) == 0
                    )
                {
                    collisionMap[i, j] = 1;
                }
            }
        }
    }
    private void CutTrees()
    {
        for (int i = 5; i < mapWidth - 6; i++)
        {
            for (int j = 5; j < mapHeight - 6; j++)
            {
                int _numberOfTiles = GetMooreNeighbourhoodTiles(i, j, collisionMap, 1);

                if ( (_numberOfTiles < 4 && collisionMap[i, j] == 1) || groundMap[i, j] != 1 || GetMooreNeighbourhoodTiles(i, j, groundMap, 2) != 0)
                    collisionMap[i, j] = 0;
            }
        }
    }
    private void SmoothTilesMooreN(int _iterations, ref int[,] _map, int _tile)
    {
        for (int k = 0; k < _iterations; k++) 
        {
            for (int i = 5; i < mapWidth - 5; i++)
            {
                for (int j = 5; j < mapHeight - 5; j++)
                {

                    int _numberOfTiles = GetMooreNeighbourhoodTiles(i, j, _map, _tile);

                    if (_numberOfTiles > 4)
                        _map[i, j] = _tile;
                }
            }
        }
    }
    private int GetMooreNeighbourhoodTiles(int _x, int _y, int[,] _map, int _tile)
    {
        int _tiles = 0;
        for (int x = _x - 1; x <= _x +1; x++)
        {
            for (int y = _y - 1; y <= _y + 1; y++)
            {
                if ( (x != _x || y != _y) && _map[x,y] == _tile)
                {
                    _tiles++;
                }
            }
        }
        return _tiles;
    }
    private void UpdateTilemap(int[,] _map, ref Tilemap _tileMap, int _z)
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Vector3Int location = new Vector3Int(x, y, _z);
                if (_map[x, y] != 0)
                {
                    _tileMap.SetTile(location, GetTile(_map[x, y], _tileMap.name));
                }
            }
        }
    }
    private void UpdateTilemapWithDeco(int[,] _map, int _mapNr, int _z, TileBase _tile, int _nrOfNeighbourhoodTiles)
    {
        for (int x = 1; x < mapWidth - 1; x++)
        {
            for (int y = 1; y < mapHeight - 1; y++)
            {
                Vector3Int location = new Vector3Int(x, y, _z);
                if (_map[x, y] == _mapNr && collisionMap[x,y] == 0 && GetMooreNeighbourhoodTiles(x, y, _map, _mapNr) >= _nrOfNeighbourhoodTiles)
                {
                    collisionTileMap.SetTile(location, _tile);
                }
            }
        }
    }
    private TileBase GetTile(int _tile,string _tileMap)
    {
        switch (_tileMap.ToLower())
        {
            case "ground":
                switch (_tile)
                {
                    case 1:
                        return grassTile;
                    case 2:
                        return groundTile;
                    default:
                        return null;
                }
            case "collision":
                switch (_tile)
                {
                    case 1:
                        return treeTile;
                    default: 
                        return null;
                }
            default: 
                return null;
        }   
    }
    private void UpdateGrid()
    {
        for (int x = 0; x < mapHeight; x++)
            for (int y = 0; y < mapWidth; y++)
            {
                if (collisionMap[x, y] != 0)
                    grid.UpdateGrid(new Vector3(x + 0.5f, y + 0.5f, -1), 1);
                else
                    grid.UpdateGrid(new Vector3(x + 0.5f, y + 0.5f, -1), 0);
                
            }
    }

    private void SpawnEnemies()
    {
        for (int x = 0; x < mapHeight; x++)
            for (int y = 0; y < mapWidth; y++)
            {
                if (collisionMap[x, y] == 0 && Random.Range(1, 100) > 98)
                    Instantiate(_enemy, new Vector3(x + 0.5f, y + 0.5f, 0f), Quaternion.identity, enemies);
                else 
                    if (collisionMap[x, y] == 0 && Random.Range(1, 100) > 98)
                        Instantiate(_enemy2, new Vector3(x + 0.5f, y + 0.5f, 0f), Quaternion.identity, enemies);
            }
    }
}
