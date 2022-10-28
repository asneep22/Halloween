using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private Transform _transform;
    private float _startScaleByX;

    [Header("Ускорение")]
    [SerializeField] private AnimationCurve _speedByTime;
    private Vector2 _movementVector;
    private float _currentTime;


    public Transform Transform
    {
        get => _transform;
    }
    public float StartScaleByX
    {
        get => _startScaleByX;
    }
    public bool CanMove { get; set; }
    public float HorizontalMovement { get => Input.GetAxis("Horizontal"); }
    public float VerticalMovement { get => Input.GetAxis("Vertical"); }
    public AnimationCurve SpeedCurve
    {
        get;
        set;
    }

    private void Start()
    {

        _transform = GetComponent<Transform>();
        _startScaleByX = _transform.localScale.x;

        SpeedCurve = _speedByTime;
        CanMove = true;
    }

    public void FixedUpdate()
    {
        if (CanMove)
        {
            TryMove();
        }
    }

    #region movement

    private void TryMove()
    {
        try
        {
            _movementVector = (Vector2.up * VerticalMovement + Vector2.right * HorizontalMovement) * _speedByTime.Evaluate(GetUpdatedCurrentTime());

            Transform.Translate(_movementVector);
            TurnToMovementDirection();
        }
        catch (System.Exception e)
        {
            Debug.LogException(e, this);
            throw;
        }
       
    }

    private float GetUpdatedCurrentTime()
    {
        if (HorizontalMovement != 0 || VerticalMovement != 0)
        {
            _currentTime += Time.deltaTime;
        }
        else
        {
            _currentTime -= Time.deltaTime;
        }

        _currentTime = Mathf.Clamp(_currentTime, 0, SpeedCurve.keys[_speedByTime.length - 1].time);
        return _currentTime;
    }

    private void UnsetMovement()
    {
        _movementVector = Vector2.zero;
    }

    public void LockMovement()
    {
        UnsetMovement();
        CanMove = false;
    }

    public void UnlockMovement()
    {
        CanMove = true;
    }
    #endregion

    private void TurnToMovementDirection()
    {
        bool isMovemingByX = HorizontalMovement != 0;
        if (isMovemingByX)
        {
            float newScaleByX = HorizontalMovement > 0 ? Mathf.Abs(_startScaleByX) : -Mathf.Abs(_startScaleByX);
            _transform.localScale = new(newScaleByX, _transform.localScale.y, _transform.localScale.z);
        }
    }
}