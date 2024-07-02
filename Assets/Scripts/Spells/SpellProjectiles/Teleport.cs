using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : BaseRecognizer
{
    private Grid _grid;
    private void Awake()
    {
        base.actions.Add("Teleport", TeleportPlayer);
    }
    private void Start()
    {
        _grid = Grid.Instance;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T)) {
            TeleportPlayer();
        }
    }
    private void TeleportPlayer()
    {
        int gridX = Mathf.FloorToInt(transform.position.x);
        int gridY = Mathf.FloorToInt(transform.position.y);

        PlayerController controller = PlayerController.Instance;

        if (!_grid.IsCellOccupied(gridX, gridY))
        {
            Vector2Int pos = controller.GetPlayerPosition();
            _grid.UpdateGrid(new Vector3(pos.x, pos.y, -1), 0);
            _grid.UpdateGrid(new Vector3(gridX, gridY, -1), -1);
            controller.transform.position = new Vector3(gridX + 0.5f, gridY + 0.5f, -1);
            Destroy(this.transform.parent.gameObject);
        }
    }
}
