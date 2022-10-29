using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class FearTrigger : MonoBehaviour
{
    private CircleCollider2D _trigger;
    private void Awake()
    {
        _trigger = GetComponent<CircleCollider2D>();
        _trigger.isTrigger = true;
        _trigger.enabled = false;
    }
}
