using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class AStarManager : MonoBehaviour
{
    public static AStarManager Instance { get; private set; }

    [SerializeField]
    private WorldBounds _bounds;

    [SerializeField]
    private GameObject _node;

    [SerializeField]
    private GameObject _navMesh;

    [SerializeField]
    private float _resolution;

    private static Node[,] _grid;
    private static List<Node> _walkableNodes;

    private void Awake()
    {
        GenerateGrid();

        Instance = this;
    }

    private void Start()
    {
        foreach (var node in _grid)
        {
            var neighbours = FindNeighbours(node);
            node.SetNeighbours(neighbours);
        }
    }

    private void GenerateGrid()
    {
        var startX = (int)_bounds.Left;
        var startY = (int)_bounds.Bottom;
        var endX = (int)_bounds.Right;
        var endY = (int)_bounds.Top;

        var gridSizeX = Mathf.CeilToInt((endX - startX) / _resolution) + 1;
        var gridSizeY = Mathf.CeilToInt((endY - startY) / _resolution) + 1;

        _grid = new Node[gridSizeX, gridSizeY];
        _walkableNodes = new List<Node>();

        int gridX = 0;
        for (float x = startX; x <= endX; x += _resolution)
        {
            int gridY = 0;
            for (float y = startY; y <= endY; y += _resolution)
            {
                var worldPosition = new Vector3(x, y, 0f);
                var nodeObj = Instantiate(_node, worldPosition, Quaternion.identity, _navMesh.transform);
                var node = nodeObj.GetComponent<Node>();

                node.Initialize(worldPosition, gridX, gridY);
                nodeObj.SetActive(true);

                if (node.Walkable)
                {
                    _walkableNodes.Add(node);
                }

                _grid[node.GridX, node.GridY] = node;

                gridY++;
            }

            gridX++;
        }
    }

    public bool GridContains(int gridX, int gridY)
    {
        if (gridX < 0 || gridY < 0 || gridX > _grid.GetLength(0) - 1 || gridY > _grid.GetLength(1) - 1)
        {
            return false;
        }

        return GetNodeFromGrid(gridX, gridY) != null;
    }

    public Node GetNodeFromGrid(int gridX, int gridY) => _grid[gridX, gridY];

    public Node RandomNode()
    {
        Debug.Log($"Walkable nodes available: {_walkableNodes.Count}");

        return _walkableNodes[Random.Range(0, _walkableNodes.Count)];
    }

    public Node RandomTargetNode(Node startNode)
    {
        var validCount = _walkableNodes.Count;
        var nodeIndex = Random.Range(0, validCount);

        if (_walkableNodes[nodeIndex] == startNode)
        {
            nodeIndex += (nodeIndex < validCount - 1) ? 1 : -1;
        }

        return _walkableNodes[nodeIndex];
    }

    public List<Node> FindNeighbours(Node node)
    {
        var neighbours = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = node.GridX + x;
                int checkY = node.GridY + y;

                if (!GridContains(checkX, checkY)) continue;

                if (x != 0 && y != 0)
                {
                    Node nodeX = GetNodeFromGrid(checkX, node.GridY);
                    Node nodeY = GetNodeFromGrid(node.GridX, checkY);

                    if (!nodeX.Walkable || !nodeY.Walkable) continue;
                }

                var neighbourNode = GetNodeFromGrid(checkX, checkY);

                if (neighbourNode.Walkable)
                {
                    neighbours.Add(GetNodeFromGrid(checkX, checkY));
                }
            }
        }

        return neighbours;
    }

    public List<Node> FindPath(Node startNode, Node targetNode)
    {
        if (!startNode.Walkable || !targetNode.Walkable) return null;

        var toSearch = new List<Node>() { startNode };
        var processed = new List<Node>();

        while (toSearch.Any())
        {
            var current = toSearch[0];

            foreach (var node in toSearch)
            {
                if (node.F <= current.F && node.H < current.H)
                {
                    current = node;
                }
            }

            processed.Add(current);
            toSearch.Remove(current);

            if (current == targetNode)
            {
                var currentPathTile = targetNode;
                var path = new List<Node>();
                while (currentPathTile != startNode)
                {
                    path.Add(currentPathTile);
                    currentPathTile = currentPathTile.Connection;
                }

                return path;
            }

            foreach (var neighbour in current.Neighbours.Where(n => n.Walkable && !processed.Contains(n)))
            {
                var inSearch = toSearch.Contains(neighbour);

                var costToNeighbour = current.G + neighbour.GetDistance(current);

                if (!inSearch || costToNeighbour < neighbour.G)
                {
                    neighbour.SetG(costToNeighbour);
                    neighbour.SetConnection(current);

                    if (!inSearch)
                    {
                        neighbour.SetH(neighbour.GetDistance(targetNode));
                        toSearch.Add(neighbour);
                    }
                }
            }
        }

        return null;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        Gizmos.color = Color.green;
        foreach (var node in _grid)
        {
            if (!node.Walkable) continue;

            foreach (var neighbour in node.Neighbours)
            {
                if (neighbour.Walkable)
                {
                    Gizmos.DrawLine(node.transform.position, neighbour.transform.position);
                }
            }
        }
    }
}
