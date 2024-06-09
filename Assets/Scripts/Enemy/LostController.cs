using UnityEngine;
using System.Collections;
using TMPro.EditorUtilities;

public class LostController : BaseEnemyController
{
    private new void Awake()
    {
        base.Awake();
        TilemapGenerator._onTilemapGenerated += HandleOnTilemapGenerated;
    }
    private new void Start()
    {
        base.Start();
        Health = 100;
        Damage = 20;
        _timeMoving = 1f;
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

        Vector2Int playerPos = base._player.GetPlayerPosition();
        if (playerPos.x == base._x + target.x && playerPos.y == base._y + target.y)
        {
            StartCoroutine(_enemyAttack.Attack(base._player, target, Damage));
            yield break;
        }

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

    public IEnumerator HandleOnTilemapGenerated()
    {
        yield return new WaitForSeconds(Random.Range(0f,2f));
        this._gameOn = true;
    }

    private void OnDestroy()
    {
        TilemapGenerator._onTilemapGenerated -= HandleOnTilemapGenerated;
    }
}
