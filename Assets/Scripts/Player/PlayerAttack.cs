using System.Collections;
using UnityEngine;

public class PlayerAttack : BaseRecognizer
{
    private PlayerController _controller;
    private Grid _grid;
    private void Awake()
    {
        _grid = Grid.Instance;
        base.actions.Add("Attack", () => { if (_controller.currentState == PlayerState.idle) StartCoroutine(Attack()); });
    }
    private void Start()
    {
        _controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _controller.currentState == PlayerState.idle)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        
        _controller.currentState = PlayerState.attacking;
        _controller.animator.PlayAttackAnim();

        yield return new WaitForSeconds(_controller.animator.GetCurrentAnimationDuration());

        _controller.animator.StopAttackAnim();
        _controller.currentState = PlayerState.idle;
    }
}
