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
    private Grid _grid;
    public static PlayerController Instance;

    private int _health;
    public int Health { get { return _health; } 
        set {
            Debug.Log($"Aveam {_health} viata, iar acum am {value} viata!");
            if (value <= 0)
            {
                _health = 10;
                SceneManager.LoadScene("MainMenu");
            }
            else 
                _health = value;
        } }

    public int _x;
    public int _y;
    private void Awake()
    {
        currentState = PlayerState.idle;
        animator = GetComponent<PlayerAnimator>();
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
        _grid = Grid.Instance;
        Instance = this;
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
            Health = 10;
        }
        else
        {
            TilemapGenerator._onTilemapGenerated += HandleOnTilemapGenerated;
            Health = 10;
        }
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
        Grid.ResetInstance();
        StartMenuButtons._onPlay -= HandleOnPlay;
        TilemapGenerator._onTilemapGenerated -= HandleOnTilemapGenerated;
    }
}
