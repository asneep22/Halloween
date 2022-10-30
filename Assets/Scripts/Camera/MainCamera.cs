using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _speed;
    [SerializeField] private Vector2 _divine;

    public Transform Target
    {
        get => _target;
        set => _target = value;
    }

    void FixedUpdate()
    {
        TryFollow();
    }

    private void TryFollow()
    {
        try
        {
            Vector3 newPosition = new(Target.position.x + _divine.x, Target.position.y + _divine.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * _speed);
        }
        catch (System.Exception e)
        {
            Debug.LogException(e, this);
            throw;
        }
    }

    public void SetPositionTo(Vector2 newPosition)
    {
        transform.position = new Vector3(newPosition.x + _divine.x, newPosition.y + +_divine.y, transform.position.z);
    }


}
