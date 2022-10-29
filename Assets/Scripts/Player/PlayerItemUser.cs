using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemUser : MonoBehaviour
{
    [SerializeField] private Transform _pickUpPoint;
    public BoxCollider2D ItemCollider
    {
        get;
        set;
    }

    [Header("Анимация")]
    [SerializeField] private Transform _hintButton;
    [SerializeField] AnimationCurve _hintScale;

    private bool _isItemPlaceFree = true;
    public IActivatable _IActivatableItem
    {
        get;
        set;
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.F) && !_isItemPlaceFree)
        {
            Throw();
            Activate();
        }
    }

    public void PickUp(Transform item)
    {
        if (_isItemPlaceFree)
        {
            item.transform.position = _pickUpPoint.position;
            _IActivatableItem = item.TryGetComponent(out IActivatable Iactivatable) ? Iactivatable : null;
            item.SetParent(_pickUpPoint);

            _isItemPlaceFree = false;
        }
    }

    public void Throw()
    {
        Transform activeItemTransform = _pickUpPoint.GetChild(0);
        activeItemTransform.SetParent(transform.parent);
        ItemCollider.enabled = true;
        _isItemPlaceFree = true;
    }

    public void Activate()
    {
        _IActivatableItem.TryActivate();
    }

    public void ShowHint()
    {
        Debug.Log("HintToPickUp");
    }

    public void HideHint()
    {
        Debug.Log("HintToPickUp");
    }
}
