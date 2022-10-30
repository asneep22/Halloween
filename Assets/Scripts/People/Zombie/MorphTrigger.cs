using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphTrigger : MonoBehaviour
{
    [SerializeField] private float _morphTime;
    [SerializeField] private Sprite _newSprite;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IMorphable iMorphable))
        {
            iMorphable.Morph(_newSprite, _morphTime);
        }
    }

}
