using System.Collections;
using UnityEngine.AI;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

[RequireComponent(typeof(NavMeshAgent))]
public class Man : MonoBehaviour
{
    private Transform _man;
    private bool _isCaroling;
    private bool _isNotDelay;
    private IEnumerator _delay;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;

    [SerializeField] private PeopleMovementPoints _movementPoints;
    [SerializeField] private float _delayTimeOnCaroling;
    [SerializeField] private float _delayTimeOnRelaxion;
    [SerializeField] private int _maximumCarrolingChance;

    [Header("Монолог")]
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private List<string> _sayTexts;
    [SerializeField] private float _sayTime = 1.2f;
    [SerializeField] private AnimationCurve _textAnimation;
    private float _sayCurrentTime;
    private bool _isSay;
    private IEnumerator _say;


    [Header("Позиционирование")]
    [SerializeField] private float _startCalculationPoint;

    public Transform Target
    {
        get;
        set;
    }

    private void Start()
    {
        _man = GetComponent<Transform>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _animator = GetComponent<Animator>();
        BuildNewRoot();
    }

    private void Update()
    {
        Move();
    }

    #region movement
    private void Move()
    {
        if (_isNotDelay && Target != null)
        {
            _animator.SetBool("isMove", true);

            _navMeshAgent.baseOffset = _startCalculationPoint + transform.localPosition.y;
            bool isFinishedRoute = GetDistanceToTarget() <= _navMeshAgent.stoppingDistance;

            if (isFinishedRoute)
            {
                _animator.SetBool("isMove", false);
                Target = null;
                _isNotDelay = true;
                StartDelay();
            }
        }
    }

    private float GetDistanceToTarget()
    {
        return Vector2.Distance(Target.position, _man.position);
    }

    private void StartDelay()
    {
        if (_delay != null)
        {
            StopCoroutine(_delay);
        }

        _delay = Delay(_isCaroling);
        StartCoroutine(_delay);
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
        Target = isGoCarroling ? _movementPoints.GetRandomCarolingPoint() : _movementPoints.GetRandomRelaxionPoint();
        _navMeshAgent.SetDestination(Target.position);

        _isNotDelay = true;
    }

    #endregion

    #region fear

    public void GetScare()
    {
        Debug.Log("Scare");
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
