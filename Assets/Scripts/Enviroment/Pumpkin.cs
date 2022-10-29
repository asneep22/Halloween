using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Pumpkin : MonoBehaviour
{
    [SerializeField] private float MaxStartFlashingTime = 5;
    [SerializeField] private AnimationCurve _lightIntencity;
    [SerializeField] private Light2D _light;
    private float _currentTime;
    private float _totalTime;
    private IEnumerator _flashing;

    private void Start()
    {
        _totalTime = _lightIntencity.keys[_lightIntencity.length - 1].time;
        float _startFlashingTime = Random.Range(0, MaxStartFlashingTime);

        Invoke(nameof(StartFlashing), _startFlashingTime);
    }

    private void StartFlashing()
    {
        _flashing = Flashing();
        StartCoroutine(_flashing);
    }

    private IEnumerator Flashing()
    {
        while (true)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            _light.intensity = _lightIntencity.Evaluate(_currentTime);
            _currentTime += Time.deltaTime;

            if (_currentTime >= _totalTime)
            {
                _currentTime = 0;
            }
        }
    }
}
