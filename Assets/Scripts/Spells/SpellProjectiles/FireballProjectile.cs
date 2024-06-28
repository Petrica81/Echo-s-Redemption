using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballProjectile : Attack
{
    public float _speed;
    public float _lifetime;
    public int _damage;

    public void Start()
    {
        base.Damage = _damage;
    }

    public IEnumerator MoveProjectile(Vector2Int direction)
    {
        float elapsedTime = 0f;
        Vector2 floatDirection = direction;
        while(elapsedTime < _lifetime)
        {
            transform.Translate(floatDirection * _speed * Time.deltaTime, Space.World);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Destroy(transform.parent.gameObject);
    }
    private void OnEnable()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("a intrat");
        Destroy(transform.parent.gameObject, 1f);
        _speed = 0;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("a intrat coll");
        Destroy(transform.parent.gameObject, 1f);
        _speed = 0;
    }
}
