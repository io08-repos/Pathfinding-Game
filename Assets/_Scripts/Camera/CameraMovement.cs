using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private WorldBounds _bounds;

    [SerializeField]
    private GameObject _player;

    [SerializeField]
    private Camera _camera;
    private Vector2 _cameraHalfSize;

    private void Awake()
    {
        _cameraHalfSize = new Vector2(_camera.orthographicSize * _camera.aspect, _camera.orthographicSize);
    }

    private void Update()
    {
        var position = _player.transform.position;

        position.x = Mathf.Clamp(position.x, _bounds.Left + _cameraHalfSize.x, _bounds.Right - _cameraHalfSize.x);
        position.y = Mathf.Clamp(position.y, _bounds.Bottom + _cameraHalfSize.y, _bounds.Top - _cameraHalfSize.y);
        position.z = _camera.transform.position.z;

        transform.position = position;
    }
}
