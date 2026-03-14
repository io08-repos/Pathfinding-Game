using System.Collections.Generic;

using UnityEngine;

public class Node : MonoBehaviour
{
    public static LayerMask ObstacleMask;

    private SpriteRenderer _renderer;

    public void Initialize(Vector3 worldPosition, int gridX, int gridY)
    {
        transform.position = worldPosition;
        GridX = gridX;
        GridY = gridY;
    }

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();

        ObstacleMask = LayerMask.GetMask("Obstacle");

        Walkable = OverlapsObstacle();

        _renderer.color = Walkable ? Color.white : Color.gray;
    }

    public List<Node> Neighbours { get; private set; }
    public Node Connection { get; private set; }

    public bool Walkable { get; private set; }

    public static float Radius = 0.125f;
    public int GridX { get; private set; }
    public int GridY { get; private set; }

    public float G { get; private set; }
    public float H { get; private set; }
    public float F => G + H;

    public float GetDistance(Node targetNode) => Vector3.Distance(transform.position, targetNode.transform.position);

    public void SetX(int value) => GridX = value;
    public void SetY(int value) => GridY = value;
    public void SetG(float value) => G = value;
    public void SetH(float value) => H = value;
    public void SetWorldPosition(Vector3 value) => transform.position = value;
    public void SetNeighbours(List<Node> value) => Neighbours = value;
    public void SetConnection(Node value) => Connection = value;

    public bool OverlapsObstacle() => !Physics2D.OverlapCircle(transform.position, Radius, ObstacleMask);
}
