using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    idle,
    moving,
    attacking,
    charging
}
public abstract class BaseEnemyController : MonoBehaviour
{
    protected Grid _grid = Grid.Instance;
    protected Animator _animator;

    protected Transform _centerPoint;

    protected int _detectionDistance;
    protected int _followDistance;
    protected int _attackRange;
    protected int _patrolRange;
    protected float _timeMoving;
    protected bool _gameOn;

    protected PlayerController _player;
    protected EnemyState _enemyState;

    protected Vector2Int _target;
    protected Vector2Int _origin;
    protected int _x;
    protected int _y;
    
    protected void Start()
    {
        _gameOn = false;
        _animator = GetComponent<Animator>();
        _enemyState = EnemyState.idle;
        _player = FindObjectOfType<PlayerController>();

        _x = Mathf.FloorToInt(transform.position.x);
        _y = Mathf.FloorToInt(transform.position.y);

        _centerPoint = transform;
        _grid.UpdateGrid(_centerPoint.position, 2);
        _origin = new Vector2Int(_x, _y);

    }
    protected void Detect(PlayerController player)
    {
        Debug.Log("detectez...");
        int distance = GetDistance(player._x, player._y);
        Debug.Log($"Distanta: {distance}");
        if (distance <= _detectionDistance && Pathfinding.CheckPath(GetPosition(), player.GetPlayerPosition(), _detectionDistance))
        {
            _target = player.GetPlayerPosition();
            Debug.Log("Am detectat playerul");
        }
        else if (((distance > _followDistance  || !Pathfinding.CheckPath(GetPosition(), player.GetPlayerPosition(), _followDistance)) && _target == player.GetPlayerPosition()) || GetDistance(_target.x, _target.y) < 2)
        {
            SetIdleTarget();
            Debug.Log("Nu am gasit playerul");
        }
    }
    protected void SetIdleTarget()
    {
        int x = 0, y = 0;
        while(_grid.IsCellOccupied(x,y))
        {
            x = _origin.x + Random.Range(-_patrolRange, _patrolRange);
            y = _origin.y + Random.Range(-_patrolRange, _patrolRange);
        }

        _target = new Vector2Int(x, y);
        Debug.Log(_target);
    }
    private int GetDistance(int x, int y)
    {
        int dstX = Mathf.Abs(x - _x);
        int dstY = Mathf.Abs(y - _y);
        return dstX + dstY;
    }
    protected Vector2Int GetPosition()
    {
        return new Vector2Int(_x, _y);
    }
    protected Vector2Int GetRandomDirection()
    {
        Vector2Int target = new Vector2Int(0, 0);
        target.x = Random.Range(-1, 2);
        target.y = target.x == 0 ? Random.Range(-1, 2) : 0;

        while(Grid.Instance.IsCellOccupied(_x + target.x, _y + target.y))
        {
            target.y = Random.Range(-1, 2);
            target.x = target.y == 0 ? Random.Range(-1, 2) : 0;
        }

        return target;
    }
}
