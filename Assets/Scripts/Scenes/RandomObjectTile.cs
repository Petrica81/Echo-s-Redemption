using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Random Object Tile", menuName = "Tiles/Random Object Tile")]
public class RandomObjectTile : TileBase
{
    public Sprite sprite;

    [SerializeField]
    private bool _spawnAtLeastOneObject;

    [SerializeField]
    [Tooltip("Objects to spawn on this tile.")]
    private GameObject[] objects;

    [Tooltip("The chance to be spawned.")]
    [SerializeField]
    private int[] probabilityToSpawn;

    [SerializeField]
    [Tooltip("The number of iterations.")]
    private int[] spawnIterations;


    [SerializeField, Range(0f,0.5f)] private float minX = 0f;
    [SerializeField, Range(0.5f, 1f)] private float maxX = 1f;
    [SerializeField, Range(0f, 0.5f)] private float minY = 0f;
    [SerializeField, Range(0.5f, 1f)] private float maxY = 1f;

    [SerializeField]
    private Tile.ColliderType _colliderType;

    private List<GameObject> _spawnedObjects;

    private Transform parent;
    public override void GetTileData(Vector3Int _location, ITilemap _tilemap, ref TileData _tileData)
    {
        _tileData.sprite = sprite;
        _tileData.colliderType = _colliderType; 
        _spawnedObjects = new List<GameObject>();
    }
    public override bool StartUp(Vector3Int _location, ITilemap _tilemap, GameObject go)
    {
        
        if(Application.isPlaying)
            SpawnRandomObject(_location);
        return base.StartUp(_location, _tilemap, go);
    }
    private void SpawnRandomObject(Vector3Int _location)
    {
        parent = GameObject.Find("Foliage")?.transform;
        if (parent != null)
        {
            if(_spawnAtLeastOneObject){
                var objf = Instantiate(objects[Random.Range(0,objects.Length)], _location + GetSpawnPosition(), Quaternion.identity, parent);
                objf.GetComponent<SpriteRenderer>().sortingOrder = 1;
                //_spawnedObjects.Add(objf);
            }
            for (int g = 0; g < objects.Length; g++)
            {
                for (int k = 0; k < spawnIterations[g]; k++)
                {
                    if (Random.Range(1, 101) <= probabilityToSpawn[g])
                    {
                        var obj = Instantiate(objects[g], _location + GetSpawnPosition(), Quaternion.identity, parent);
                        obj.GetComponent<SpriteRenderer>().sortingOrder = 1;
                        //_spawnedObjects.Add(obj);
                    }
                }
            }
        }
    }
    
    private Vector3 GetSpawnPosition()
    {
        return new Vector3(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY),
            0);
    }
}
