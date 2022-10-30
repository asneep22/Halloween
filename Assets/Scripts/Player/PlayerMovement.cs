using QFSW.MOP2;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    private float _startScaleByX;

    [Header("Стадии")]
    [SerializeField] private string _switchStagesParticlesPoolName = "PoofParticles";
    [SerializeField] private float _additionalSpeedByStage;
    [SerializeField] private int _needableFearsToSecondStage;
    [SerializeField] private int _needableFearsToThirdStage;
    [SerializeField] private SpriteRenderer _firstStage;
    [SerializeField] private GameObject _secondStage;
    [SerializeField] private GameObject _thirdStage;
    private int _currentStage;
    public int CurrentStage
    {
        get => _currentStage;
        private set => _currentStage = value;
    }

    [Header("Собранные страхи")]
    [SerializeField] private int _fearsCount;
    [SerializeField] private int _maxFearsCount;

    public int FearsCount
    {
        get => _fearsCount;
        set
        {
            if (!UIManager.Instance.IsLose && !UIManager.Instance.IsWin)
            {
                if (_fearsCount < value)
                {
                    AudioPlayer.PlayRandom(transform, MasterObjectPooler.Instance.GetObjectComponent<AudioSource>(_audioSourcePoolName), _pickUpFearsClips);
                } else
                {
                    AudioPlayer.PlayRandom(transform, MasterObjectPooler.Instance.GetObjectComponent<AudioSource>(_audioSourcePoolName), _dropFearsClips);
                }

                UIManager.Instance.FearsCount.text = $"{value}";

                if (value <= 0)
                {
                    Lose();
                    return;
                } else if (value >= _maxFearsCount)
                {   

                    Win();
                    return;
                }

                _fearsCount = value;
                TryUpdateStage();
            }
        }
    }

    [Header("Ускорение")]
    [SerializeField] private AnimationCurve _speedByTime;
    private Vector2 _movementVector;
    private float _currentTime;

    [Header("Звуки")]
    [SerializeField] private string _audioSourcePoolName = "AudioSource";
    [SerializeField] private List<AudioClip> _pickUpFearsClips;
    [SerializeField] private List<AudioClip> _dropFearsClips;

    public Transform Transform
    {
        get => _transform;
    }
    public float StartScaleByX
    {
        get => _startScaleByX;
    }
    public bool CanMove { get; set; }
    public float HorizontalMovement { get => Input.GetAxis("Horizontal"); }
    public float VerticalMovement { get => Input.GetAxis("Vertical"); }
    public AnimationCurve SpeedCurve
    {
        get;
        set;
    }

    private void Start()
    {
        _startScaleByX = _transform.localScale.x;

        SpeedCurve = _speedByTime;
        CanMove = true;
        _currentStage = 0;
    }

    public void FixedUpdate()
    {
        if (CanMove)
        {
            TryMove();
        }
    }

    private void Win()
    {
        UIManager.Instance.IsWin = true;
    }

    private void Lose()
    {
        UIManager.Instance.IsLose = true;
    }

    #region movement
    private void TryMove()
    {
        try
        {
            _movementVector = (Vector2.up * VerticalMovement + Vector2.right * HorizontalMovement) * (_speedByTime.Evaluate(GetUpdatedCurrentTime()) + _additionalSpeedByStage * _currentStage);

            transform.Translate(_movementVector);
            TurnToMovementDirection();
        }
        catch (System.Exception e)
        {
            Debug.LogException(e, this);
            throw;
        }
       
    }
    private float GetUpdatedCurrentTime()
    {
        if (HorizontalMovement != 0 || VerticalMovement != 0)
        {
            _currentTime += Time.deltaTime;
        }
        else
        {
            _currentTime -= Time.deltaTime;
        }

        _currentTime = Mathf.Clamp(_currentTime, 0, SpeedCurve.keys[_speedByTime.length - 1].time);
        return _currentTime;
    }

    private void UnsetMovement()
    {
        _movementVector = Vector2.zero;
    }

    public void LockMovement()
    {
        UnsetMovement();
        CanMove = false;
    }

    public void UnlockMovement()
    {
        CanMove = true;
    }

    private void TurnToMovementDirection()
    {
        bool isMovemingByX = HorizontalMovement != 0;
        if (isMovemingByX)
        {
            float newScaleByX = HorizontalMovement > 0 ? Mathf.Abs(_startScaleByX) : -Mathf.Abs(_startScaleByX);
            Vector3 newScale = new(newScaleByX, _transform.localScale.y, _transform.localScale.z);
            _firstStage.transform.localScale = newScale;
            _secondStage.transform.localScale = newScale;
            _thirdStage.transform.localScale = newScale;
        }
    }

    #endregion

    #region stages

    private void TryUpdateStage()
    {
        try
        {
            if (_fearsCount > _needableFearsToThirdStage && _currentStage != 2)
            {
                EnableStageThree();
            }
            else if (_fearsCount > _needableFearsToSecondStage && _fearsCount < _needableFearsToThirdStage && _currentStage != 1)
            {
                EnableStageTwo();
            }
            else if (_fearsCount < _needableFearsToSecondStage && _currentStage != 0)
            {
                EnableStageOne();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogException(e, this);
            throw;
        }

    }

    private void EnableStageOne()
    {
        PlayChangeStageParticles();
        _thirdStage.SetActive(false);
        _secondStage.SetActive(false);
        _firstStage.enabled = true;
        _currentStage = 0;
    }

    private void EnableStageTwo()
    {
        PlayChangeStageParticles();
        _thirdStage.SetActive(false);
        _secondStage.SetActive(true);
        _firstStage.enabled = false;
        _currentStage = 1;
    }

    private void EnableStageThree()
    {
        PlayChangeStageParticles();
        _thirdStage.SetActive(true);
        _secondStage.SetActive(false);
        _secondStage.SetActive(false);
        _currentStage = 2;
    }

    private void PlayChangeStageParticles()
    {
        ParticleSystem _particles = MasterObjectPooler.Instance.GetObjectComponent<ParticleSystem>(_switchStagesParticlesPoolName);
        _particles.transform.position = transform.position;
        _particles.Play();
    }

    #endregion
}