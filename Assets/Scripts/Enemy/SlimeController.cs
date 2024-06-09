using System.Collections;
using UnityEngine;

public class SlimeController : BaseEnemyController
{
    // Start is called before the first frame update
    private new void Start()
    {
        base._detectionDistance = 5;
        base._followDistance = 10;
        base._attackRange = 1;
        base._patrolRange = 5;
        base._timeMoving = 0.2f;
        base.Start();
        Debug.Log("Start slime");
        base.SetIdleTarget();
        PlayerMovement.OnMove += () => Detect(_player);
        PlayerMovement.OnMove += () => _gameOn = true;
    }

    private void FixedUpdate()
    {
        if (_gameOn && base._enemyState == EnemyState.idle)
        {
            StartCoroutine(Move());
        }
    }

    private IEnumerator Move()
    {
        base._enemyState = EnemyState.moving;
        yield return new WaitForSeconds(1f);

        Vector2Int currentPos = GetPosition();
        Vector2Int playerPos = _player.GetPlayerPosition();

        // Use A* pathfinding to get the direction towards the player
        Vector2Int targetDirection = Pathfinding.GetDirection(currentPos, playerPos, _followDistance);

        // Log the target direction
        Debug.Log(targetDirection);

        // Update grid for the slime's new position
        _grid.UpdateGrid(new Vector3(base._x, base._y, -1), 0);
        _grid.UpdateGrid(new Vector3(base._x + targetDirection.x, base._y + targetDirection.y, -1), 2);

        Vector2 originPos = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < _timeMoving)
        {
            transform.position = Vector2.Lerp(originPos, originPos + (Vector2)targetDirection, elapsedTime / _timeMoving);
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(_timeMoving / 100f);
        }

        yield return null;
        transform.position = originPos + (Vector2)targetDirection;
        base._x += targetDirection.x;
        base._y += targetDirection.y;
        base._enemyState = EnemyState.idle;
    }
}
