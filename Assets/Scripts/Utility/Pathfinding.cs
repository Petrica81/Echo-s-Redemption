using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Node
{
    public int x;
    public int y;
    public int gCost;
    public int hCost;
    public Node parent;

    public Node(int x, int y)
    {
        this.x = x;
        this.y = y;
        gCost = 1000;
    }
    public int FCost { get { return gCost + hCost; } }
    public bool IsWalkable()
    {
        return !Grid.Instance.IsCellOccupied(x, y);
    }
}
public static class Pathfinding
{
    private static Grid _grid = Grid.Instance;

    public static bool CheckPath(Vector2Int startPosition, Vector2Int targetPosition, int maxDepth)
    {
        Node startNode = new Node(startPosition.x, startPosition.y);
        Node targetNode = new Node(targetPosition.x, targetPosition.y);

        List<Node> open = new List<Node>();
        HashSet<Node> visited = new HashSet<Node>();
        open.Add(startNode);

        startNode.gCost = 0;
        startNode.hCost = GetDistance(startNode, targetNode);
        int depth = 0;

        while (open.Count > 0)
        {
            Node currentNode = open[0];

            for (int i = 1; i < open.Count; i++)
                if (open[i].FCost < currentNode.FCost || (open[i].FCost == currentNode.FCost && open[i].hCost < currentNode.hCost))
                    currentNode = open[i];

            open.Remove(currentNode);
            visited.Add(currentNode);

            if (depth >= maxDepth)
                break;

            if (currentNode == targetNode)
                return true;

            foreach (Node neighbor in GetCloseNeighbors(currentNode))
            {
                if (!neighbor.IsWalkable() || visited.Contains(neighbor))
                    continue;

                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !open.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!open.Contains(neighbor))
                    {
                        open.Add(neighbor);
                    }
                }
            }
            depth++;
        }
        return false;
    }
    public static Vector2Int GetDirection(Vector2Int startPosition, Vector2Int targetPosition, int maxDepth)
    {
        List<Vector2Int> path = FindPath(startPosition, targetPosition, maxDepth);
        return path.Count > 0 ? path[1] - startPosition: new Vector2Int(0,0);
    }
    public static List<Vector2Int> FindPath(Vector2Int startPosition, Vector2Int targetPosition, int maxDepth)
    {
        List<Vector2Int> path =  new List<Vector2Int>();

        Node startNode =  new Node(startPosition.x, startPosition.y);
        Node targetNode = new Node(targetPosition.x, targetPosition.y);

        List<Node> open = new List<Node>();
        HashSet<Node> visited = new HashSet<Node>();
        open.Add(startNode);

        int depth = 0;

        while (open.Count > 0)
        {
            Node currentNode = open[0];

            for (int i = 1; i < open.Count; i++)
                if (open[i].FCost < currentNode.FCost || (open[i].FCost == currentNode.FCost && open[i].hCost < currentNode.hCost))
                    currentNode = open[i];
               
            open.Remove(currentNode);
            visited.Add(currentNode);

            if (depth >= maxDepth)
                break;

            if(currentNode == targetNode)
            {
                path = RetracePath(startNode, targetNode);
                break;
            }

            foreach (Node neighbor in GetCloseNeighbors(currentNode))
            {
                if (!neighbor.IsWalkable() || visited.Contains(neighbor))
                    continue;

                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !open.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!open.Contains(neighbor))
                    {
                        open.Add(neighbor);
                    }
                }
            }
            depth++;
        }
        return path;
    }


    public static List<Node> GetCloseNeighbors(Node node)
    {
        List<Node> nodes = new List<Node>();
        if (_grid.IsInsideGrid(node.x - 1, node.y - 1))
            nodes.Add(new Node(node.x - 1, node.y - 1));
        if (_grid.IsInsideGrid(node.x + 1, node.y - 1))
            nodes.Add(new Node(node.x + 1, node.y - 1));
        if (_grid.IsInsideGrid(node.x - 1, node.y + 1))
            nodes.Add(new Node(node.x - 1, node.y + 1));
        if (_grid.IsInsideGrid(node.x + 1, node.y + 1))
            nodes.Add(new Node(node.x + 1, node.y + 1));
        return nodes;
    }
    public static List<Vector2Int> RetracePath(Node startNode, Node endNode)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(new Vector2Int(currentNode.x, currentNode.y));
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }
    private static int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.x - nodeB.x);
        int dstY = Mathf.Abs(nodeA.y - nodeB.y);
        return dstX + dstY; // Assuming movement cost between adjacent nodes is 1
    }
}
