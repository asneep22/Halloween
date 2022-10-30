using QFSW.MOP2;
using UnityEngine;

public class ActiveGrave : ActiveItem, IActivatable
{
    private Transform _transform;

    [SerializeField] private string _poolName;
    [SerializeField] private string _zombiePoolName;
    [SerializeField] private string _activateParticlesPoolName;
    [SerializeField] private float _delayBeforeActivateParticels = 0.05f;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    public void TryActivate()
    {
        try
        {
            SummonZombie();
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
        AudioPlayer.PlayRandom(transform, MasterObjectPooler.Instance.GetObjectComponent<AudioSource>(AudioSourcePoolName), ActivateClips);
        Zombie zombie = MasterObjectPooler.Instance.GetObjectComponent<Zombie>(_zombiePoolName);
        zombie.transform.SetParent(_transform.parent);
        zombie.transform.localPosition = new(_transform.localPosition.x, _transform.localPosition.y, 0);

        zombie.Active();
    }


}
