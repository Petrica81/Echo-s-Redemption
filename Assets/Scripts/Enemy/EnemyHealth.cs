using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private BaseEnemyController _controller;
    private void Start()
    {
        _controller = GetComponent<BaseEnemyController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerWeapon"))
        {
            _controller.Health -= 50;
            Debug.Log($"Remaining enemy health: {_controller.Health}");
        }
    }
}
