using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerState
{
    idle,
    moving,
    attacking
}
public class PlayerController : MonoBehaviour
{
    public PlayerState currentState;
    public PlayerInventory inventory;
    public PlayerAnimator animator;
    private PlayerMovement movement;
    private PlayerAttack attack;
    private Grid _grid = Grid.Instance;
    public int _x;
    public int _y;
    private void Awake()
    {
        currentState = PlayerState.idle;
        animator = GetComponent<PlayerAnimator>();
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
    }

    private void Start()
    {
        animator.PlayIdleAnim(
            inventory.Helmet.Material,
            inventory.Chestplate.Material,
            inventory.Pants.Material
            );

        _x = Mathf.FloorToInt(transform.position.x);
        _y = Mathf.FloorToInt(transform.position.y);

        if (SceneManager.GetActiveScene().name.Contains("MainMenu"))
        {
            movement.enabled = false;
            attack.enabled = false;
            StartMenuButtons._onPlay += HandleOnPlay;
        }
        else
            TilemapGenerator._onTilemapGenerated += HandleOnTilemapGenerated;
    }
    public Vector2Int GetPlayerPosition()
    {
        return new Vector2Int(_x, _y);
    }
    public void HandleOnPlay()
    {
        movement.enabled = true;
        attack.enabled = true;
    }
    public IEnumerator HandleOnTilemapGenerated()
    {
        while(_grid.IsCellOccupied(_x, _y))
        {
            _x = Random.Range(5, _grid.GetSize().x - 5);
            _y = Random.Range(5, _grid.GetSize().y - 5);
        }
        yield return null;
    }
    public void OnDestroy()
    {
        StartMenuButtons._onPlay -= HandleOnPlay;
        TilemapGenerator._onTilemapGenerated -= HandleOnTilemapGenerated;
    }
}
