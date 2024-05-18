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
        if(_gameOn && base._enemyState == EnemyState.idle)
        {
            StartCoroutine(Move());
        }
    }
    private IEnumerator Move()
    {
        base._enemyState = EnemyState.moving;
        yield return new WaitForSeconds(1f);
        Vector2Int target = Pathfinding.GetDirection(GetPosition(), _target, _followDistance);

        Debug.Log(target);
        _grid.UpdateGrid(new Vector3(_x, _y, -1), 0);
        _grid.UpdateGrid(new Vector3(_x + target.x, _y + target.y, -1), 2);

        Vector2 originPos;

        float elapsedTime = 0f;

        originPos = transform.position;

        while (elapsedTime < _timeMoving)
        {
            transform.position = Vector2.Lerp(originPos, originPos + target, elapsedTime / _timeMoving);
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(_timeMoving / 100f);
        }
        yield return null;
        transform.position = originPos + target;
        base._x += target.x;
        base._y += target.y;
        base._enemyState = EnemyState.idle; 
    }
}
