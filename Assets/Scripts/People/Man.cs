using TMPro;
using QFSW.MOP2;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(NavMeshAgent))]
public class Man : MonoBehaviour
{
    private Transform _transform;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private bool _isCaroling;
    private bool _isNotDelay;
    private float _startSpeed;
    private float _previousPositionX;
    private Vector3 _startScale;

    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PeopleMovementPoints _movementPoints;
    [SerializeField] private Transform _manSprite;
    [SerializeField] private float _delayTimeOnCaroling;
    [SerializeField] private float _delayTimeOnRelaxion;
    [SerializeField] private int _maximumCarrolingChance;
    private IEnumerator _delay;

    [Header("Монолог")]
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private AnimationCurve _textAnimation;
    [SerializeField] private List<string> _sayTexts;
    [SerializeField] private List<string> _scareTexts;
    [SerializeField] private float _sayTime = 1.2f;
    private bool _isSay;
    private float _sayCurrentTime;
    private IEnumerator _say;

    [Header("Испуг")]
    [SerializeField] [Min(.5f)] private float _scareTime;
    [SerializeField] [Min(1)] private float _scareSpeed;
    [SerializeField] private Vector2 _runAwayFromFearMinMaxDistance;
    private IEnumerator _scare;

    [Header("Очки испуга")]
    [SerializeField] private string _fearPoolName;
    [SerializeField] private Vector2 _dropFearsCounMinMax;

    [Header("Позиционирование")]
    [SerializeField] private float _startCalculationPoint;

    public Vector3 Target
    {
        get;
        set;
    }

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _startSpeed = _navMeshAgent.speed;
        _startScale = _manSprite.localScale;
        BuildNewRoot();
    }

    private void Update()
    {
        Move();
    }

    #region movement
    private void Move()
    {

        if (_isNotDelay && Target != Vector3.zero)
        {
            bool isFinishedRoute = GetDistanceToTarget() <= _navMeshAgent.stoppingDistance;
            _navMeshAgent.baseOffset = _startCalculationPoint + transform.localPosition.y;
            _animator.SetBool("isMove", true);
            TurnToMovementDirection();

            if (isFinishedRoute)
            {
                Target = Vector2.zero;
            }
        }
        else if (Target == Vector3.zero)
        {
            _animator.SetBool("isMove", false);
            _isNotDelay = true;
            StartDelay();
        }
    }

    private void TurnToMovementDirection()
    {
        if (_previousPositionX != transform.position.x)
        {
            if (_previousPositionX < transform.position.x)
            {
                _manSprite.localScale = _startScale;
            } else
            {
                _manSprite.localScale = new(-_startScale.x, _startScale.y, _startScale.z);
            }
        }

        _previousPositionX = transform.position.x;
    }

    private float GetDistanceToTarget()
    {
        return Vector2.Distance(Target, _transform.position);
    }

    private void StartDelay()
    {
        StopDelay();
        _delay = Delay(_isCaroling);
        StartCoroutine(_delay);
    }

    private void StopDelay()
    {
        if (_delay != null)
        {
            StopCoroutine(_delay);
        }
    }

    private IEnumerator Delay(bool isCaroling)
    {
        float delayTime = isCaroling ? Random.Range(0, _delayTimeOnCaroling) : Random.Range(0, _delayTimeOnRelaxion);
        yield return new WaitForSeconds(delayTime);
        BuildNewRoot();
    }

    private void BuildNewRoot()
    {
        int _randomChance = Random.Range(0, 100);
        bool isGoCarroling = _randomChance <= _maximumCarrolingChance;

        _isCaroling = isGoCarroling;
        Target = isGoCarroling ? _movementPoints.GetRandomCarolingPoint().position : _movementPoints.GetRandomRelaxionPoint().position;
        _navMeshAgent.SetDestination(Target);

        _isNotDelay = true;
    }

    #endregion

    #region scare

    public void DropFears()
    {
        int count = Mathf.RoundToInt(Random.Range(_dropFearsCounMinMax.x, _dropFearsCounMinMax.y));

        for (int i = 0; i < count; i++)
        {
            GameObject item = MasterObjectPooler.Instance.GetObject(_fearPoolName);
            if (item.TryGetComponent(out Fear fear))
            {
                fear.DropFrom(_transform, _playerMovement);
            }
        }
    }
    public void GetScare()
    {
        DropFears();
        StartScare();
        ScareSayRandom();
        BuildNewScarePosition();
        _navMeshAgent.speed = _scareSpeed;
    }

    private void BuildNewScarePosition()
    {
        float newTargetYPos = Random.Range(0, _runAwayFromFearMinMaxDistance.y);
        float newTargetXPos = Random.Range(0, _runAwayFromFearMinMaxDistance.x);

        if (_manSprite.localScale.x > 0)
        {
            Target = new(transform.localPosition.x - newTargetXPos, transform.localPosition.y + newTargetYPos, 50);
        } else
        {
            Target = new(transform.localPosition.x + newTargetXPos, transform.localPosition.y + newTargetYPos, 50);
        }

        _navMeshAgent.SetDestination(Target);
        _isNotDelay = true;

    }

    private void StartScare()
    {
        StopScare();
        _scare = Scare();
        StartCoroutine(_scare);
    }

    private void StopScare()
    {
        if (_scare != null)
        {
            StopCoroutine(_scare);
        }
    }

    private IEnumerator Scare()
    {
        yield return new WaitForSeconds(_scareTime);
        _navMeshAgent.speed = _startSpeed;
        BuildNewRoot();
    }

    #endregion

    #region say

    public void SayRandom()
    {
        if (!_isSay)
        {
            int index = Random.Range(0, _sayTexts.Count);
            string sayText = _sayTexts[index];
            StartSay(sayText);
        }
    }

    public void ScareSayRandom()
    {
        if (!_isSay)
        {
            int index = Random.Range(0, _scareTexts.Count);
            string sayText = _scareTexts[index];
            StartSay(sayText);
        }
    }

    private void StartSay(string text)
    {
        if (_say != null)
        {
            StopCoroutine(_say);
        }

        _say = Say(text);
        StartCoroutine(_say);
    }

    private IEnumerator Say(string text)
    {

        _text.text = text;
        Transform textTransform = _text.transform;
        float _sayTotalTime = _textAnimation.keys[_textAnimation.length - 1].time;
        _isSay = true;

        while (_sayCurrentTime < _sayTotalTime)
        {
            _sayCurrentTime += Time.deltaTime;
            float newScale = _textAnimation.Evaluate(_sayCurrentTime);
            textTransform.localScale = new Vector3(newScale, newScale, newScale);

            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return new WaitForSeconds(_sayTime);

        _sayTotalTime = 0;
        while (_sayTotalTime < _sayCurrentTime)
        {
            _sayCurrentTime -= Time.deltaTime;
            float newScale = _textAnimation.Evaluate(_sayCurrentTime);
            textTransform.localScale = new Vector3(newScale, newScale, newScale);

            yield return new WaitForSeconds(Time.deltaTime);
        }

        _isSay = false;
    }

    #endregion
}
