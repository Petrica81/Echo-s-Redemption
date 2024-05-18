using UnityEngine;
using System.Collections;

public class LostController : BaseEnemyController
{
    private new  void Start()
    {
        base.Start();
        _timeMoving = 1f;
        PlayerMovement.OnMove += () => _gameOn = true;
    }
    private void FixedUpdate()
    {
        if (_gameOn && _enemyState == EnemyState.idle)
        {
            StartCoroutine(Move());
        }
    }
    private IEnumerator Move()
    {
        base._enemyState = EnemyState.moving;
        yield return new WaitForSeconds(1f);
        Vector2Int target = base.GetRandomDirection();

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

        _grid.UpdateGrid(new Vector3(_x, _y, -1), 0);
        yield return null;
        transform.position = originPos + target;
        base._x += target.x;
        base._y += target.y;
        base._enemyState = EnemyState.idle;
    }
}
