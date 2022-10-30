using QFSW.MOP2;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ManTrigger : MonoBehaviour, IMorphable
{
    [SerializeField] private Man _man;
    public Man Man
    {
        get => _man;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerMovement playerMovement))
        {
            _man.SayRandom();
        }

        if (collision.TryGetComponent(out FearTrigger fearTrigger))
        {
            _man.GetScare();
        }
    }

    public void Morph(Sprite morphSprite, float morphTime)
    {
        _man.GetScare();
        PlayMorphParticles();
        _man.PlayMorphSound();
        Man.SpriteRenderer.sprite = morphSprite;
        Invoke(nameof(Unmorph), morphTime);
    }

    public void Unmorph()
    {
        PlayMorphParticles();
        _man.PlayMorphSound();
        _man.SpriteRenderer.sprite = _man.StartSprite ;
    }


    private void PlayMorphParticles()
    {
        ParticleSystem _particles = MasterObjectPooler.Instance.GetObjectComponent<ParticleSystem>(_man.MorphParticles);
        _particles.transform.position = transform.position;
        _particles.Play();
    }
}
