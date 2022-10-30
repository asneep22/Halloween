using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollowPoints : MonoBehaviour
{
    [SerializeField] private MainCamera _mainCamera;
    [SerializeField] private FadeChanger _fadeChanger;
    [SerializeField] private List<Transform> _followPoints;
    [SerializeField] private int _sceneIndexAfterReading = 2;
    private int _currentIndex = 0;
    private int CurrentIndex
    {
        get => _currentIndex;
        set
        {
            if (value < 0)
            {
                return;
            }

            if (value > _followPoints.Count - 1)
            {
                _fadeChanger.StartFadeInAndChangeScene(_sceneIndexAfterReading);
                return;
            }

            _currentIndex = value;
        }
    }

    [Header("Задержка")]
    [SerializeField][Min(0)] private float _delayTime;
    private bool _isNotDelay = true;
    private IEnumerator _delay;

    private void Update()
    {
        if (_isNotDelay)
        {
            float VerticalMovement = Input.GetAxis("Vertical");

            if (VerticalMovement > 0)
            {
                CameraFollowNext();
                StartDelay();
            }
            else if (VerticalMovement < 0)
            {
                CameraFollowPreviously();
                StartDelay();
            }
        }
    }

    private void CameraFollowPreviously()
    {
        CurrentIndex++;
        _mainCamera.Target = _followPoints[CurrentIndex];
    }

    private void CameraFollowNext()
    {
        CurrentIndex--;
        _mainCamera.Target = _followPoints[CurrentIndex];
    }

    private void StartDelay()
    {
        _delay = Delay();
        StartCoroutine(_delay);
    }

    private IEnumerator Delay()
    {
        _isNotDelay = false;
        yield return new WaitForSeconds(_delayTime);
        _isNotDelay = true;
    }
}
