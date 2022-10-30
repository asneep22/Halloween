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
    private float _scaleCurrentTime;
    private IEnumerator _hint;

    private bool _isItemPlaceFree = true;
    public IActivatable IActivatableItem
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
            IActivatableItem = item.TryGetComponent(out IActivatable Iactivatable) ? Iactivatable : null;
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
        IActivatableItem.TryActivate();
    }

    public void StartShowHint()
    {
        if (_hint != null)
        {
            StopCoroutine(_hint);
        }

        _hint = ShowHint();
        StartCoroutine(_hint);
    }

    public void StopShowHint()
    {
        if (_hint != null)
        {
            StopCoroutine(_hint);
        }

        _hint = HideHint();
        StartCoroutine(_hint);
    }

    private IEnumerator ShowHint()
    {
        float totalTime = _hintScale.keys[_hintScale.length - 1].time;

        while (_scaleCurrentTime < totalTime)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            _scaleCurrentTime += Time.deltaTime;
            float newScale = _hintScale.Evaluate(_scaleCurrentTime);
            _hintButton.transform.localScale = new(newScale, newScale, newScale);
        }
    }

    private IEnumerator HideHint()
    {
        float totalTime = 0;

        while (totalTime < _scaleCurrentTime )
        {
            yield return new WaitForSeconds(Time.deltaTime);
            _scaleCurrentTime -= Time.deltaTime;
            float newScale = _hintScale.Evaluate(_scaleCurrentTime);
            _hintButton.transform.localScale = new(newScale, newScale, newScale);
        }
    }
}
