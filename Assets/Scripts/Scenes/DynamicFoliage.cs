using System.Collections.Generic;
using UnityEngine;

public class DynamicFoliage : MonoBehaviour
{
    [SerializeField]
    private PlayerController _player;
    private int _locationSize = 10;
    private FoliageLocation[,] _locations;

    public class FoliageLocation
    {
        public Vector2Int location;
        public List<GameObject> objects;

        public FoliageLocation(int x , int y)
        {
            location = new Vector2Int(x , y);
            objects = new List<GameObject>();
        }
    }

    private Vector2Int _currentLocation;
    
    void Start()
    {
        CreateLocations();
        GroupFoliageByLocation();
        _currentLocation = new Vector2Int(-1, -1);
        UpdateLocation();
        PlayerMovement.OnMove += UpdateLocation;
    }

    private void CreateLocations()
    {
        int size = 500 / _locationSize;
        _locations = new FoliageLocation[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                _locations[i, j] = new FoliageLocation(i * _locationSize, j * _locationSize);
            }
        }
    }
    private void GroupFoliageByLocation()
    {
        foreach (Transform foliage in transform)
        {
            Vector2Int foliagePos = new Vector2Int(Mathf.FloorToInt(foliage.position.x), Mathf.FloorToInt(foliage.position.y));
            Vector2Int locationIndex = new Vector2Int(foliagePos.x / _locationSize, foliagePos.y / _locationSize);

            if (locationIndex.x >= 0 && locationIndex.x < _locations.GetLength(0) &&
                locationIndex.y >= 0 && locationIndex.y < _locations.GetLength(1))
            {
                _locations[locationIndex.x, locationIndex.y].objects.Add(foliage.gameObject);
                foliage.gameObject.SetActive(false);
            }
        }
    }
    public Vector2Int FindPlayer()
    {
        Vector2Int playerPos = _player.GetPlayerPosition();

        Vector2Int playerLocationIndex = new Vector2Int(playerPos.x / _locationSize, playerPos.y / _locationSize);

        return playerLocationIndex;
    }
    private void UpdateLocation()
    {
        Vector2Int newPlayerLocation = FindPlayer();
        if (_currentLocation != newPlayerLocation)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Vector2Int neighborLocationPlayer = newPlayerLocation + new Vector2Int(x, y);

                    if (neighborLocationPlayer.x >= 0 && neighborLocationPlayer.x < _locations.GetLength(0) &&
                        neighborLocationPlayer.y >= 0 && neighborLocationPlayer.y < _locations.GetLength(1))
                    {
                        SetLocationActive(_locations[neighborLocationPlayer.x, neighborLocationPlayer.y], true);
                    }

                    Vector2Int neighborLocation = _currentLocation + new Vector2Int(x, y);

                    if (neighborLocation.x >= 0 && neighborLocation.x < _locations.GetLength(0) &&
                        neighborLocation.y >= 0 && neighborLocation.y < _locations.GetLength(1) &&
                        CustomDistance(neighborLocation, newPlayerLocation) > 1)
                    {
                        SetLocationActive(_locations[neighborLocation.x, neighborLocation.y], false);
                    }
                }
            }

            _currentLocation = newPlayerLocation;
        }
    }
    private void SetLocationActive(FoliageLocation location, bool active)
    {
        foreach (GameObject foliageObject in location.objects)
        {
            foliageObject.SetActive(active);
        }
    }
    private int CustomDistance(Vector2Int origin, Vector2Int target)
    {
        int distX = Mathf.Abs(origin.x - target.x);
        int distY = Mathf.Abs(origin.y - target.y);

        return distX < distY ? distY : distX;
    }
    private void OnDestroy()
    {
        _locations = new FoliageLocation[1,1];
    }
}
