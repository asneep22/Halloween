using QFSW.MOP2;
using System.Collections;
using UnityEngine;

public class ActivePumpkin : ActiveItem, IActivatable
{
    [SerializeField] private string _poolName;
    [SerializeField] private float _activateTime;
    [SerializeField] private float _fearColliderActiveTime;

    [Header("Коллайдер испуга")]
    [SerializeField] private CircleCollider2D _fearCollider;

    [Header("Частицы")]
    [SerializeField] private string _particlesOnCoundownPoolName;
    [SerializeField] private string _particlesOnActivatePoolName;
    private ParticleSystem _countdownParticles;
    private ParticleSystem _activateParticles;

    public void TryActivate()
    {
        try
        {
            IsDisactive = false;
            StartCoroutine(Activate());
        }
        catch (System.Exception e)
        {
            Debug.LogException(e, this);
            throw;
        }
    }

    private void PlayCountdownParticles()
    {
        _countdownParticles = MasterObjectPooler.Instance.GetObjectComponent<ParticleSystem>(_particlesOnCoundownPoolName);
        _countdownParticles.transform.position = transform.position;
        _countdownParticles.Play();
    }

    private void StopCoundownParticles()
    {
        _countdownParticles.Stop();
    }

    private IEnumerator Activate()
    {
        PlayCountdownParticles();
        yield return new WaitForSeconds(_activateTime - _fearColliderActiveTime);
        StopCoundownParticles();
        _fearCollider.enabled = true;
        yield return new WaitForSeconds(_fearColliderActiveTime);
        _fearCollider.enabled = false;
        MasterObjectPooler.Instance.Release(gameObject, _poolName);
    }
}
