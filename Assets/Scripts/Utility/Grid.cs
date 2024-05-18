using UnityEngine;

public class Grid
{
    private static Grid _instance;
    private int _height;
    private int _width;

    private int[,] _grid;

    public static Grid Instance
    {
        get
        {
            if (_instance == null)
                _instance = new Grid(100, 100);
            return _instance;
        }
    }
    public Grid(int width, int height)
    {
        _width = width;
        _height = height;

        _grid = new int[width, height];

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                _grid[x, y] = 0;
            }
    }
    public Vector2Int GetSize()
    {
        return new Vector2Int(_width, _height);
    }
    public void UpdateGrid(Vector3 worldPosition, int value)
    {
        int x = Mathf.FloorToInt(worldPosition.x);
        int y = Mathf.FloorToInt(worldPosition.y);

        if (IsInsideGrid(x,y))
        {
            _grid[x, y] = value;
        }
        else
        {
            Debug.LogWarning("You can't upgrade grid at position outside of grid bounds!");
        }
    }
    public bool IsCellOccupied(int x, int y)
    {
        if (_grid[x,y] != 0)
            return true;
        return false;
    }
    public void DrawDebugCircles(Vector2Int position, int range)
    {
        for (int x = position.x - range; x < position.x + range; x++)
            for (int y = position.y - range; y < position.y + range && IsInsideGrid(x,y); y++)
                DrawDebugCircle(new Vector3(x + 0.5f, y + 0.5f, -1), 0.5f, _grid[x, y]);
    }
    private void DrawDebugCircle(Vector3 position, float radius, int value)
    {
        switch (value)
        {
            case -1:
                DebugDraw.DrawCircle(position, radius, Color.white);
                break;
            case 0:
                DebugDraw.DrawCircle(position, radius, Color.green);
                break;
            case 1:
                DebugDraw.DrawCircle(position, radius, Color.red);
                break;
            default:
                DebugDraw.DrawCircle(position, radius, Color.yellow); 
                break;
        }
    }
    public bool IsInsideGrid(int x , int y)
    {
        return x >= 0 && x < _width && y >= 0 && y < _height;
    }
}
