using QFSW.MOP2;
using System.Collections;
using UnityEngine;

public class ActivePumpkin : ActiveItem, IActivatable
{
    private Transform _transform;

    [SerializeField] private string _poolName;
    [SerializeField] private float _activateTime;
    [SerializeField] private float _fearColliderActiveTime;

    [Header("Коллайдер испуга")]
    [SerializeField] private CircleCollider2D _fearCollider;

    [Header("Анимация")]
    [SerializeField] private AnimationCurve _scaleCurve;
    private float _scaleCurrentTime;
    private IEnumerator _changeScale;
    private Vector3 _startScale;

    [Header("Частицы")]
    [SerializeField] private string _particlesOnActivatePoolName;
    private ParticleSystem _fearParticles;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _startScale = _transform.localScale;
    }

    public void TryActivate()
    {
        try
        {
            IsDisactive = false;
            StartChangeScale();
            StartCoroutine(Activate());
        }
        catch (System.Exception e)
        {
            Debug.LogException(e, this);
            throw;
        }
    }

    private IEnumerator Activate()
    {
        yield return new WaitForSeconds(_activateTime - _fearColliderActiveTime);
        PlayActivateParticles();
        _fearCollider.enabled = true;
        yield return new WaitForSeconds(_fearColliderActiveTime);
        _fearCollider.enabled = false;
        MasterObjectPooler.Instance.Release(gameObject, _poolName);
    }

    #region animation

    private void StartChangeScale()
    {
        StopChangeScale();

        _changeScale = ChangeScale();
        StartCoroutine(ChangeScale());
    }

    private void StopChangeScale()
    {
        if (_changeScale != null)
        {
            StopCoroutine(_changeScale);
        }
    }

    private IEnumerator ChangeScale()
    {
        float totalTime = _scaleCurve.keys[_scaleCurve.length - 1].time;

        while (true)
        {
            if (_scaleCurrentTime >= totalTime)
            {
                _scaleCurrentTime = 0;
            }

            _scaleCurrentTime += Time.deltaTime;
            float newXScale = _scaleCurve.Evaluate(_scaleCurrentTime) + _startScale.x;
            float newYScale = _scaleCurve.Evaluate(_scaleCurrentTime) + _startScale.x;
            _transform.localScale = new(newXScale, newYScale, _transform.localScale.z);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    #endregion

    #region particles
    private void PlayActivateParticles()
    {
        _fearParticles = MasterObjectPooler.Instance.GetObjectComponent<ParticleSystem>(_particlesOnActivatePoolName);
        _fearParticles.transform.position = _transform.position;
        _fearParticles.Play();
    }

    #endregion
}
