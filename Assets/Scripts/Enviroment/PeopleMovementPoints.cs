using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleMovementPoints : MonoBehaviour
{
    [SerializeField] private List<Transform> _carolingPoints;
    [SerializeField] private List<Transform> _relactionPoints;

    public Transform GetRandomCarolingPoint()
    {
        return _carolingPoints[Random.Range(0, _carolingPoints.Count)];
    }

    public Transform GetRandomRelaxionPoint()
    {
        return _relactionPoints[Random.Range(0, _relactionPoints.Count)];
    }
}
