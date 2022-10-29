using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ActiveItem : MonoBehaviour
{
    private BoxCollider2D _boxCollider2D;
    [SerializeField] private PlayerItemUser _itemUser;

    public bool IsPlayerInThePickUpZone
    {
        get;
        set;
    }
    public bool IsDisactive
    {
        get;
        set;
    } = true;

    private void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _boxCollider2D.isTrigger = true;
    }

    private void Update()
    {
        if (IsPlayerInThePickUpZone && IsDisactive)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                _itemUser.PickUp(transform);
                _itemUser.ItemCollider = _boxCollider2D;
                _boxCollider2D.enabled = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerItemUser itemUser))
        {
            _itemUser = itemUser;
            IsPlayerInThePickUpZone = true;
            itemUser.ShowHint();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerItemUser itemUser))
        {
            _itemUser = null;
            IsPlayerInThePickUpZone = false;
            itemUser.HideHint();
        }
    }
}
