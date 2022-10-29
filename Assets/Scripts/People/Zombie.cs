using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    private Transform _transform;
    private NavMeshAgent _navMeshAgent;
    [SerializeField] private Vector3 _startSize = new(.015f, .015f, .02f);

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }
    private void Start()
    {
        _transform.localScale = _startSize;
        _navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
    }
}
