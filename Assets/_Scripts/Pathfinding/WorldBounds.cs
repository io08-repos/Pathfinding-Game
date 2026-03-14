using UnityEngine;

public class WorldBounds : MonoBehaviour
{
    public float Left;
    public float Right;
    public float Top;
    public float Bottom;

    private void OnDrawGizmos()
    {
        var topLeft = new Vector3(Left, Top, 0f);
        var topRight = new Vector3(Right, Top, 0f);
        var bottomLeft = new Vector3(Left, Bottom, 0f);
        var bottomRight = new Vector3(Right, Bottom, 0f);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}

