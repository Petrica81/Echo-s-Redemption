using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Spell
{
    private FireballProjectile _projectile;
    // Start is called before the first frame update
    void Start()
    {
        _projectile = transform.GetChild(0).GetComponent<FireballProjectile>();
        CastSpell();
    }

    private void CastSpell()
    {
        PlayerController _player = PlayerController.Instance;
        Vector2Int direction = _player.animator.GetDirection();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        _projectile.gameObject.SetActive(true);

        StartCoroutine(_projectile.MoveProjectile(direction));
    }
}
