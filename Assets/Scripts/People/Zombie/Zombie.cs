using QFSW.MOP2;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(CircleCollider2D))]
public class Zombie : MonoBehaviour
{
    private Transform _transform;
    private Transform _target;
    private Vector3 _startSize;

    [SerializeField] private string _particlesPoolName;
    [SerializeField] private string _poolName;
    [SerializeField] private float _lifeTime;

    private NavMeshAgent _navMeshAgent;

    [Header("Позиционирование")]
    [SerializeField] private float _startCalculationPosition;
    public NavMeshAgent NavMeshAgent
    {
        get => _navMeshAgent;
        set => _navMeshAgent = value;
    }
    public Transform Target
    {
        get => _target;
        set => _target = value;
    }

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _startSize = _transform.localScale;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.enabled = false;
        _navMeshAgent.updateRotation = false;
    }

    private void Update()
    {
        if (NavMeshAgent.isOnNavMesh)
        {
            NavMeshAgent.baseOffset = _startCalculationPosition - _transform.position.y;
        } else
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out ManTrigger manTrigger) & NavMeshAgent.isOnNavMesh)
        {
            if (_target == null)
            {
                _target = manTrigger.Man.transform;
            }
            else
            {
                float _currentTargetDistance = Vector2.Distance(transform.localPosition, _target.transform.localPosition);
                float _newTargetPositionDistance = Vector2.Distance(transform.localPosition, manTrigger.Man.transform.localPosition);

                if (_newTargetPositionDistance > _currentTargetDistance)
                {
                    _target = manTrigger.Man.transform;
                }
            }


            NavMeshAgent.SetDestination(new(_target.localPosition.x, _target.localPosition.y, 50));
        }
    }



    public void Active()
    {
        _navMeshAgent.enabled = true;
        PlayParticlesOnZombie();
        StartCoroutine(LossHealthtime());
        _transform.localScale = _startSize;
    }

    private IEnumerator LossHealthtime()
    {
        yield return new WaitForSeconds(_lifeTime);
        Death();
    }

    private void Death()
    {
        _navMeshAgent.enabled = false;
        PlayParticlesOnZombie();
        MasterObjectPooler.Instance.Release(gameObject, _poolName);
    }

    public void PlayParticlesOnZombie()
    {
        ParticleSystem particles = MasterObjectPooler.Instance.GetObjectComponent<ParticleSystem>(_particlesPoolName);
        particles.transform.position = _transform.position;
        particles.Play();
    }

}
