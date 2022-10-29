using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Man man))
        {
            man.GetScare();
        }
    }
}
