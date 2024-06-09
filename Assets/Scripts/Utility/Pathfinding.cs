using System.Collections;
using System.Collections.Generic;
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
        gCost = int.MaxValue; // Initialize to a high value
    }

    public int FCost
    {
        get { return gCost + hCost; }
    }

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

        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();
        openList.Add(startNode);

        startNode.gCost = 0;
        startNode.hCost = GetDistance(startNode, targetNode);

        int depth = 0;

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].FCost < currentNode.FCost ||
                    (openList[i].FCost == currentNode.FCost && openList[i].hCost < currentNode.hCost))
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (depth >= maxDepth)
            {
                return false; // Failed to find a path within the depth limit
            }

            if (currentNode.x == targetNode.x && currentNode.y == targetNode.y)
            {
                return true; // Path found
            }

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (!neighbor.IsWalkable() || closedList.Contains(neighbor))
                {
                    continue;
                }

                int newCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newCostToNeighbor < neighbor.gCost || !openList.Contains(neighbor))
                {
                    neighbor.gCost = newCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }

            depth++;
        }

        return false; // No path found
    }

    public static Vector2Int GetDirection(Vector2Int startPosition, Vector2Int targetPosition, int maxDepth)
    {
        List<Vector2Int> path = FindPath(startPosition, targetPosition, maxDepth);
        return path.Count > 1 ? path[1] - startPosition : new Vector2Int(0, 0);
    }

    public static List<Vector2Int> FindPath(Vector2Int startPosition, Vector2Int targetPosition, int maxDepth)
    {
        List<Vector2Int> path = new List<Vector2Int>();

        Node startNode = new Node(startPosition.x, startPosition.y);
        Node targetNode = new Node(targetPosition.x, targetPosition.y);

        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();
        openList.Add(startNode);

        startNode.gCost = 0;
        startNode.hCost = GetDistance(startNode, targetNode);

        int depth = 0;

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].FCost < currentNode.FCost ||
                    (openList[i].FCost == currentNode.FCost && openList[i].hCost < currentNode.hCost))
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (depth >= maxDepth)
            {
                return path; // Return empty path if depth limit is reached
            }

            if (currentNode.x == targetNode.x && currentNode.y == targetNode.y)
            {
                path = RetracePath(startNode, targetNode);
                break;
            }

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (!neighbor.IsWalkable() || closedList.Contains(neighbor))
                {
                    continue;
                }

                int newCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newCostToNeighbor < neighbor.gCost || !openList.Contains(neighbor))
                {
                    neighbor.gCost = newCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }

            depth++;
        }

        return path;
    }

    public static List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0)
                {
                    continue;
                }

                int checkX = node.x + dx;
                int checkY = node.y + dy;

                if (_grid.IsInsideGrid(checkX, checkY))
                {
                    neighbors.Add(new Node(checkX, checkY));
                }
            }
        }

        return neighbors;
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
        return dstX + dstY; // Manhattan distance for grid movement
    }
}
