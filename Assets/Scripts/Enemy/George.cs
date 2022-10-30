using QFSW.MOP2;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(YPositionSetterToZOnFixedUpdate))]
public class George : MonoBehaviour
{
    private Transform _transform;

    [SerializeField] private string _poolName;
    [SerializeField] private string _fearsPoolName;
    [SerializeField] private float _lifeTime;
    [SerializeField] private float _distanceToStealFears;
    [SerializeField] private float _stelFearesTimeForOne;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private string _activeParticelsPoolName;
    private float _currentTime;

    [Header("Звуки")]
    [SerializeField] private string _audioSourcePoolName = "AudioSource";
    [SerializeField] private List<AudioClip> _activeAudioClips;
    [SerializeField] private List<AudioClip> _disactiveClips;
    public PlayerMovement PlayerMovement
    {
        get;
        set;
    }

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        if (!UIManager.Instance.IsLose)
        {
            Move();
            StealFears();
        }
    }

    private void Move()
    {
        if (PlayerMovement != null)
        {
            transform.Translate(_moveSpeed * Time.deltaTime * (PlayerMovement.transform.position - _transform.position).normalized);
        }
    }

    private void StealFears()
    {
        if (PlayerMovement != null && PlayerMovement.CurrentStage != 2)
        {
            float _distance = Vector2.Distance(PlayerMovement.transform.position, transform.position);

            if (_distance <= _distanceToStealFears)
            {
                if (_currentTime < _stelFearesTimeForOne)
                {
                    _currentTime += Time.deltaTime;
                } else
                {
                    Fear _fear = MasterObjectPooler.Instance.GetObjectComponent<Fear>(_fearsPoolName);
                    _fear.transform.position = PlayerMovement.transform.position;
                    _fear.StartMoveTo(_transform);

                    _currentTime = 0;
                    PlayerMovement.FearsCount--;
                }
            }
        }
    }

    public void Active()
    {
        AudioPlayer.PlayRandom(transform, MasterObjectPooler.Instance.GetObjectComponent<AudioSource>(_audioSourcePoolName), _activeAudioClips);
        PlayActiveParticles();
        Invoke(nameof(Death), _lifeTime);
    }

    private void Death()
    {
        AudioPlayer.PlayRandom(transform, MasterObjectPooler.Instance.GetObjectComponent<AudioSource>(_audioSourcePoolName), _disactiveClips);
        PlayActiveParticles();
        MasterObjectPooler.Instance.Release(gameObject, _poolName);
    }

    private void PlayActiveParticles()
    {
        ParticleSystem _particles = MasterObjectPooler.Instance.GetObjectComponent<ParticleSystem>(_activeParticelsPoolName);
        _particles.transform.position = new(transform.position.x, transform.position.y, transform.position.z - 0.1f);
        _particles.Play();
    }

}


