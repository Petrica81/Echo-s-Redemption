using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private BaseEnemyController _controller;

    private void Start()
    {
        _controller = GetComponent<BaseEnemyController>();
    }

    public IEnumerator Attack(PlayerController player, Vector2Int direction, int damage)
    {
        _controller.EnemyState = EnemyState.attacking;

        player.Health -= damage;
        Debug.Log($"Am atacat playerul si i-am dat {damage} dmg!");

        _controller.EnemyState = EnemyState.idle;
        yield return null;
    }
}
