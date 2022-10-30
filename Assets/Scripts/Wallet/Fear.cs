using QFSW.MOP2;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(YPositionSetterToZOnFixedUpdate))]
public class Fear : MonoBehaviour
{
    private Transform _player;
    private PlayerMovement _playerMovement;
    private Transform _transform;
    private float _waitTime;
    private float _currentWaitTime;

    [SerializeField] private string _poolName;
    [SerializeField] private float _speedOnDrop;
    [SerializeField] private float _speedOnPickUp;
    [SerializeField] private float _distanceToPickUp;
    [SerializeField] private Vector2 _dropDistanceMinMax;
    [SerializeField] private Vector2 _waitTimeMinMax;
    private IEnumerator _moveTo;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    #region drop
    public void DropFrom(Transform transformFrom, PlayerMovement playerMovement)
    {
        _transform.SetParent(transformFrom.parent);

        float dropXPoint = transformFrom.localPosition.x + Random.Range(_dropDistanceMinMax.x, _dropDistanceMinMax.y);
        float dropYPoint = transformFrom.localPosition.y +Random.Range(_dropDistanceMinMax.x, _dropDistanceMinMax.y);

        _transform.localPosition = transformFrom.localPosition;
        _waitTime = Random.Range(_waitTimeMinMax.x, _waitTimeMinMax.y);
        _player = playerMovement.transform;
        _playerMovement = playerMovement;

        StartCoroutine(Drop(dropXPoint, dropYPoint));
    }

    private IEnumerator Drop(float dropXPoint, float dropYPoint)
    {
        Debug.Log(dropXPoint);
        while (_currentWaitTime < _waitTime)
        {
            _currentWaitTime += Time.deltaTime;
            _transform.localPosition = Vector3.Lerp(_transform.localPosition, new Vector3(dropXPoint, dropYPoint, transform.localPosition.z), Time.deltaTime * _speedOnDrop);

            yield return new WaitForSeconds(Time.deltaTime);
        }

        StartMoveTo(_player);
    }

    public void StartMoveTo(Transform target)
    {
        StopMove();

        _moveTo = MoveToPlayer(target);
        StartCoroutine(_moveTo);
    }

    private void StopMove()
    {
        if (_moveTo != null) 
        {
            StopCoroutine(_moveTo);
        }
    }

    private IEnumerator MoveToPlayer(Transform target)
    {
        float distance = Vector2.Distance(_transform.position, target.position);

        while (distance > _distanceToPickUp)
        {
            distance = Vector2.Distance(_transform.position, target.position);

            Vector2 MoveToPlayer = new Vector2(target.localPosition.x - _transform.localPosition.x, target.localPosition.y - _transform.localPosition.y).normalized;
            _transform.Translate(_speedOnPickUp * Time.fixedDeltaTime * MoveToPlayer);
            yield return new WaitForSeconds(.01f);
        }

        if (target == _player)
        {
            _playerMovement.FearsCount++;
        }

        MasterObjectPooler.Instance.Release(gameObject, _poolName);
    }

    #endregion
}
