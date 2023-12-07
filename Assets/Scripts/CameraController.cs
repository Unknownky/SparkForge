using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _camera;

    public float leap = 0.1f;
    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _camera.orthographicSize -= leap;
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, 3f, 10f);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            _camera.orthographicSize += leap;
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, 3f, 10f);
        }
    }

}
