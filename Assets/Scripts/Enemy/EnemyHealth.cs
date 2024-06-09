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
            _controller.Health -= collision.GetComponent<Attack>().Damage;
            Debug.Log($"Remaining enemy health: {_controller.Health}");
            if (_controller.Health <= 0)
                Destroy(this.gameObject);
        }
    }
}
