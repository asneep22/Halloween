using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerPOsitionTrigger : MonoBehaviour
{
    [SerializeField] private MainCamera _mainCamera;
    [SerializeField] private Transform _target;
    [SerializeField] private bool _changeX;
    [SerializeField] private bool _changeY;
    [SerializeField] private Vector2 _divine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        float _newXPosition = collision.transform.position.x;
        float _newYPosition = collision.transform.position.y;

        if (_changeX)
        {
            _newXPosition = _target.position.x;
        }

        if (_changeY)
        {
            _newYPosition = _target.position.y;
        }

        collision.transform.position = new Vector3(_newXPosition + _divine.x, _newYPosition + _divine.y, _newYPosition);
        _mainCamera.SetPositionTo(collision.transform.position);
    }
}
