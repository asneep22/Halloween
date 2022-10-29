using QFSW.MOP2;
using UnityEngine;

public class ActiveGrave : ActiveItem, IActivatable
{
    private Transform _transform;

    [SerializeField] private string _poolName;
    [SerializeField] private string _zombiePoolName;
    [SerializeField] private string _activateParticlesPoolName;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }
    public void TryActivate()
    {
        try
        {
            SummonZombie();
            PlayActivateParticles();
            MasterObjectPooler.Instance.Release(gameObject, _poolName);
            IsDisactive = false;
        }
        catch (System.Exception e)
        {
            Debug.LogException(e, this);
            throw;
        }
    }

    private void SummonZombie()
    {
        Transform zombieGo = MasterObjectPooler.Instance.GetObject(_zombiePoolName).transform;
        zombieGo.SetParent(_transform.parent);
        zombieGo.localPosition = new(_transform.localPosition.x, _transform.localPosition.y, 0);
        if (zombieGo.TryGetComponent(out Zombie zombie))
        {
            Debug.Log(zombie);
        }

    }

    private void PlayActivateParticles()
    {
        ParticleSystem fearParticles = MasterObjectPooler.Instance.GetObjectComponent<ParticleSystem>(_activateParticlesPoolName);
        fearParticles.transform.position = _transform.position;
        fearParticles.Play();
    }


}
