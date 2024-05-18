using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Grid _grid = Grid.Instance;
    public static event Delegates.PlayAction OnMove;
    private PlayerController _controller;
    private float _timeMoving = 0.2f;

    private void Start()
    {
        _controller = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W) && _controller.currentState == PlayerState.idle)
        {
            StartCoroutine(Move(0, 1));
        }
        if (Input.GetKey(KeyCode.S) && _controller.currentState == PlayerState.idle)
        {
            StartCoroutine(Move(0, -1));
        }
        if (Input.GetKey(KeyCode.D) && _controller.currentState == PlayerState.idle)
        {
            StartCoroutine(Move(1, 0));
        }
        if (Input.GetKey(KeyCode.A) && _controller.currentState == PlayerState.idle)
        {
            StartCoroutine(Move(-1, 0));
        }
    }

    /// <summary>
    /// Moves the player in the specified direction with the movement restricted by collision.
    /// </summary>
    /// <param name="_x">Horizontal direction.</param>
    /// <param name="_y">Vertical direction.</param>
    /// <returns></returns>
    private IEnumerator Move(int x, int y)
    {
        Vector3 target = transform.position + new Vector3(x, y, 0);

        int gridX = Mathf.FloorToInt(target.x);
        int gridY = Mathf.FloorToInt(target.y);

        _controller.animator.UpdateDirection(x, y);
        if (!_grid.IsCellOccupied(gridX, gridY))
        {
            OnMove?.Invoke();
            _controller._x = gridX;
            _controller._y = gridY;
            _grid.UpdateGrid(new Vector3(gridX, gridY, -1), -1);
            _grid.UpdateGrid(new Vector3(gridX - x, gridY - y, -1), 0);
            _controller.currentState = PlayerState.moving;

            Vector3 originPos;

            float elapsedTime = 0f;

            originPos = transform.position;

            while (elapsedTime < _timeMoving)
            {
                transform.position = Vector3.Lerp(originPos, target, elapsedTime/_timeMoving);
                elapsedTime += Time.deltaTime;
                yield return new WaitForSeconds(_timeMoving/100f);
            }
            yield return null;
            transform.position = target;
            _controller.currentState = PlayerState.idle;
        }
    }
}