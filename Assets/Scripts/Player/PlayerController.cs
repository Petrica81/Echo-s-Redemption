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

        if (SceneManager.GetActiveScene().name.Contains("MainMenu")) {
            movement.enabled = false;
            attack.enabled = false;
            StartMenuButtons.OnPlay += () => movement.enabled = true;
            StartMenuButtons.OnPlay += () => attack.enabled = true;
        }
    }
    public Vector2Int GetPlayerPosition()
    {
        return new Vector2Int(_x, _y);
    }
}
